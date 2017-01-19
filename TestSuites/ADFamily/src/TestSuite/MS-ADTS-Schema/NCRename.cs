// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.DirectoryServices.Protocols;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocol.TestSuites.ActiveDirectory.Adts.Schema
{
    public partial class DataSchemaTestSuite
    {
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("CDC")]
        [TestCategory("TDC")]
        public void Schema_TestNCRename()
        {
            NCRenameDiag diag = new NCRenameDiag(adAdapter.PDCIPAddress, adAdapter.PrimaryDomainDnsName, adAdapter.PrimaryDomainNetBiosName, adAdapter.DomainAdministratorName, adAdapter.DomainUserPassword);
            diag.AddChild(adAdapter.CDCIPAddress, adAdapter.ChildDomainDnsName, adAdapter.ChildDomainNetBiosName);
            diag.AddTrustedDomain(
                adAdapter.TDCNetbiosName,
                adAdapter.TrustDomainDnsName,
                adAdapter.TrustDomainNetBiosName,
                adAdapter.DomainAdministratorName,
                adAdapter.DomainUserPassword);
           List<string> errors =  diag.Diag();
           foreach (string str in errors)
           {
               BaseTestSite.Log.Add(LogEntryKind.CheckFailed, str);
           }
           BaseTestSite.Assert.AreEqual<int>(0, errors.Count, "Expected 0 error be found");
        }
    }


    public class NCRenameDiag
    {
        List<string> errors = new List<string>();

        string domainDNS;
        string domainNetbios;
        string domainDN;

        string dc;
        string user;
        string password;

        public NCRenameDiag(string dcAddr, string domainDNSName, string domainNBName, string userName, string pwd)
        {
            dc = dcAddr;
            domainDNS = domainDNSName.ToLower();
            domainNetbios = domainNBName.ToLower();
            domainDN = dnsToDN(domainDNS);
            user = userName;
            password = pwd;
        }

        struct TrustDomain
        {
            public string DCAddress;
            public string DomainDNS;
            public string DomainDN;
            public string DomainNetbios;
            public string User;
            public string Password;
        }

        List<TrustDomain> trusts = new List<TrustDomain>();
        List<TrustDomain> children = new List<TrustDomain>();

        public void AddTrustedDomain(string dc, string dns, string nb, string user, string pwd)
        {
            TrustDomain td = new TrustDomain();
            td.DCAddress = dc;
            td.DomainDNS = dns;
            td.DomainDN = dnsToDN(td.DomainDNS);
            td.DomainNetbios = nb;
            td.User = user;
            td.Password = pwd;
            trusts.Add(td);
        }

        public void AddChild(string dc, string dns, string nb)
        {
            TrustDomain td = new TrustDomain();
            td.DCAddress = dc;
            td.DomainDNS = dns;
            td.DomainDN = dnsToDN(td.DomainDNS);
            td.DomainNetbios = nb;
            children.Add(td);
        }


        public List<string> Diag()
        {
            LdapConnection con = new LdapConnection(
               new LdapDirectoryIdentifier(dc),
               new System.Net.NetworkCredential(user, password, domainDNS));

            string objectDN = "cn=enterprise configuration,cn=partitions,cn=configuration," + domainDN;

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("dnsroot", domainDNS);
            verifyAttributes(con, objectDN, dict);

            objectDN = "cn=enterprise schema,cn=partitions,cn=configuration," + domainDN;
            verifyAttributes(con, objectDN, dict);

            objectDN = "cn=" + domainNetbios + ",cn=partitions,cn=configuration," + domainDN;
            dict.Add("netbiosname", domainNetbios);
            dict.Add("msds-dnsrootalias", domainDNS);
            dict.Add("trustparent", null);
            dict.Add("roottrust", null);
            verifyAttributes(con, objectDN, dict);

            #region check children
            foreach (TrustDomain child in children)
            {
                LdapConnection ldap = new LdapConnection(
                       new LdapDirectoryIdentifier(child.DCAddress),
               new System.Net.NetworkCredential(user, password, domainDNS));
                objectDN = "cn=" + child.DomainNetbios + ",cn=partitions,cn=configuration," + domainDN;
                dict.Clear();
                dict.Add("dnsroot", child.DomainDNS);
                dict.Add("msds-dnsrootalias", child.DomainDNS);
                dict.Add("trustparent", domainDNS);
                dict.Add("roottrust", null);
                verifyAttributes(ldap, objectDN, dict);
            }
            #endregion

            #region check trusted
            foreach (TrustDomain trust in trusts)
            {
                LdapConnection ldap = new LdapConnection(
                      new LdapDirectoryIdentifier(trust.DCAddress),
              new System.Net.NetworkCredential(trust.User, trust.Password, trust.DomainDNS));
                objectDN = "cn=" + domainDNS + ",cn=system," + trust.DomainDN;
                dict.Clear();
                dict.Add("flatname", domainNetbios);
                dict.Add("trustpartner", domainDNS);
                verifyAttributes(ldap, objectDN, dict);


                objectDN = "cn=" + domainNetbios + "$,cn=users," + trust.DomainDN;
                dict.Clear();
                dict.Add("samaccountname", domainNetbios + "$");
                verifyAttributes(ldap, objectDN, dict);
            }
            #endregion

            return errors;
        }


        void verifyAttributes(LdapConnection con, string dn, Dictionary<string, string> attributes)
        {
            SearchResultAttributeCollection attrs = getAttributes(
                con,
               dn,
                attributes.Keys.ToArray());
            if (attrs == null)
                return;

            Dictionary<string, string>.Enumerator enumer = attributes.GetEnumerator();
            while (enumer.MoveNext())
            {
                if (!attrs.Contains(enumer.Current.Key) && enumer.Current.Value != null)
                {
                    errors.Add("Failed to find attribute " + enumer.Current.Key + " from object " + dn);
                }
                else
                {
                    string value = attrs[enumer.Current.Key].ToString().ToLower();
                    if (enumer.Current.Value.ToLower() != value)
                    {
                        errors.Add("expected: " + enumer.Current.Value.ToLower() + " , actual: " + value + ", attribute " + enumer.Current.Key + " of object " + dn);
                    }
                }
            }
        }


        SearchResultAttributeCollection getAttributes(LdapConnection con, string dn, string[] attributes)
        {
            SearchRequest req = null;
            SearchResponse res = null;
            SearchResultAttributeCollection ret = null;
            try
            {
                req = new SearchRequest(
                    dn,
                    "(objectclass=*)",
                     System.DirectoryServices.Protocols.SearchScope.Base,
                     attributes);

                res = (SearchResponse)con.SendRequest(req);
                if (res.Entries.Count == 0)
                    errors.Add("Failed to query object " + dn);
            }
            catch (Exception e)
            {
                errors.Add("Failed to get attributes from object " + dn + ", error info: " + e.Message);
            }
            return ret;
        }

        string dnsToDN(string dns)
        {
            string[] values = dns.Replace(" ", "").Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            string ret = "";
            for (int i = 0; i < values.Length; i++)
            {
                ret += "DC=";
                ret += values[i];
                ret += ",";
            }

            return ret.Remove(ret.Length - 1);
        }
    }
}
