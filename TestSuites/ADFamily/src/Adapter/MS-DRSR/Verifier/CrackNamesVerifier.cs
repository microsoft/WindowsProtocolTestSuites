// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices.Protocols;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    public partial class DRSRVerifier : IDRSRVerifier
    {
        #region Verify CrackNames Responses

        // Verify DS_LIST_SITES of IDL_DRSCrackNames
        void VerifyCrackNamesListAllSites(
            DsServer dc,
            DRS_MSG_CRACKREQ req,
            DRS_MSG_CRACKREPLY? reply)
        {
            DsSite[] sites = ldapAd.ListSites(dc);

            testSite.Assert.IsTrue(
                reply.Value.V1.pResult[0].cItems == sites.Length,
                "IDL_DRSCrackNames: DS_LIST_SITES: wrong site count, expect:{0}, got:{1}",
                sites.Length,
                reply.Value.V1.pResult[0].cItems
            );

            string[] siteDns = new string[sites.Length];
            for (int i = 0; i < sites.Length; ++i)
                siteDns[i] = sites[i].DN;

            string[] listSiteDns = new string[reply.Value.V1.pResult[0].cItems];
            for (int i = 0; i < reply.Value.V1.pResult[0].cItems; ++i)
            {
                testSite.Assert.IsTrue(
                    reply.Value.V1.pResult[0].rItems[i].status == DS_NAME_ERROR.DS_NAME_NO_ERROR,
                    "IDL_DRSCrackNames: DS_LIST_SITES: return status should be 0, got {0}",
                    reply.Value.V1.pResult[0].rItems[i].status
                    );

                listSiteDns[i] = reply.Value.V1.pResult[0].rItems[i].pName;
            }

            testSite.Assert.IsTrue(
                DrsrHelper.IsStringArrayEqual(siteDns, listSiteDns),
                "IDL_DRSCrackNames: DS_LIST_SITES: failed to verify site DNs"
                );
        }

        void VerifyCrackNamesListRoles(
            DsServer dc,
            DRS_MSG_CRACKREQ req,
            DRS_MSG_CRACKREPLY? reply)
        {
            bool isLds = (dc is AdldsServer);
            DsDomain domain = dc.Domain;

            DS_NAME_RESULTW result = reply.Value.V1.pResult[0];

            if (isLds)
            {
                testSite.Assert.IsTrue(
                    result.cItems == 2,
                    "IDL_DRSCrackNames: DS_LIST_ROLES: FSMO role owner of a LDS server should be 2."
                    );
            }
            else
            {
                testSite.Assert.IsTrue(
                    result.cItems == 5,
                    "IDL_DRSCrackNames: DS_LIST_ROLES: FSMO role owner of a DS server should be 5."
                    );
            }

            for (int i = 0; i < result.cItems; ++i)
            {
                testSite.Assert.IsTrue(
                    result.rItems[i].status == DS_NAME_ERROR.DS_NAME_NO_ERROR,
                    "IDL_DRSCrackNames: DS_LIST_ROLES: return status should be 0, got {0}",
                    result.rItems[i].status
                    );
            }

            testSite.Assert.IsTrue(
                domain.FsmoRoleOwners[FSMORoles.Schema] == result.rItems[0].pName,
                "IDL_DRSCrackNames: DS_LIST_ROLES: failed to verify schema owner."
                );

            testSite.Assert.IsTrue(
                domain.FsmoRoleOwners[FSMORoles.DomainNaming] == result.rItems[0].pName,
                "IDL_DRSCrackNames: DS_LIST_ROLES: failed to verify domain naming owner."
                );

            // DS server might have 3 more roles.
            if (!isLds)
            {
                testSite.Assert.IsTrue(
                    domain.FsmoRoleOwners[FSMORoles.PDC] == result.rItems[0].pName,
                    "IDL_DRSCrackNames: DS_LIST_ROLES: failed to verify PDCE owner."
                    );
                testSite.Assert.IsTrue(
                    domain.FsmoRoleOwners[FSMORoles.RidAllocation] == result.rItems[0].pName,
                    "IDL_DRSCrackNames: DS_LIST_ROLES: failed to verify RID allocation owner."
                    );
                testSite.Assert.IsTrue(
                    domain.FsmoRoleOwners[FSMORoles.Infrastructure] == result.rItems[0].pName,
                    "IDL_DRSCrackNames: DS_LIST_ROLES: failed to verify Infrastructure owner."
                    );
            }
        }

        void VerifyCrackNamesListDomains(
            DsServer dc,
            DRS_MSG_CRACKREQ req,
            DRS_MSG_CRACKREPLY? reply)
        {

            DsDomain[] domains = ldapAd.ListDomains(dc);

            testSite.Assert.IsTrue(
                reply.Value.V1.pResult[0].cItems == domains.Length,
                "IDL_DRSCrackNames: DS_LIST_DOMAINS: wrong domain count, expect:{0}, got:{1}",
                domains.Length,
                reply.Value.V1.pResult[0].cItems
                );

            string[] domainDns = new string[domains.Length];
            for (int i = 0; i < domains.Length; ++i)
                domainDns[i] = domains[i].Name;

            DS_NAME_RESULTW result = reply.Value.V1.pResult[0];
            string[] listDomainDns = new string[result.cItems];
            for (int i = 0; i < result.cItems; ++i)
            {
                testSite.Assert.IsTrue(
                    result.rItems[i].status == DS_NAME_ERROR.DS_NAME_NO_ERROR,
                    "IDL_DRSCrackNames: DS_LIST_DOMAINS: return status should be 0, got {0}",
                    result.rItems[i].status
                    );
                listDomainDns[i] = result.rItems[i].pName;
            }

            testSite.Assert.IsTrue(
                DrsrHelper.IsStringArrayEqual(domainDns, listDomainDns),
                "IDL_DRSCrackNames: DS_LIST_DOMAINS: failed to verify domain DNs."
                );
        }

        void VerifyCrackNamesListServersInSite(
            DsServer dc,
            DRS_MSG_CRACKREQ req,
            DRS_MSG_CRACKREPLY? reply)
        {
            string siteDn = req.V1.rpNames[0];
            DsSite site = ldapAd.GetSite(dc, siteDn);

            testSite.Assert.IsTrue(
                reply.Value.V1.pResult[0].cItems == site.Servers.Length,
                "IDL_DRSCrackNames: DS_LIST_SERVERS_IN_SITE: wrong server count, expect:{0}, got:{1}",
                site.Servers.Length,
                reply.Value.V1.pResult[0].cItems
                );

            string[] servers = new string[site.Servers.Length];
            for (int i = 0; i < site.Servers.Length; ++i)
                servers[i] = site.Servers[i].NtdsDsaObjectName;

            string[] drsrServers = new string[reply.Value.V1.pResult[0].cItems];
            for (int i = 0; i < reply.Value.V1.pResult[0].cItems; ++i) {
                drsrServers[i] = reply.Value.V1.pResult[0].rItems[i].pName;
                testSite.Assert.IsTrue(
                    reply.Value.V1.pResult[0].rItems[i].status == DS_NAME_ERROR.DS_NAME_NO_ERROR,
                    "IDL_DRSCrackNames: DS_LIST_SERVERS_IN_SITE: return status should be 0, got {0}",
                    reply.Value.V1.pResult[0].rItems[i].status
                    );
            }

            testSite.Assert.IsTrue(
                DrsrHelper.IsStringArrayEqual(servers, drsrServers),
                "IDL_DRSCrackNames: DS_LIST_SERVERS_IN_SITE: failed to verify servers in site {0}",
                siteDn
                );

        }

        void VerifyCrackNamesListDomainsInSite(
           DsServer dc,
           DRS_MSG_CRACKREQ req,
           DRS_MSG_CRACKREPLY? reply)
        {
            string siteDn = req.V1.rpNames[0];
            DsSite site = ldapAd.GetSite(dc, siteDn);
            string[] domains = new string[site.Domains.Length];

            testSite.Assert.IsTrue(
                reply.Value.V1.pResult[0].cItems == domains.Length,
                "IDL_DRSCrackNames: DS_LIST_DOMAINS_IN_SITE: wrong domain count, expect:{0}, got:{1}",
                domains.Length,
                reply.Value.V1.pResult[0].cItems
                );

            for (int i = 0; i < site.Domains.Length; ++i)
                domains[i] = site.Domains[i].Name;

            string[] drsrDomains = new string[reply.Value.V1.pResult[0].cItems];
            for (int i = 0; i < reply.Value.V1.pResult[0].cItems; ++i)
            {
                drsrDomains[i] = reply.Value.V1.pResult[0].rItems[i].pName;
                testSite.Assert.IsTrue(
                    reply.Value.V1.pResult[0].rItems[i].status == DS_NAME_ERROR.DS_NAME_NO_ERROR,
                    "IDL_DRSCrackNames: DS_LIST_DOMAINS_IN_SITE: return status should be 0, got {0}",
                    reply.Value.V1.pResult[0].rItems[i].status
                    );
            }

            testSite.Assert.IsTrue(
                DrsrHelper.IsStringArrayEqual(domains, drsrDomains),
                "IDL_DRSCrackNames: DS_LIST_DOMAINS_IN_SITE: failed to verify servers in site {0}",
                siteDn
                );

        }

        void VerifyCrackNamesListServersForDomainInSite(
            DsServer dc,
            DRS_MSG_CRACKREQ req,
            DRS_MSG_CRACKREPLY? reply)
        {
            string domainDn = req.V1.rpNames[0];
            string siteDn = req.V1.rpNames[1];

            DsServer[] servers = ldapAd.ListServersForDomainInSite(dc, domainDn, siteDn);

            testSite.Assert.IsTrue(
                reply.Value.V1.pResult[0].cItems == servers.Length,
                "IDL_DRSCrackNames: DS_LIST_SERVERS_FOR_DOMAIN_IN_SITE: wrong server count, expect:{0}, got:{1}",
                servers.Length,
                reply.Value.V1.pResult[0].cItems
                );

            string[] serverDns = new string[servers.Length];
            for (int i = 0; i < servers.Length; ++i)
                serverDns[i] = servers[i].ServerObjectName;

            string[] drsrServers = new string[reply.Value.V1.pResult[0].cItems];
            for (int i = 0; i < reply.Value.V1.pResult[0].cItems; ++i)
            {
                drsrServers[i] = reply.Value.V1.pResult[0].rItems[i].pName;
                testSite.Assert.IsTrue(
                    reply.Value.V1.pResult[0].rItems[i].status == DS_NAME_ERROR.DS_NAME_NO_ERROR,
                    "IDL_DRSCrackNames: DS_LIST_SERVERS_FOR_DOMAIN_IN_SITE: return status should be 0, got {0}",
                    reply.Value.V1.pResult[0].rItems[i].status
                    );
            }

            testSite.Assert.IsTrue(
                DrsrHelper.IsStringArrayEqual(serverDns, drsrServers),
                "IDL_DRSCrackNames: DS_LIST_SERVERS_FOR_DOMAIN_IN_SITE: failed to verify servers in site {0}",
                siteDn
                );

        }

        void VerifyCrackNamesListInfoForServer(
            DsServer dc,
            DRS_MSG_CRACKREQ req,
            DRS_MSG_CRACKREPLY? reply)
        {
            string serverDn = req.V1.rpNames[0];
            string dsaObjDn, dnsHostName, serverReference;

            ldapAd.ListInfoForServer(dc, serverDn, out dsaObjDn, out dnsHostName, out serverReference);

            testSite.Assert.IsTrue(
                reply.Value.V1.pResult[0].cItems == 3,
                "IDL_DRSCrackNames: DS_LIST_INFO_FOR_SERVER: result should have 3 items, got {0}.",
                reply.Value.V1.pResult[0].cItems
                );

            string drsrObjDn = reply.Value.V1.pResult[0].rItems[0].pName;
            testSite.Assert.IsTrue(
                drsrObjDn == dsaObjDn,
                "IDL_DRSCrackNames: DS_LIST_INFO_FOR_SERVER: wrong NTDS DSA DN, expect:{0}, got:{1}",
                dsaObjDn,
                drsrObjDn
                );

            string drsrDnsHostName = reply.Value.V1.pResult[0].rItems[1].pName;
            testSite.Assert.IsTrue(
                drsrDnsHostName == dnsHostName,
                "IDL_DRSCrackNames: DS_LIST_INFO_FOR_SERVER: wrong DNS host name, expect:{0}, got:{1}",
                dnsHostName,
                drsrDnsHostName
                );

            string drsrServerReference = reply.Value.V1.pResult[0].rItems[2].pName;
            testSite.Assert.IsTrue(
                drsrServerReference == serverReference,
                "IDL_DRSCrackNames: DS_LIST_INFO_FOR_SERVER: wrong server reference, expect:{0}, got:{1}",
                serverReference,
                drsrServerReference
                );
           
        }

        void VerifyCrackNamesListNcs(
            DsServer dc,
            DRS_MSG_CRACKREQ req,
            DRS_MSG_CRACKREPLY? reply)
        {
            string[] ncs = ldapAd.ListNCs(dc);
            string[] drsrNcs = new string[reply.Value.V1.pResult[0].cItems];
            for (int i = 0; i < reply.Value.V1.pResult[0].cItems; ++i)
            {
                drsrNcs[i] = reply.Value.V1.pResult[0].rItems[i].pName;
                testSite.Assert.IsTrue(
                    reply.Value.V1.pResult[0].rItems[i].status == DS_NAME_ERROR.DS_NAME_NO_ERROR,
                    "IDL_DRSCrackNames: DS_LIST_NCS: return status should be 0, got {0}",
                    reply.Value.V1.pResult[0].rItems[i].status
                    );
            }

            testSite.Assert.IsTrue(
                DrsrHelper.IsStringArrayEqual(ncs, drsrNcs),
                "IDL_DRSCrackNames: DS_LIST_NCS: failed to verify NCs."
                );
        }

        void VerifyCrackNamesListServersWithDcsInSite(
            DsServer dc,
            DRS_MSG_CRACKREQ req,
            DRS_MSG_CRACKREPLY? reply)
        {
            string siteDn = req.V1.rpNames[0];
            DsServer[] servers = ldapAd.ListServersWithDcsInSite(dc, siteDn);

            testSite.Assert.IsTrue(
                reply.Value.V1.pResult[0].cItems == servers.Length,
                "IDL_DRSCrackNames: DS_LIST_SERVERS_WITH_DCS_IN_SITE: wrong server count, expect:{0}, got:{1}",
                servers.Length,
                reply.Value.V1.pResult[0].cItems
            );

            string[] serverDns = new string[servers.Length];
            for (int i = 0; i < servers.Length; ++i)
                serverDns[i] = servers[i].ServerObjectName;

            string[] drsrServerDns = new string[reply.Value.V1.pResult[0].cItems];
            for (int i = 0; i < reply.Value.V1.pResult[0].cItems; ++i)
            {
                drsrServerDns[i] = reply.Value.V1.pResult[0].rItems[i].pName;
                testSite.Assert.IsTrue(
                    reply.Value.V1.pResult[0].rItems[i].status == DS_NAME_ERROR.DS_NAME_NO_ERROR,
                    "IDL_DRSCrackNames: DS_LIST_SERVERS_WITH_DCS_IN_SITE: return status should be 0, got {0}",
                    reply.Value.V1.pResult[0].rItems[i].status
                );
            }

            testSite.Assert.IsTrue(
                DrsrHelper.IsStringArrayEqual(serverDns, drsrServerDns),
                "IDL_DRSCrackNames: DS_LIST_SERVERS_WITH_DCS_IN_SITE: failed to verify NCs."
            );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1304:SpecifyCultureInfo", MessageId = "System.String.ToLower")]
        void VerifyCrackNamesListGlobalCatalogServers(
            DsServer dc,
            DRS_MSG_CRACKREQ req,
            DRS_MSG_CRACKREPLY? reply)
        {
            DsServer[] servers = ldapAd.ListGcServers(dc);
            testSite.Assert.IsTrue(
               reply.Value.V1.pResult[0].cItems == servers.Length,
               "IDL_DRSCrackNames: DS_LIST_GLOBAL_CATALOG_SERVERS: wrong server count, expect:{0}, got:{1}",
               servers.Length,
               reply.Value.V1.pResult[0].cItems
           );

            string[] siteDns = new string[servers.Length];
            string[] dnsHostNames = new string[servers.Length];
            for (int i = 0; i < servers.Length; ++i)
            {
                siteDns[i] = servers[i].Site.DN.ToLower();
                dnsHostNames[i] = servers[i].DnsHostName.ToLower();
            }

            string[] drsrSiteDns = new string[reply.Value.V1.pResult[0].cItems];
            string[] drsrDnsHostNames = new string[reply.Value.V1.pResult[0].cItems];
            for (int i = 0; i < reply.Value.V1.pResult[0].cItems; ++i)
            {
                drsrSiteDns[i] = reply.Value.V1.pResult[0].rItems[i].pName.ToLower();
                drsrDnsHostNames[i] = reply.Value.V1.pResult[0].rItems[i].pDomain.ToLower();
                testSite.Assert.IsTrue(
                    reply.Value.V1.pResult[0].rItems[i].status == DS_NAME_ERROR.DS_NAME_NO_ERROR,
                    "IDL_DRSCrackNames: DS_LIST_GLOBAL_CATALOG_SERVERS: return status should be 0, got {0}",
                    reply.Value.V1.pResult[0].rItems[i].status
                );
            }

            testSite.Assert.IsTrue(
                DrsrHelper.IsStringArrayEqual(siteDns, drsrSiteDns),
                "IDL_DRSCrackNames: DS_LIST_GLOBAL_CATALOG_SERVERS: failed to verify site DNs."
            );

            testSite.Assert.IsTrue(
                DrsrHelper.IsStringArrayEqual(dnsHostNames, drsrDnsHostNames),
                "IDL_DRSCrackNames: DS_LIST_GLOBAL_CATALOG_SERVERS: failed to verify server DNS host names."
            );
 
        }

        void VerifyCrackNamesMapSchemaGuid(
            DsServer dc,
            DRS_MSG_CRACKREQ req,
            DRS_MSG_CRACKREPLY? reply)
        {
            RootDSE rootDse = LdapUtility.GetRootDSE(dc);
            Guid guid = new Guid(req.V1.rpNames[0]);
            string schemaGuidStr = LdapUtility.GetBinaryString(guid.ToByteArray());

            string name = ldapAd.GetAttributeValueInString(
                dc,
                rootDse.schemaNamingContext,
                "lDAPDisplayName",
                "(schemaIDGUID=" + schemaGuidStr + ")", 
                SearchScope.Subtree
                );

            testSite.Assert.IsTrue(
                name == reply.Value.V1.pResult[0].rItems[0].pName,
                "IDL_DRSCrackNames: DS_MAP_SCHEMA_GUID: failed to verify schema name, expect:{0}, got{1}",
                name,
                reply.Value.V1.pResult[0].rItems[0].pName
                );

            string[] objClass = LdapUtility.GetAttributeValuesString(
                dc,
                rootDse.schemaNamingContext,
                "objectClass",
                "(schemaIDGUID=" + schemaGuidStr + ")",
                SearchScope.Subtree
                );

            string objLastClass = objClass[objClass.Length - 1];
            switch (objLastClass)
            {
                case "classSchema":
                    testSite.Assert.IsTrue(
                        DS_NAME_ERROR.DS_NAME_ERROR_SCHEMA_GUID_CLASS == reply.Value.V1.pResult[0].rItems[0].status,
                        "IDL_DRSCrackNames: DS_MAP_SCHEMA_GUID: failed to verify status, expect:{0}, got{1}",
                        DS_NAME_ERROR.DS_NAME_ERROR_SCHEMA_GUID_CLASS,
                        reply.Value.V1.pResult[0].rItems[0].status
                        );
                    break;
                case "attributeSchema":
                    testSite.Assert.IsTrue(
                        DS_NAME_ERROR.DS_NAME_ERROR_SCHEMA_GUID_ATTR == reply.Value.V1.pResult[0].rItems[0].status,
                        "IDL_DRSCrackNames: DS_MAP_SCHEMA_GUID: failed to verify status, expect:{0}, got{1}",
                        DS_NAME_ERROR.DS_NAME_ERROR_SCHEMA_GUID_CLASS,
                        reply.Value.V1.pResult[0].rItems[0].status
                        );
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Verify CrackNames results.
        /// </summary>
        /// <param name="machine"></param>
        /// <param name="retVal"></param>
        /// <param name="dwInVersion"></param>
        /// <param name="req"></param>
        /// <param name="outVersion"></param>
        /// <param name="reply"></param>
        public void VerifyDrsCrackNames(
            EnvironmentConfig.Machine machine,
            uint retVal,
            uint dwInVersion,
            DRS_MSG_CRACKREQ req,
            uint? outVersion,
            DRS_MSG_CRACKREPLY? reply)
        {
            // IDL_DRSCrackNames supports only V1
            testSite.Assert.IsTrue(outVersion == 1,
                "IDL_DRSCrackNames: Checking outVersion - got: {0}, expect: {1}, outVersion should be 1.", outVersion, 1);
            testSite.Assert.IsNotNull(reply, "IDL_DRSCrackNames: Checking outMessage - outMessage should not be null.");
            testSite.Assert.IsNotNull(reply.Value.V1.pResult, "IDL_DRSCrackNames: Checking pResult - pResult in outMessage should not be null.");


            DsServer srv = (DsServer)EnvironmentConfig.MachineStore[machine];

            uint flag = req.V1.dwFlags;
            switch (flag)
            {
                case (uint)DRS_MSG_CRACKREQ_FLAGS.DS_NAME_FLAG_GC_VERIFY:
                    {
                        const uint ERROR_DS_GCVERIFY_ERROR = 0x20e1;

                        bool isGc = ldapAd.IsGc(srv);
                        if (!isGc)
                        {
                            testSite.Assert.IsTrue(
                                 retVal == ERROR_DS_GCVERIFY_ERROR,
                                 "IDL_DRSCrackNames: Cannot verify the GC server {0}",
                                 srv.NetbiosName);
                        }
                        else
                        {
                            testSite.Assert.IsTrue(
                                retVal == (uint)DS_NAME_ERROR.DS_NAME_NO_ERROR,
                                "IDL_DRSCrackNames: DC {0} is not a GC server",
                                srv.NetbiosName);
                        }
                    }
                    break;
                default:
                    break;
            }

            // formats
            uint formatOffered = req.V1.formatOffered;
            switch (formatOffered)
            {
                case (uint)DS_NAME_FORMAT.DS_INVALID_NAME:
                    // Invalid name, just break.
                    break;
                case (uint)formatOffered_Values.DS_LIST_SITES:
                    VerifyCrackNamesListAllSites(srv, req, reply);
                    break;
                case (uint)formatOffered_Values.DS_LIST_SERVERS_IN_SITE:
                    VerifyCrackNamesListServersInSite(srv, req, reply);
                    break;
                case (uint)formatOffered_Values.DS_LIST_DOMAINS_IN_SITE:
                    VerifyCrackNamesListDomainsInSite(srv, req, reply);
                    break;
                case (uint)formatOffered_Values.DS_LIST_SERVERS_FOR_DOMAIN_IN_SITE:
                    VerifyCrackNamesListServersForDomainInSite(srv, req, reply);
                    break;
                case (uint)formatOffered_Values.DS_LIST_INFO_FOR_SERVER:
                    VerifyCrackNamesListInfoForServer(srv, req, reply);
                    break;
                case (uint)formatOffered_Values.DS_LIST_ROLES:
                    VerifyCrackNamesListRoles(srv, req, reply);
                    break;
                case (uint)formatOffered_Values.DS_LIST_DOMAINS:
                    VerifyCrackNamesListDomains(srv, req, reply);
                    break;
                case (uint)formatOffered_Values.DS_LIST_NCS:
                    VerifyCrackNamesListNcs(srv, req, reply);
                    break;
                case (uint)formatOffered_Values.DS_LIST_SERVERS_WITH_DCS_IN_SITE:
                    VerifyCrackNamesListServersWithDcsInSite(srv, req, reply);
                    break;
                case (uint)formatOffered_Values.DS_LIST_GLOBAL_CATALOG_SERVERS:
                    VerifyCrackNamesListGlobalCatalogServers(srv, req, reply);
                    break;
                case (uint)formatOffered_Values.DS_MAP_SCHEMA_GUID:
                    VerifyCrackNamesMapSchemaGuid(srv, req, reply);
                    break;
                case (uint)DS_NAME_FORMAT.DS_UNKNOWN_NAME:
                case (uint)DS_NAME_FORMAT.DS_FQDN_1779_NAME:
                case (uint)DS_NAME_FORMAT.DS_NT4_ACCOUNT_NAME:
                case (uint)DS_NAME_FORMAT.DS_DISPLAY_NAME:
                case (uint)DS_NAME_FORMAT.DS_UNIQUE_ID_NAME:
                case (uint)DS_NAME_FORMAT.DS_CANONICAL_NAME:
                case (uint)DS_NAME_FORMAT.DS_USER_PRINCIPAL_NAME:
                case (uint)DS_NAME_FORMAT.DS_CANONICAL_NAME_EX:
                case (uint)DS_NAME_FORMAT.DS_SERVICE_PRINCIPAL_NAME:
                case (uint)DS_NAME_FORMAT.DS_SID_OR_SID_HISTORY_NAME:
                case (uint)DS_NAME_FORMAT.DS_DNS_DOMAIN_NAME:
                case (uint)formatOffered_Values.DS_NT4_ACCOUNT_NAME_SANS_DOMAIN:
                case (uint)formatOffered_Values.DS_NT4_ACCOUNT_NAME_SANS_DOMAIN_EX:
                case (uint)formatOffered_Values.DS_ALT_SECURITY_IDENTITIES_NAME:
                case (uint)formatOffered_Values.DS_STRING_SID_NAME:
                case (uint)formatOffered_Values.DS_USER_PRINCIPAL_NAME_AND_ALTSECID:
                    {
                        for (int i = 0; i < req.V1.cNames; ++i)
                        {
                           DS_NAME_RESULT_ITEMW r = ldapAd.LookupNames(
                                srv,
                                req.V1.dwFlags,
                                req.V1.formatOffered,
                                req.V1.formatDesired,
                                req.V1.rpNames[i]
                                );

                           // status code
                           testSite.Assert.IsTrue(
                               r.status == reply.Value.V1.pResult[0].rItems[i].status,
                               "IDL_DRSCrackNames: Verify status failed, expect: {0}, got: {1}",
                               r.status,
                               reply.Value.V1.pResult[0].rItems[i].status
                               );

                            // name
                            if (r.pName == null)
                            {
                                testSite.Assert.IsNull(reply.Value.V1.pResult[0].rItems[i].pName,
                                    "IDL_DRSCrackNames: Verify pName failed, expect: null, got: {0}.",
                                    reply.Value.V1.pResult[0].rItems[i].pName);
                            }
                            else
                            {
                                testSite.Assert.IsTrue(
                                    r.pName == reply.Value.V1.pResult[0].rItems[i].pName,
                                    "IDL_DRSCrackNames: Verify pName failed, expect: {0}, got: {1}.",
                                    r.pName,
                                    reply.Value.V1.pResult[0].rItems[i].pName
                                );
                            }

                            // domain
                            if (r.pDomain == null)
                            {
                                testSite.Assert.IsNull(reply.Value.V1.pResult[0].rItems[i].pDomain,
                                    "IDL_DRSCrackNames: Verify pDomain failed, expect: null, got: {0}.",
                                    reply.Value.V1.pResult[0].rItems[i].pDomain);
                            }
                            else
                            {
                                testSite.Assert.IsTrue(
                                    r.pDomain == reply.Value.V1.pResult[0].rItems[i].pDomain,
                                    "IDL_DRSCrackNames: Verify pDomain failed, expect: {0}, got: {1}.",
                                    r.pDomain,
                                    reply.Value.V1.pResult[0].rItems[i].pDomain
                                    );
                            }
                        }
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion
    }
}
