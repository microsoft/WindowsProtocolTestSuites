// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.DirectoryServices.Protocols;
using System.Net;
using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocol.TestSuites.Kerberos.Adapter
{
    /// <summary>
    /// SutControlAdapter is a class, which implements ISutControlAdapter interface.
    /// This is a managed adapter having methods to verify state change in Windows environment
    /// </summary>
    public class SutControlAdapter: ManagedAdapterBase, ISutControlAdapter
    {
        /// <summary>
        /// This method is to help enable the compound identity feature on the computer account in the specific domain.
        /// </summary>
        /// <param name="domainName">The domain name of the service principal.</param>
        /// <param name="computerName">The host name of the service principal.</param>
        /// <param name="adminName">Need administrator's credential to modify active directory account.</param>
        /// <param name="adminPwd">Need administrator's credential to modify active directory account.</param>
        public void enableCompId(string domainName, string computerName, string adminName, string adminPwd)
        {
            LdapConnection connection = new LdapConnection(domainName);
            NetworkCredential cred = new NetworkCredential(adminName, adminPwd, domainName);
            connection.Credential = cred;
            string dn = PacHelper.GetDomainDnFromDomainName(domainName);
            string targetOu = "cn=Computers," + dn;
            computerName = computerName.Replace("$", "");
            string filter = "cn=" + computerName;
            string[] attributesToReturn = new string[] { "msDS-SupportedEncryptionTypes" };
            SearchRequest searchRequest = new SearchRequest(targetOu, filter, SearchScope.Subtree, attributesToReturn);

            SearchResponse searchResponse = (SearchResponse)connection.SendRequest(searchRequest);
            SearchResultAttributeCollection attributes = searchResponse.Entries[0].Attributes;

            object attributeValue = null;
            attributeValue = PacHelper.getAttributeValue(attributes, "msDS-SupportedEncryptionTypes");
            uint? supportedEncTypes = (uint?)Convert.ToInt32(attributeValue);

            uint compIdFlag = 131072;
            if ((supportedEncTypes.Value & compIdFlag) != compIdFlag)
            {
                string computerDN = filter + "," + targetOu;
                supportedEncTypes = supportedEncTypes + compIdFlag;
                ModifyRequest modRequest = new ModifyRequest(computerDN, DirectoryAttributeOperation.Replace, "msDS-SupportedEncryptionTypes", supportedEncTypes.ToString());
                ModifyResponse modResponse = (ModifyResponse)connection.SendRequest(modRequest);
            }
        }

        /// <summary>
        /// This method is used to enable or disable selective authentication for an inbound trust for the localForest
        /// </summary>
        /// <param name="localForest">Local Forest Name</param>
        /// <param name="userName">Domain admin user name for Local Forest</param>
        /// <param name="password">Domain admin password for Local Forest</param>
        /// <param name="targetForest">Target Forest Name</param>
        /// <param name="enable">true or false</param>
        public void setSelectiveAuth(string localForest, string userName, string password, string targetForest, bool enable)
        {
            System.DirectoryServices.ActiveDirectory.DirectoryContext context = new System.DirectoryServices.ActiveDirectory.DirectoryContext(System.DirectoryServices.ActiveDirectory.DirectoryContextType.Forest, localForest, userName, password);
            System.DirectoryServices.ActiveDirectory.Forest forest = System.DirectoryServices.ActiveDirectory.Forest.GetForest(context);
            forest.SetSelectiveAuthenticationStatus(targetForest, enable);
        }


        /// <summary>
        /// This method is used to get an authentication policy TGT life time by policy name and attribute name
        /// If the TGT lifetime is not set, null will return
        /// </summary>
        /// <param name="domainName">Domain Name</param>
        /// <param name="policyname">authentication policy name</param>
        /// <param name="tgtlifetimeattributename">the lifetime attribute name, such as msds-ComputerTGTLifetime, msds-UserTGTLifetime or msds-ServiceTGTLifetime</param>
        /// <param name="adminName">Admin user Name</param>
        /// <param name="adminPwd">Admin password</param>
        public double? getAuthPolicyTGTLifeTime(string domainName, string policyName, string tgtLifetimeAttributeName, string adminName, string adminPwd)
        {
            LdapConnection connection = new LdapConnection(domainName);
            NetworkCredential cred = new NetworkCredential(adminName, adminPwd, domainName);
            connection.Credential = cred;
            string dn = PacHelper.GetDomainDnFromDomainName(domainName);
            string targetOu = "CN=" + policyName + ",CN=AuthN Policies,CN=AuthN Policy Configuration,CN=Services,CN=Configuration,DC=" + domainName + ",DC=com";

            string filter = "CN=" + policyName;
            string[] attributesToReturn = new string[] { tgtLifetimeAttributeName };

            double? tgtLifeTime = null;
            SearchRequest searchRequest = null;
            SearchResponse searchResponse = null;
            try
            {
                searchRequest = new SearchRequest(targetOu, filter, SearchScope.Subtree, attributesToReturn);
                searchResponse = (SearchResponse)connection.SendRequest(searchRequest);
                SearchResultAttributeCollection attributes = searchResponse.Entries[0].Attributes;
                object attributeValue= PacHelper.getAttributeValue(attributes, tgtLifetimeAttributeName);
                tgtLifeTime = (double?)Convert.ToDouble(attributeValue);
                    
            }
            catch
            {
                throw new InvalidOperationException("Request attribute failed with targetOU: " + targetOu + ", filter: " + filter + ", attribute: " + tgtLifetimeAttributeName);
            }

            return tgtLifeTime;
        }

        /// <summary>
        /// This method is used to get attribute display name of an account
        /// </summary>
        /// <param name="domainName">Local domain Name</param>
        /// <param name="accountName">Account name, user name or computer name</param>
        /// <param name="accountType">Users or computers</param>
        /// <param name="attributename">The attribute of account to query</param>
        /// <param name="adminName">Admin user Name</param>
        /// <param name="adminPwd">Admin password</param>
        public string getAccountAttributeDN(string domainName, string accountName, string accountType, string attributeName, string adminName, string adminPwd)
        {
            LdapConnection connection = new LdapConnection(domainName);
            NetworkCredential cred = new NetworkCredential(adminName, adminPwd, domainName);
            connection.Credential = cred;
            string dn = PacHelper.GetDomainDnFromDomainName(domainName);
            string targetOu = "CN=" + accountName + ",CN=" + accountType + ",DC=" + domainName + ",DC=com";

            string filter = "CN=" + accountName;
            string[] attributesToReturn = new string[] { attributeName };
            
            SearchRequest searchRequest = null;
            SearchResponse searchResponse = null;
            string attributeValue = null;

            try
            {
                searchRequest = new SearchRequest(targetOu, filter, SearchScope.Subtree, attributesToReturn);

                searchResponse = (SearchResponse)connection.SendRequest(searchRequest);
                SearchResultAttributeCollection attributes = searchResponse.Entries[0].Attributes;
                object attribute = null;
                attribute = PacHelper.getAttributeValue(attributes, attributeName);
                attributeValue = Convert.ToString(attribute);
                    
            }
            catch
            {
                throw new InvalidOperationException("Request attribute failed with targetOU: " + targetOu + ", filter: " + filter + ", attribute: " + attributeName);
            }

            return attributeValue;
        }
        
    }
}
