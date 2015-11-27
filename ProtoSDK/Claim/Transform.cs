// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Text;
using System.Xml;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory
{

    public class ClaimTransformer
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="dc">DC name or address</param>
        /// <param name="domain">domain DNS name</param>
        /// <param name="user">user name for LDAP connection</param>
        /// <param name="password">password of user</param>
        public ClaimTransformer(string dc, string domain, string user, string password)
        {
            DomainController = dc;
            DomainDNSName = domain;
            UserName = user;
            Password = password;
        }

        private LdapConnection connect()
        {
            return new LdapConnection(new LdapDirectoryIdentifier(DomainDNSName), new System.Net.NetworkCredential(UserName, Password, DomainDNSName));
        }

        /// <summary>
        /// user name for connection
        /// </summary>
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// password for user
        /// </summary>
        public string Password
        {
            get;
            set;
        }

        /// <summary>
        /// DNS name of the domain
        /// </summary>
        public string DomainDNSName
        {
            get
            {
                return domainDNS;
            }
            set
            {
                domainDNS = value;
                domainNC = dnsNameToDN(value);
            }
        }

        /// <summary>
        /// machine name of the DC
        /// </summary>
        public string DomainController
        {
            get;
            set;
        }

        /// <summary>
        /// domain DNS name
        /// </summary>
        string domainDNS;

        /// <summary>
        /// domain NC Distinguished Name
        /// </summary>
        string domainNC;

        /// <summary>
        /// convert DNS name to Distinguished Name format
        /// </summary>
        /// <param name="dns">dns name</param>
        /// <returns>Distinguished Name</returns>
        string dnsNameToDN(string dns)
        {
            string[] tmps = dns.ToLower().Replace(" ", "").Split(new string[] { ",", "." }, StringSplitOptions.RemoveEmptyEntries);
            string ret = "";
            foreach (string s in tmps)
            {
                ret += "DC=";
                ret += s;
                ret += ",";
            }

            return ret.Remove(ret.Length - 1);
        }

        public Win32ErrorCode_32 TransformClaimsOnTrustTraversal(CLAIMS_ARRAY[] input, string trustName, bool fIncomingDirection, out List<CLAIMS_ARRAY> output)
        {
            output = null;
            //found trust
            SearchRequest search = new SearchRequest(
                "cn=system," + domainNC,
                "(cn=" + trustName + ")",
                 SearchScope.OneLevel,
                 new string[] { "*" });
            bool hasException = false;
            SearchResponse response = null;
            do
            {
                try
                {
                    using (LdapConnection connection = connect())
                    {
                        response = (SearchResponse)connection.SendRequest(search);
                    }
                }
                catch
                {
                    hasException = true;
                }
            } while (hasException);
            if (response.ResultCode != ResultCode.Success || response.Entries.Count == 0)
                return Win32ErrorCode_32.ERROR_INVALID_PARAMETER;
            string xml = null;
            Win32ErrorCode_32 err = getClaimsTransformationRuleXml(trustName, response.Entries[0].DistinguishedName, fIncomingDirection, out xml);
            if (err != Win32ErrorCode_32.ERROR_SUCCESS && err != Win32ErrorCode_32.ERROR_INVALID_FUNCTION)
                return Win32ErrorCode_32.ERROR_SUCCESS;
            string text = null;
            if (xml != null)
            {
                getTransformRulesText(xml, out text);
            }
            output = SimpleCTAEngine.Run(input, text);
            return Win32ErrorCode_32.ERROR_SUCCESS;
        }

        Win32ErrorCode_32 getClaimsTransformationRuleXml(string trustName, string trustObjDN, bool fIncomingDirection, out string ruleXml)
        {
            ruleXml = null;
            SearchRequest search = new SearchRequest(
                "cn=claims transformation policies,cn=claims configuration,cn=services,cn=configuration," + domainNC,
                "(objectclass=msds-claimstransformationpolicytype)",
                 SearchScope.OneLevel,
                new string[] { "*" });
            using (LdapConnection connection = connect())
            {
                SearchResponse policiesResponse = (SearchResponse)connection.SendRequest(search);
                string claimsTransformObject = null;
                search = new SearchRequest(
                    trustObjDN,
                    "(objectclass=*)",
                     SearchScope.Base,
                    new string[] { "msDS-IngressClaimsTransformationPolicy", "msDS-EgressClaimsTransformationPolicy" });
                SearchResponse trustObj = (SearchResponse)connection.SendRequest(search);
                if (trustObj.ResultCode != ResultCode.Success || trustObj.Entries.Count == 0)
                    return Win32ErrorCode_32.ERROR_INVALID_FUNCTION;
                if (fIncomingDirection)
                {
                    if (trustObj.Entries[0].Attributes["msDS-IngressClaimsTransformationPolicy"] == null || trustObj.Entries[0].Attributes["msDS-IngressClaimsTransformationPolicy"].Count == 0)
                        return Win32ErrorCode_32.ERROR_INVALID_FUNCTION;
                    claimsTransformObject = trustObj.Entries[0].Attributes["msDS-IngressClaimsTransformationPolicy"][0].ToString();
                }
                else
                {
                    if (trustObj.Entries[0].Attributes["msDS-EgressClaimsTransformPolicy"] == null || trustObj.Entries[0].Attributes["msDS-EgressClaimsTransformationPolicy"].Count == 0)
                        return Win32ErrorCode_32.ERROR_INVALID_FUNCTION;
                    claimsTransformObject = trustObj.Entries[0].Attributes["msDS-EgressClaimsTransformationPolicy"][0].ToString();
                }
                foreach (SearchResultEntry entry in policiesResponse.Entries)
                {
                    if (entry.DistinguishedName.ToLower() == claimsTransformObject.ToLower())
                    {
                        if (entry.Attributes["msDS-TransformationRules"] != null && entry.Attributes["msDS-TransformationRules"].Count != 0)
                        {
                            ruleXml = entry.Attributes["msDS-TransformationRules"][0].ToString();
                            return Win32ErrorCode_32.ERROR_SUCCESS;
                        }
                    }
                }

                return Win32ErrorCode_32.ERROR_INVALID_PARAMETER;
            }
        }

        Win32ErrorCode_32 getTransformRulesText(string xml, out string text)
        {
            text = null;
            if (xml == null)
                return Win32ErrorCode_32.ERROR_SUCCESS;

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.XmlResolver = null;
            using (System.IO.StringReader sr = new System.IO.StringReader(xml))
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.DtdProcessing = DtdProcessing.Prohibit;
                settings.XmlResolver = null;
                using (System.Xml.XmlReader xmlreader = System.Xml.XmlReader.Create(sr, settings))
                {
                    doc.Load(xmlreader);
                    int version = -1;
                    System.Xml.XmlNode ruleNode = null;
                    for (int i = 0; i < doc.ChildNodes.Count; i++)
                    {
                        if (doc.ChildNodes[i].Name == "ClaimsTransformationPolicy")
                        {
                            for (int j = 0; j < doc.ChildNodes[i].ChildNodes.Count; j++)
                            {
                                if (doc.ChildNodes[i].ChildNodes[j].Name == "Rules")
                                {
                                    if (doc.ChildNodes[i].ChildNodes[j].Attributes["version"] == null)
                                        return Win32ErrorCode_32.ERROR_INVALID_DATA;
                                    if (!int.TryParse(doc.ChildNodes[i].ChildNodes[j].Attributes["version"].Value, out version))
                                        return Win32ErrorCode_32.ERROR_INVALID_DATA;
                                    ruleNode = doc.ChildNodes[i].ChildNodes[j];
                                    break;
                                }
                            }
                        }
                    }
                    if (version != 1)
                        return Win32ErrorCode_32.ERROR_INVALID_DATA;

                    text = ruleNode.InnerText;
                    return Win32ErrorCode_32.ERROR_SUCCESS;
                }
            }
        }
    }
}
