// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Net;
using System.Text;

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    public partial class LdapAdapter : ManagedAdapterBase, ILdapAdapter
    {
        DSNAME GetDSNameFromSid(DsServer dc, NT4SID sid)
        {
            RootDSE rootDse = LdapUtility.GetRootDSE(dc);
            string baseDn = rootDse.defaultNamingContext;

            StringBuilder sidStr = new StringBuilder();
            for (int i = 0; i < sid.Data.Length; ++i)
                sidStr.AppendFormat(@"\{0:x2}", sid.Data[i]);

            string filter = "(objectSid=" + sidStr.ToString() + ")";
            string dn = GetAttributeValueInString(dc, baseDn, "distinguishedName", filter, System.DirectoryServices.Protocols.SearchScope.Subtree);

            return LdapUtility.CreateDSNameForObject(dc, dn);
        }

        DirectoryAttribute GetAttrVals(
            DsServer dc,
            DSNAME o,
            string attrName,
            bool includeDeletedLinks = false)
        {
            SearchResultEntryCollection results = null;
            ResultCode re = Search(
                dc,
                LdapUtility.ConvertUshortArrayToString(o.StringName),
                "(objectClass=*)",
                System.DirectoryServices.Protocols.SearchScope.Base,
                new string[] { attrName },
                out results);

            if (re == ResultCode.Success)
            {
                return results[0].Attributes[attrName];
            }

            return null;
        }

        DS_NAME_RESULT_ITEMW LookupUnknownName(DsServer dc, uint flags, uint formatDesired, string name)
        {
            DS_NAME_RESULT_ITEMW result = new DS_NAME_RESULT_ITEMW();
            uint[] formatOrder = new uint[] {
                (uint)DS_NAME_FORMAT.DS_FQDN_1779_NAME,
                (uint)DS_NAME_FORMAT.DS_USER_PRINCIPAL_NAME,
                (uint)DS_NAME_FORMAT.DS_NT4_ACCOUNT_NAME,
                (uint)DS_NAME_FORMAT.DS_CANONICAL_NAME,
                (uint)DS_NAME_FORMAT.DS_UNIQUE_ID_NAME,
                (uint)DS_NAME_FORMAT.DS_DISPLAY_NAME,
                (uint)DS_NAME_FORMAT.DS_SERVICE_PRINCIPAL_NAME,
                (uint)DS_NAME_FORMAT.DS_SID_OR_SID_HISTORY_NAME,
                (uint)DS_NAME_FORMAT.DS_CANONICAL_NAME_EX
            };

            foreach (uint format in formatOrder)
            {
                result = LookupNames(dc, flags, format, formatDesired, name);
                if (result.status != DS_NAME_ERROR.DS_NAME_ERROR_NOT_FOUND)
                    return result;
            }

            return result;
        }

        DSNAME[] LookupAttr(DsServer dc, uint flags, string attrName, string attrValue)
        {
            if (attrName == null || attrValue == null)
                return null;

            RootDSE rootDse = LdapUtility.GetRootDSE(dc);

            SearchResultEntryCollection results = null;
            ResultCode re = Search(
                dc,
                rootDse.defaultNamingContext,
                "(" + attrName + "=" + attrValue + ")",
                System.DirectoryServices.Protocols.SearchScope.Subtree,
                new string[] { "distinguishedName" },
                out results);

            if (re == ResultCode.NoSuchObject)
            {
                // not found in default NC, try in config NC
                re = Search(
                    dc,
                    rootDse.configurationNamingContext,
                    "(" + attrName + "=" + attrValue + ")",
                    System.DirectoryServices.Protocols.SearchScope.Subtree,
                    new string[] { "distinguishedName" },
                    out results);
            }

            if (re == ResultCode.NoSuchObject)
            {
                // not found in config NC, try in schema NC
                re = Search(
                    dc,
                    rootDse.schemaNamingContext,
                    "(" + attrName + "=" + attrValue + ")",
                    System.DirectoryServices.Protocols.SearchScope.Subtree,
                    new string[] { "distinguishedName" },
                    out results);
            }
 
            if (re == ResultCode.Success && results.Count > 0)
            {
                List<DSNAME> names = new List<DSNAME>();
                foreach (SearchResultEntry e in results)
                {
                    names.Add(LdapUtility.CreateDSNameForObject(dc, e.DistinguishedName));
                }
                return names.ToArray();
            }
            return null;
        }

        string DomainDNSNameFromDomain(DsServer dc, DSNAME domainNc)
        {
            // Looks like this method also returns NetBIOS name
            // return DomainNetBIOSNameFromDomain(dc, domainNc);
            return DrsrHelper.GetFQDNFromDN(LdapUtility.ConvertUshortArrayToString(domainNc.StringName));
        }

        DSNAME? DomainFromDomainDNSName(DsServer dc, string domainName)
        {
            string dn = DrsrHelper.GetDNFromFQDN(domainName);
            if (dn != null)
                return LdapUtility.CreateDSNameForObject(dc, dn);
            else
                return null;
        }

        string DomainNameFromCanonicalName(string canonicalName)
        {
            int firstSlash = canonicalName.IndexOf('/');
            if (firstSlash < 0)
                return null;

            return canonicalName.Substring(0, firstSlash);
        }


        string DomainNameFromSid(DsServer dc, NT4SID sid)
        {
            SecurityIdentifier secId = new SecurityIdentifier(sid.Data, 0);

            RootDSE rootDse = LdapUtility.GetRootDSE(dc);
            return DrsrHelper.GetFQDNFromDN(rootDse.defaultNamingContext);
        }

        string DomainNameFromUPN(string upn)
        {
            int atPos = upn.IndexOf('@');
            if (atPos < 0)
                return null;

            return upn.Substring(atPos + 1);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1304:SpecifyCultureInfo", MessageId = "System.String.ToUpper")]
        string DomainNetBIOSNameFromDomain(DsServer dc, DSNAME domainNc)
        {
            return (GetAttributeValueInString(
                dc,
                LdapUtility.ConvertUshortArrayToString(domainNc.StringName),
                "name")).ToUpper();
        }

        NT4SID DomainSidFromSid(NT4SID sid)
        {
            SecurityIdentifier secId = new SecurityIdentifier(sid.Data, 0);
            SecurityIdentifier domainSid = secId.AccountDomainSid;

            NT4SID nsid = new NT4SID();
            nsid.Data = new byte[28];
            domainSid.GetBinaryForm(nsid.Data, 0);

            return nsid;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1307:SpecifyStringComparison", MessageId = "System.String.StartsWith(System.String)")]
        string RetrieveDCSuffixFromDn(string dn)
        {
            if (!dn.Contains(","))
                return null;

            // Search from the bottom up
            string[] parts = dn.Split(',');
            string re = "";
            for (int i = parts.Length - 1; i >= 0; --i)
            {
                string p = parts[i].Trim();
                if (p.StartsWith("DC"))
                    re = p + "," + re;
            }

            return re.Substring(0, re.Length - 1);
        }

        DSNAME[] LookupUPNAndAltSecID(DsServer dc, uint flags, bool includingAltSecID, string name)
        {
            DSNAME[] rt1, rt2;

            List<DSNAME> rt = new List<DSNAME>();
            if (includingAltSecID)
            {
                rt1 = LookupAttr(dc, flags, "userPrincipalName", name);
                rt2 = LookupAttr(dc, flags, "altSecurityIdentities", name);
                if (rt1 != null)
                    rt.AddRange(rt1);
                if (rt2 != null)
                    rt.AddRange(rt2);
            }
            else
            {
                rt1 = LookupAttr(dc, flags, "userPrincipalName", name);
                if (rt1 != null)
                    rt.AddRange(rt1);
            }

            if (rt.Count > 0)
                return rt.ToArray();

            name = UserNameFromUPN(name);
            if (name != null)
                return LookupAttr(dc, flags, "sAMAccountName", name);

            return null;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1307:SpecifyStringComparison", MessageId = "System.String.IndexOf(System.String)")]
        string UserNameFromUPN(string upn)
        {
            int pos = upn.IndexOf("@");
            if (pos < 0)
                return null;

            return upn.Substring(0, pos);
        }

        DSNAME? LookupCanonicalName(DsServer dc, string name)
        {
            if (name == null)
                return null;

            string label = null;
            DSNAME? curObj = null;
            ParseCanonicalName(name, out label, out name);
            curObj = DomainFromDomainDNSName(dc, label);
            while (name != null && curObj != null)
            {
                ParseCanonicalName(name, out label, out name);
                SearchResultEntryCollection results = null;
                ResultCode re = Search(
                    dc,
                    LdapUtility.ConvertUshortArrayToString(curObj.Value.StringName),
                    "(objectClass=*)",
                    System.DirectoryServices.Protocols.SearchScope.OneLevel,
                    new string[] { "name" },
                    out results
                    );

                foreach (SearchResultEntry e in results)
                {
                    if (label == (string)e.Attributes["name"][0])
                    {
                        curObj = LdapUtility.CreateDSNameForObject(dc, e.DistinguishedName);
                        break;
                    }
                }

                if (curObj == null)
                    return null;
            }

            return curObj;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1307:SpecifyStringComparison", MessageId = "System.String.IndexOf(System.String)")]
        void ParseCanonicalName(string name, out string firstPart, out string remainder)
        {
            int pos = name.IndexOf("/");
            if (pos < 0)
            {
                firstPart = name;
                remainder = null;
                return;
            }

            firstPart = name.Substring(0, pos);
            remainder = name.Substring(pos + 1);
        }

        DSNAME? DescendantObject(DsServer dc, string ancestor, string rdns)
        {
            string dn = rdns + ancestor;
            DSNAME ds = LdapUtility.CreateDSNameForObject(dc, dn);
            if (ds.StringName == null)
                return null;

            return ds;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1307:SpecifyStringComparison", MessageId = "System.String.IndexOf(System.String)")]
        string MapSPN(string spn, string[] spnMappings)
        {
            string serviceClass, domainName;
            ParseCanonicalName(spn, out serviceClass, out domainName);
            foreach (string map in spnMappings)
            {
                int eq = map.IndexOf("=");
                string alias = map.Substring(0, eq);
                string[] serviceClasses = map.Substring(eq + 1).Split(',');

                foreach (string sc in serviceClasses)
                {
                    if (serviceClass == sc.Trim())
                        return alias + "/" + domainName;
                }
            }

            return null;
        }

        string GetServiceClassFromSPN(string spn)
        {
            string serviceClass, remainder;
            ParseCanonicalName(spn, out serviceClass, out remainder);
            return serviceClass;
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1307:SpecifyStringComparison", MessageId = "System.String.StartsWith(System.String)")]
        string DomainNameFromDN(string dn)
        {
            // Do this by collecting all RDNs starting with "DC=", and combine the remainder with "."
            string[] rdns = dn.Split(',');
            string domainName = "";
            for (int i = 0; i < rdns.Length - 1; ++i)
            {
                string rdn = rdns[i].Trim();
                if (rdn.StartsWith("DC="))
                    domainName += (rdn.Substring(3) + ".");
            }

            return domainName + rdns[rdns.Length - 1].Trim().Substring(3);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1307:SpecifyStringComparison", MessageId = "System.String.IndexOf(System.String)")]
        string GetInstanceNameFromSPN(string spn)
        {
            string first, remainder;
            ParseCanonicalName(spn, out first, out remainder);
            int pos = remainder.IndexOf("/");
            if (pos < 0)
                return remainder;

            return remainder.Substring(0, pos);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1304:SpecifyCultureInfo", MessageId = "System.String.ToUpper")]
        DSNAME[] LookupSPN(DsServer dc, uint flags, string name)
        {
            DSNAME[] rt = null;
            Guid? dcGuid = null;
            string[] spnMappings = null;
            string mappedSpn = null;

            RootDSE rootDse = LdapUtility.GetRootDSE(dc);

            rt = LookupAttr(dc, flags, "servicePrincipalName", name);
            if (rt != null)
                return rt;

            string dsService = "CN=Directory Service,CN=Windows NT,CN=Services," + rootDse.configurationNamingContext;
            spnMappings = LdapUtility.GetAttributeValuesString(dc, dsService + rootDse.configurationNamingContext, "sPNMappings");
            if (spnMappings != null)
            {
                mappedSpn = MapSPN(name, spnMappings);
                if (mappedSpn != null)
                {
                    rt = LookupAttr(dc, flags, "servicePrincipalName", mappedSpn);
                    if (rt != null)
                        return rt;
                }
            }

            if (GetServiceClassFromSPN(name) == DrsrUtility.DRSUAPI_RPC_INTERFACE_UUID.ToString().ToUpper()
                && GetServiceNameFromSPN(name) == DomainNameFromDN(rootDse.defaultNamingContext))
            {
                dcGuid = new Guid(GetInstanceNameFromSPN(name));
                if (dcGuid != null)
                {
                    string objDn = LdapUtility.GetObjectDnByGuid(dc, rootDse.configurationNamingContext, dcGuid.Value);
                    if (objDn != null)
                    {
                        objDn = GetParentObjectDn(objDn);
                        if (objDn != null)
                        {
                            string srvRef = GetAttributeValueInString(dc, objDn, "serverReference");
                            rt = new DSNAME[] { GetDsName(dc, srvRef).Value };
                        }
                    }
                }
            }

            return rt;
        }

        string GetServiceNameFromSPN(string spn)
        {
            string[] parts = spn.Split('/');
            if (parts.Length < 3)
                return null;

            return parts[2];
        }

        DSNAME[] LookupSID(DsServer dc, uint flags, NT4SID sid)
        {
            List<DSNAME> rt = new List<DSNAME>();
            SecurityIdentifier secId = new SecurityIdentifier(sid.Data, 0);

            DSNAME[] rt1 = LookupAttr(dc, flags, "objectSid", secId.ToString());
            DSNAME[] rt2 = LookupAttr(dc, flags, "sIDHistory", secId.ToString());

            if (rt1 != null)
                rt.AddRange(rt1);
            if (rt2 != null)
                rt.AddRange(rt2);

            if (rt.Count > 0)
                return rt.ToArray();
            else
                return null;
        }

        NT4SID SidFromStringSid(string name)
        {
            const int SidSize = 28;
            NT4SID sid = new NT4SID();

            sid.Data = new byte[SidSize];
            SecurityIdentifier secId = new SecurityIdentifier(name);
            secId.GetBinaryForm(sid.Data, 0);

            return sid;
        }

        string CanonicalNameFromCanonicalNameEx(string name)
        {
            int newlinePos = name.IndexOf('\n');
            if (newlinePos < 0)
                return null;
            return name.Substring(0, newlinePos) + "/" + name.Substring(newlinePos + 1);
        }


        string GetCanonicalName(DsServer dc, DSNAME obj, bool extended)
        {
            string result;
            string dn = LdapUtility.ConvertUshortArrayToString(obj.StringName);
            //if (GetObjectNC(dc, obj).Guid == obj.Guid)
            if (RetrieveDCSuffixFromDn(dn) == dn)
                return DomainDNSNameFromDomain(dc, obj);


            DSNAME parentObj = GetDsName(dc, GetParentObjectDn(dn)).Value;
            result = GetCanonicalName(dc, parentObj, false);
            if (extended == true)
                result = result + "\n";
            else
                result = result + "/";

            string name = GetAttributeValueInString(dc, dn, "name");
            result = result + name;
            return result;
        }

        string[] ConstructOutput(DsServer dc, DSNAME obj, uint formatDesired)
        {
            string dn = LdapUtility.ConvertUshortArrayToString(obj.StringName);
            if (dn == null)
                return null;

            string value = null;
            switch (formatDesired)
            {
                case (uint)DS_NAME_FORMAT.DS_FQDN_1779_NAME:
                    value = GetAttributeValueInString(dc, dn, "distinguishedName");
                    break;
                case (uint)DS_NAME_FORMAT.DS_NT4_ACCOUNT_NAME:
                    value = DomainNetBIOSNameFromDomain(dc, GetObjectNC(dc, obj)) + "\\" +
                        GetAttributeValueInString(dc, dn, "sAMAccountName");
                    break;
                case (uint)DS_NAME_FORMAT.DS_USER_PRINCIPAL_NAME:
                    value = GetAttributeValueInString(dc, dn, "userPrincipalName");
                    break;
                case (uint)DS_NAME_FORMAT.DS_CANONICAL_NAME:
                    value = GetCanonicalName(dc, obj, false);
                    break;
                case (uint)DS_NAME_FORMAT.DS_UNIQUE_ID_NAME:
                    // curly-braced form.
                    value = "{"
                        + (new Guid(GetAttributeValueInBytes(dc, dn, "objectGuid"))).ToString()
                        + "}";
                    break;
                case (uint)DS_NAME_FORMAT.DS_DISPLAY_NAME:
                    value = GetAttributeValueInString(dc, dn, "displayName");
                    break;
                case (uint)DS_NAME_FORMAT.DS_SERVICE_PRINCIPAL_NAME:
                    return LdapUtility.GetAttributeValuesString(dc, dn, "servicePrincipalName");
                case (uint)DS_NAME_FORMAT.DS_CANONICAL_NAME_EX:
                    value = GetCanonicalName(dc, obj, true);
                    break;
                case (uint)formatDesired_Values.DS_STRING_SID_NAME:
                    value = LdapUtility.GetObjectStringSid(dc, dn);
                    break;
                case (uint)formatDesired_Values.DS_USER_PRINCIPAL_NAME_FOR_LOGON:
                    {
                        string upn = GetAttributeValueInString(dc, dn, "userPrincipalName");
                        if (upn != null)
                            value = upn;
                        else
                            value = GetAttributeValueInString(dc, dn, "sAMAccountName") + "@"
                            + DomainNetBIOSNameFromDomain(dc, GetObjectNC(dc, obj));
                    }
                    break;
                default:
                    break;
            }

            if (value != null)
                return new string[] { value };
            return null;
        }

        DS_NAME_RESULT_ITEMW LookupFPO(bool fCanonicalEx, DSNAME obj, DS_NAME_RESULT_ITEMW result)
        {
            throw new NotImplementedException();
        }

        public string TrimDn(string dn)
        {
            string[] parts = dn.Trim().Split(',');
            string re = "";
            for (int i = 0; i < parts.Length - 1; ++i)
                re += ((parts[i].Trim()) + ",");

            re += (parts[parts.Length - 1].Trim());
            return re;
        }

        public DSNAME GetObjectNC(DsServer dc, DSNAME obj)
        {
            RootDSE rootDse = LdapUtility.GetRootDSE(dc);

            string dn = LdapUtility.ConvertUshortArrayToString(obj.StringName);

            string ncDn = "";
            // there might be spaces between each RDN of the dn.
            dn = TrimDn(dn);

            // default first
            if (rootDse.defaultNamingContext != null)
            {
                // AD LDS doesn't have default NC
                if (dn.Contains(rootDse.defaultNamingContext))
                    ncDn = rootDse.defaultNamingContext;
            }

            if (dn.Contains(rootDse.configurationNamingContext))
                ncDn = rootDse.configurationNamingContext;

            if (dn.Contains(rootDse.schemaNamingContext))
                ncDn = rootDse.schemaNamingContext;

            return LdapUtility.CreateDSNameForObject(dc, ncDn);
        }

        /// <summary>
        /// Check if the server is a GC.
        /// </summary>
        /// <param name="dc">The DC to be checked.</param>
        /// <returns>True if the server is a GC, false otherwise.</returns>
        public bool IsGc(DsServer dc)
        {
            RootDSE rootDse = LdapUtility.GetRootDSE(dc);
            return rootDse.isGcReady;
        }

        /// <summary>
        /// Get all site objects in the forest where the DC is located.
        /// </summary>
        /// <param name="dc">The DC in the forest.</param>
        /// <returns>All site objects in the forest.</returns>
        public DsSite[] ListSites(DsServer dc)
        {
            RootDSE rootDse = LdapUtility.GetRootDSE(dc);
            // Forest, so start with the root of config nc
            string siteRoot = "CN=Sites," + rootDse.configurationNamingContext;

            SearchResultEntryCollection results = null;
            ResultCode ret = Search(
                dc,
                siteRoot,
                "(objectClass=site)",
                System.DirectoryServices.Protocols.SearchScope.OneLevel,
                null,
                out results
                );
 
            List<DsSite> sites = new List<DsSite>();
            foreach (SearchResultEntry site in results)
            {
                string dn = site.DistinguishedName;
                sites.Add(GetSite(dc, dn));
            }

            return sites.ToArray();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1307:SpecifyStringComparison", MessageId = "System.String.StartsWith(System.String)")]
        public DsSite GetSite(DsServer dc, string dn)
        {
            DsSite site = new DsSite();
            site.DN = dn;

            // servers
            // find all "server" objects under the site dn
            SearchResultEntryCollection results = null;
            ResultCode ret = Search(
                dc,
                dn,
                "(objectClass=server)",
                System.DirectoryServices.Protocols.SearchScope.Subtree,
                null,
                out results
                );

            List<DsServer> servers = new List<DsServer>();
            foreach (SearchResultEntry e in results)
            {
                DsServer srv = new DsServer();
                srv.NtdsDsaObjectName = e.DistinguishedName;
                servers.Add(srv);
            }
            site.Servers = servers.ToArray();

            // Look into every NTDS DSA object to find the domain it is in.
            List<string> domainNcs = new List<string>();
            foreach (DsServer s in site.Servers)
            {
                string[] ncs = LdapUtility.GetAttributeValuesString(
                    dc,
                    s.NtdsDsaObjectName,
                    "hasMasterNCs",
                    "(objectClass=nTDSDSA)",
                    System.DirectoryServices.Protocols.SearchScope.Subtree);
                if (ncs == null)
                    continue;
                foreach (string nc in ncs)
                {
                    bool newNc = true;
                    foreach (string oldNc in domainNcs)
                    {
                        if (oldNc == nc)
                        {
                            newNc = false;
                            break;
                        }
                    }
                    if (newNc)
                        domainNcs.Add(nc);
                }
            }
            /*
            string[] domainNcs = LdapUtility.GetAttributeValuesString(
                dc,
                site.DN,
                "hasMasterNCs", //"msDS-HasDomainNCs", 
                "(objectClass=nTDSDSA)",
                System.DirectoryServices.Protocols.SearchScope.Subtree);
            */

            bool isAdlds = !EnvironmentConfig.TestDS;

            if (domainNcs == null && isAdlds)
            {
                return site;
            }

            // Eliminate ConfigNC and SchemaNC
            List<string> filteredDomainNcs = new List<string>();
            foreach (string d in domainNcs)
            {
                if (d.StartsWith("CN=Configuration") || d.StartsWith("CN=Schema"))
                    continue;
                filteredDomainNcs.Add(d);
            }

            List<DsDomain> domains = new List<DsDomain>();
            foreach (string tdn in filteredDomainNcs)
            {
                bool n = true;
                foreach (DsDomain d in domains)
                {
                    if (d.Name == tdn)
                    {
                        n = false;
                        break;
                    }
                }

                if (n)
                {
                    DsDomain nd;
                    if (isAdlds)
                    {
                        nd = new AdldsDomain();
                    }
                    else
                    {
                        nd = new AddsDomain();
                    }
                    nd.Name = tdn;
                    domains.Add(nd);
                }
            }
            site.Domains = domains.ToArray();
            return site;
        }

        /// <summary>
        /// Get the DN, DNS host name and serverReference objects of the server.
        /// </summary>
        /// <param name="dc">The DC to talk to.</param>
        /// <param name="dn">The DN of the server.</param>
        /// <param name="dsaObjDn">When return, contains the NTDS DSA object DN.</param>
        /// <param name="dnsHostName">When return, contains the DNS host name of the server.</param>
        /// <param name="serverReference">When return, contains the serverReference object of the server.</param>
        public void ListInfoForServer(
            DsServer dc,
            string dn,
            out string dsaObjDn,
            out string dnsHostName,
            out string serverReference)
        {
            dsaObjDn = GetAttributeValueInString(
                dc,
                dn,
                "distinguishedName",
                "(objectClass=nTDSDSA)",
                System.DirectoryServices.Protocols.SearchScope.Subtree
                );
            dnsHostName = GetAttributeValueInString(dc, dn, "dNSHostName");
            serverReference = GetAttributeValueInString(dc, dn, "serverReference");
        }

        /// <summary>
        /// Get all domains in the forest.
        /// </summary>
        /// <param name="dc">The DC in the forest.</param>
        /// <returns>All domain objects in the forest.</returns>
        public DsDomain[] ListDomains(DsServer dc)
        {
            RootDSE rootDse = LdapUtility.GetRootDSE(dc);
            string partitionDn = "CN=Partitions," + rootDse.configurationNamingContext;
            SearchResultEntryCollection results = null;

            ResultCode re = LdapUtility.Search(
                dc,
                partitionDn,
                "(&(objectClass=crossRef)(systemFlags:1.2.840.113556.1.4.804:=2))",
                System.DirectoryServices.Protocols.SearchScope.Subtree,
                new string[] { "nCName" },
                out results);

            if (re != ResultCode.Success)
                return null;

            List<DsDomain> domains = new List<DsDomain>();

            foreach (SearchResultEntry e in results)
            {
                DirectoryAttribute attr = e.Attributes["nCName"];
                string dn = (string)attr[0];
                DsDomain domain = new AddsDomain();
                domain.Name = dn;
                domains.Add(domain);
            }

            return domains.ToArray();
        }

        /// <summary>
        /// Get all NCs in the forest.
        /// </summary>
        /// <param name="dc">The DC in the forest.</param>
        /// <returns>All NC DNs in the forest.</returns>
        public string[] ListNCs(DsServer dc)
        {
            RootDSE rootDse = LdapUtility.GetRootDSE(dc);
            string partitionDn = "CN=Partitions," + rootDse.configurationNamingContext;
            SearchResultEntryCollection results = null;

            ResultCode re = LdapUtility.Search(
                dc,
                partitionDn,
                "(objectClass=crossRef)",
                System.DirectoryServices.Protocols.SearchScope.Subtree,
                new string[] { "nCName" },
                out results);

            if (re != ResultCode.Success)
                return null;

            List<string> ncs = new List<string>();

            foreach (SearchResultEntry e in results)
            {
                DirectoryAttribute attr = e.Attributes["nCName"];
                string dn = (string)attr[0];
                ncs.Add(dn);
            }

            return ncs.ToArray();
        }

        /// <summary>
        /// Get all DCs in a given site.
        /// </summary>
        /// <param name="dc">The DC to talk to.</param>
        /// <param name="siteDn">The site to query servers from.</param>
        /// <returns>All servers in the site.</returns>
        public DsServer[] ListServersWithDcsInSite(DsServer dc, string siteDn)
        {
            SearchResultEntryCollection results = null;

            ResultCode re = Search(
                dc,
                siteDn,
                "(&(objectClass=nTDSDSA)(msDS-hasMasterNCs=*))",
                System.DirectoryServices.Protocols.SearchScope.Subtree,
                new string[] { "distinguishedName" },
                out results);

            if (re != ResultCode.Success)
                return null;

            List<DsServer> servers = new List<DsServer>();
            foreach (SearchResultEntry e in results)
            {
                DirectoryAttribute attr = e.Attributes["distinguishedName"];
                string dn = (string)attr[0];
                DsServer srv = new DsServer();
                // We need the server DN, not its NTDS DSA object DN.
                srv.ServerObjectName = GetParentObjectDn(dn);
                srv.NtdsDsaObjectName = dn;
                servers.Add(srv);
            }

            return servers.ToArray();
        }

        /// <summary>
        /// Get all GC servers in the forest.
        /// </summary>
        /// <param name="dc">The DC in the forest.</param>
        /// <returns>All GC servers in the forest.</returns>
        public DsServer[] ListGcServers(DsServer dc)
        {
            RootDSE rootDse = LdapUtility.GetRootDSE(dc);
            string configNcDn = rootDse.configurationNamingContext;

            SearchResultEntryCollection results = null;

            ResultCode re = Search(
                dc,
                configNcDn,
                "(&(objectClass=nTDSDSA)(invocationId=*)(options:1.2.840.113556.1.4.804:=1))",
                System.DirectoryServices.Protocols.SearchScope.Subtree,
                null,
                out results);

            if (re != ResultCode.Success)
                return null;

            List<DsServer> servers = new List<DsServer>();
            foreach (SearchResultEntry e in results)
            {
                string dn = e.DistinguishedName;
                string serverObjDn = GetParentObjectDn(dn);
                string siteObjDn = GetParentObjectDn(GetParentObjectDn(serverObjDn));

                DsServer srv = new DsServer();
                // find the dnsHostName of the server object
                srv.DnsHostName = GetAttributeValueInString(dc, serverObjDn, "dNSHostName");
                srv.Site = new DsSite();
                // Here, the site DN is actually the RDN of the site.
                srv.Site.DN = siteObjDn.Split(',')[0].Remove(0, 3).Trim();

                servers.Add(srv);
            }
            return servers.ToArray();
        }

        /// <summary>
        /// Get all DCs of a specific domain in a specific site.
        /// </summary>
        /// <param name="dc">The DC to talk to.</param>
        /// <param name="domainDn">The specific domain.</param>
        /// <param name="siteDn">The specific site.</param>
        /// <returns>DCs in the specific domain in the specific site.</returns>
        public DsServer[] ListServersForDomainInSite(DsServer dc, string domainDn, string siteDn)
        {
            SearchResultEntryCollection results = null;

            ResultCode re = Search(
                dc,
                siteDn,
                "(&(objectClass=nTDSDSA)(msDS-hasMasterNCs=" + domainDn + "))",
                System.DirectoryServices.Protocols.SearchScope.Subtree,
                new string[] { "distinguishedName" },
                out results);

            if (re != ResultCode.Success)
                return null;

            List<DsServer> servers = new List<DsServer>();
            foreach (SearchResultEntry e in results)
            {
                DirectoryAttribute attr = e.Attributes["distinguishedName"];
                string dn = (string)attr[0];
                DsServer srv = new DsServer();
                // We need the server DN, not its NTDS DSA object DN.
                srv.ServerObjectName = GetParentObjectDn(dn);
                srv.NtdsDsaObjectName = dn;
                servers.Add(srv);
            }

            return servers.ToArray();
        }

        /// <summary>
        /// Lookup names in given format, return the name in another given format
        /// </summary>
        /// <param name="dc">The DC to talk to.</param>
        /// <param name="flags">DS_NAME_FLAG flags.</param>
        /// <param name="formatOffered">The format of the name in `name'.</param>
        /// <param name="formatDesired">the format of the name in the return value.</param>
        /// <param name="name">Input name to translate.</param>
        /// <returns>A DS_NAME_RESULT_ITEMW structure containing the translated name.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.Convert.ToUInt32(System.String)")]
        public DS_NAME_RESULT_ITEMW LookupNames(
            DsServer dc,
            uint flags,
            uint formatOffered,
            uint formatDesired,
            string name)
        {
            DS_NAME_RESULT_ITEMW result = new DS_NAME_RESULT_ITEMW();
            string referredDomain = "";
            // Unknown name
            if (formatOffered == (uint)DS_NAME_FORMAT.DS_UNKNOWN_NAME)
                return LookupUnknownName(dc, flags, formatDesired, name);

            DSNAME[] rt = null;
            string domainName = null;
            switch (formatOffered)
            {
                case (uint)DS_NAME_FORMAT.DS_UNKNOWN_NAME:
                    return LookupUnknownName(dc, flags, formatDesired, name);
                case (uint)DS_NAME_FORMAT.DS_FQDN_1779_NAME:
                    {
                        rt = LookupAttr(dc, flags, "distinguishedName", name);
                        if (EnvironmentConfig.TestDS)
                        {
                            DSNAME? dcDsname = GetDsName(dc, RetrieveDCSuffixFromDn(name));
                            if (dcDsname != null)
                                domainName = DomainDNSNameFromDomain(dc, dcDsname.Value);
                        }
                    }
                    break;
                case (uint)DS_NAME_FORMAT.DS_NT4_ACCOUNT_NAME:
                    {
                        rt = LookupAttr(dc, flags, "sAMAccountName", LdapUtility.UserNameFromNT4AccountName(name));
                        if (EnvironmentConfig.TestDS)
                        {
                            domainName = LdapUtility.DomainNameFromNT4AccountName(name);
                        }
                    }
                    break;
                case (uint)DS_NAME_FORMAT.DS_USER_PRINCIPAL_NAME:
                    {
                        rt = LookupUPNAndAltSecID(dc, flags, false, name);
                        if (EnvironmentConfig.TestDS)
                        {
                            domainName = DomainNameFromUPN(name);
                        }
                    }
                    break;
                case (uint)DS_NAME_FORMAT.DS_CANONICAL_NAME:
                    {
                        DSNAME? v = LookupCanonicalName(dc, name);
                        if (v != null)
                            rt = new DSNAME[] { v.Value };
                        else
                            rt = null;
                        if (EnvironmentConfig.TestDS)
                        {
                            domainName = DomainNameFromCanonicalName(name);
                        }
                    }
                    break;
                case (uint)DS_NAME_FORMAT.DS_UNIQUE_ID_NAME:
                    {
                        RootDSE rootDse = LdapUtility.GetRootDSE(dc);

                        // The GUID is in the curly braced form, so we need 
                        // to remove the braces first.
                        if (!name.Contains("{") && name.Contains("}"))
                        {
                            rt = null;
                            break;
                        }
                        string guidStr = name.Substring(1, name.Length - 2);
                        Guid guid = new Guid(guidStr);
                        string dn = null;

                        dn = LdapUtility.GetObjectDnByGuid(dc, rootDse.rootDomainNamingContext, guid);
                        if (dn == null)
                            dn = LdapUtility.GetObjectDnByGuid(dc, rootDse.configurationNamingContext, guid);
                        if (dn == null)
                            dn = LdapUtility.GetObjectDnByGuid(dc, rootDse.schemaNamingContext, guid);

                        if (dn == null)
                            break;

                        rt = new DSNAME[] { LdapUtility.CreateDSNameForObject(dc, dn) };
                    }
                    break;
                case (uint)DS_NAME_FORMAT.DS_DISPLAY_NAME:
                    {
                        rt = LookupAttr(dc, flags, "displayName", name);
                    }
                    break;
                case (uint)DS_NAME_FORMAT.DS_SERVICE_PRINCIPAL_NAME:
                    {
                        rt = LookupSPN(dc, flags, name);
                        if (EnvironmentConfig.TestDS)
                        {
                            domainName = GetServiceNameFromSPN(name);
                        }
                    }
                    break;
                case (uint)DS_NAME_FORMAT.DS_SID_OR_SID_HISTORY_NAME:
                case (uint)formatOffered_Values.DS_STRING_SID_NAME:
                    {
                        rt = LookupSID(dc, flags, SidFromStringSid(name));
                        if (EnvironmentConfig.TestDS)
                        {
                            domainName = DomainNameFromSid(dc, DomainSidFromSid(SidFromStringSid(name)));
                        }
                    }
                    break;
                case (uint)DS_NAME_FORMAT.DS_CANONICAL_NAME_EX:
                    {
                        DSNAME? v = LookupCanonicalName(dc, CanonicalNameFromCanonicalNameEx(name));
                        if (v == null)
                            rt = null;
                        else
                            rt = new DSNAME[] { v.Value };

                        if (EnvironmentConfig.TestDS)
                        {
                            domainName = DomainNameFromCanonicalName(name);
                        }
                    }
                    break;
                case (uint)formatOffered_Values.DS_NT4_ACCOUNT_NAME_SANS_DOMAIN:
                case (uint)formatOffered_Values.DS_NT4_ACCOUNT_NAME_SANS_DOMAIN_EX:
                    {
                        rt = LookupAttr(dc, flags, "sAMAccountName", name);
                    }
                    break;
                case (uint)formatOffered_Values.DS_ALT_SECURITY_IDENTITIES_NAME:
                    {
                        rt = LookupAttr(dc, flags, "altSecurityIdentities", name);
                    }
                    break;
                case (uint)formatOffered_Values.DS_USER_PRINCIPAL_NAME_AND_ALTSECID:
                    {
                        rt = LookupUPNAndAltSecID(dc, flags, true, name);
                        if (EnvironmentConfig.TestDS)
                        {
                            domainName = DomainNameFromUPN(name);
                        }
                    }
                    break;
                default:
                    rt = null;
                    break;
            }

            if (rt == null && domainName != null)
            {
                result.status = DS_NAME_ERROR.DS_NAME_ERROR_DOMAIN_ONLY;
                if (formatOffered == (uint)DS_NAME_FORMAT.DS_NT4_ACCOUNT_NAME
                    || formatOffered == (uint)DS_NAME_FORMAT.DS_USER_PRINCIPAL_NAME
                    || formatOffered == (uint)DS_NAME_FORMAT.DS_SERVICE_PRINCIPAL_NAME
                    || formatOffered == (uint)DS_NAME_FORMAT.DS_SID_OR_SID_HISTORY_NAME
                    || formatOffered == (uint)formatOffered_Values.DS_STRING_SID_NAME
                    || formatOffered == (uint)formatOffered_Values.DS_USER_PRINCIPAL_NAME_AND_ALTSECID)
                {
                    if (TrustInfo.IsDomainNameInTrustedForest(dc, domainName, ref referredDomain))
                    {
                        result.pDomain = referredDomain;
                        if ((flags & (uint)DRS_MSG_CRACKREQ_FLAGS.DS_NAME_FLAG_TRUST_REFERRAL) > 0)
                            result.status = DS_NAME_ERROR.DS_NAME_ERROR_TRUST_REFERRAL;
                        else
                            result.status = DS_NAME_ERROR.DS_NAME_ERROR_DOMAIN_ONLY;
                    }
                }

                return result;
            }

            if (rt == null)
            {
                result.status = DS_NAME_ERROR.DS_NAME_ERROR_NOT_FOUND;
                return result;
            }

            if (rt.Length > 1)
            {
                result.status = DS_NAME_ERROR.DS_NAME_ERROR_NOT_UNIQUE;
                return result;
            }

            DSNAME obj = rt[0];
            if (formatOffered == (uint)formatOffered_Values.DS_NT4_ACCOUNT_NAME_SANS_DOMAIN_EX)
            {
                string uacStr = GetAttributeValueInString(
                    dc,
                    LdapUtility.ConvertUshortArrayToString(obj.StringName),
                    "userAccountControl");
                uint uac = Convert.ToUInt32(uacStr);

                const uint ADS_UF_ACCOUNTDISABLE = 0x0002;
                const uint ADS_UF_TEMP_DUPLICATE_ACCOUNT = 0x0100;
                if ((uac & (ADS_UF_ACCOUNTDISABLE | ADS_UF_TEMP_DUPLICATE_ACCOUNT)) > 0)
                {
                    result.status = DS_NAME_ERROR.DS_NAME_ERROR_NOT_FOUND;
                    return result;
                }
            }

            string[] names = ConstructOutput(dc, obj, formatDesired);

            if ((names == null)
                && (obj.SidLen != 0)
                && ((flags & (uint)DRS_MSG_CRACKREQ_FLAGS.DS_NAME_FLAG_PRIVATE_RESOLVE_FPOS) > 0))
            {
                if (formatDesired == (uint)DS_NAME_FORMAT.DS_NT4_ACCOUNT_NAME
                    || formatDesired == (uint)DS_NAME_FORMAT.DS_DISPLAY_NAME
                    || formatDesired == (uint)DS_NAME_FORMAT.DS_CANONICAL_NAME
                    || formatDesired == (uint)DS_NAME_FORMAT.DS_CANONICAL_NAME_EX
                    || formatDesired == (uint)DS_NAME_FORMAT.DS_USER_PRINCIPAL_NAME
                    || formatDesired == (uint)formatDesired_Values.DS_USER_PRINCIPAL_NAME_FOR_LOGON
                    || formatDesired == (uint)DS_NAME_FORMAT.DS_SERVICE_PRINCIPAL_NAME)
                {
                    bool fCanonicalEx = false;
                    if (formatDesired == (uint)DS_NAME_FORMAT.DS_CANONICAL_NAME_EX)
                        fCanonicalEx = true;

                    result = LookupFPO(fCanonicalEx, obj, result);
                    return result;
                }
            }

            if (names == null)
            {
                if (formatDesired == (uint)DS_NAME_FORMAT.DS_SID_OR_SID_HISTORY_NAME
                    || formatDesired == (uint)DS_NAME_FORMAT.DS_UNKNOWN_NAME)
                {
                    result.status = DS_NAME_ERROR.DS_NAME_ERROR_RESOLVING;
                }
                else
                {
                    result.status = DS_NAME_ERROR.DS_NAME_ERROR_NO_MAPPING;
                }
                return result;
            }

            if (names.Length > 1)
            {
                result.status = DS_NAME_ERROR.DS_NAME_ERROR_NOT_UNIQUE;
                return result;
            }

            result.pName = names[0];
            if (EnvironmentConfig.TestDS)
            {
                string objDn = LdapUtility.ConvertUshortArrayToString(obj.StringName);
                DSNAME? dcDsname = GetDsName(dc, RetrieveDCSuffixFromDn(objDn));
                string domainDn = "";
                if (dcDsname != null)
                    domainDn = DomainDNSNameFromDomain(dc, dcDsname.Value);

                result.pDomain = domainDn; 
            }
            else
            {
                result.pDomain = "";
            }
            result.status = DS_NAME_ERROR.DS_NAME_NO_ERROR;

            return result;
        }

        ITestSite site;

    
    }
}
