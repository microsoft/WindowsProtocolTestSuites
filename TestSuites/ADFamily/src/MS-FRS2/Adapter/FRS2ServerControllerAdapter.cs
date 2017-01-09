// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.Messages;
using Microsoft.Protocols.TestTools.Messages.Marshaling;
using System.DirectoryServices.Protocols;
using System.DirectoryServices;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Microsoft.Protocols.TestSuites.MS_FRS2
{
    class FRS2ServerControllerAdapter : ManagedAdapterBase, IFRS2ServerControllerAdapter
    {
        #region RetriveObectAttributes

        /// <summary>
        /// Retrieve Object with attributes is to retrieve server Object Variables.
        /// </summary>
        /// <param name="distinguishedName"> ServerDistinguished Name</param>
        /// <param name="serverName">Server Name</param>
        /// <param name="ldapFilter">Fileter String</param>
        /// <param name="attributes">Attributes to be queried</param>
        /// <param name="scope">Search Scope</param>
        /// <param name="searchResponse">Search Response</param>
        /// <returns></returns>
        public void RetrieveObjectwithattributes(string distinguishedName, string serverName, string ldapFilter, string[] attributes, System.DirectoryServices.Protocols.SearchScope scope, out  System.DirectoryServices.Protocols.SearchResponse searchResponse)
        {
            using (LdapConnection serverConnection = new LdapConnection(serverName))
            {
                serverConnection.AuthType = AuthType.Basic;
                serverConnection.SessionOptions.ProtocolVersion = 3;
                serverConnection.Bind(new System.Net.NetworkCredential(ConfigStore.DomainNetbiosName + "\\" + ConfigStore.AdminName, ConfigStore.AdminPassword));
                int pageSize = 3000;
                SearchRequest searchRequest = new SearchRequest(distinguishedName, ldapFilter, scope, attributes);
                PageResultRequestControl requestControl = new PageResultRequestControl(pageSize);
                searchRequest.Controls.Add(requestControl);
                searchResponse = (SearchResponse)serverConnection.SendRequest(searchRequest);
            }
        }

        #endregion

        #region AD ValidationLogic
        /// <summary>
        /// AdValidation is used to validate AD requirements.
        /// This method verifies the particular property of the object exists or not.
        /// </summary>
        /// <param name="GuidString">Distinguished Name</param>
        /// <param name="GuidClass">ladpFilter</param>
        /// <param name="refString">attribute</param>
        /// <returns></returns>
        public bool AdValidation(String GuidString, string GuidClass, string refString)
        {
            if (GuidString == null)
                return false;
            bool flag = false;
            //Ladap Connection
            using (LdapConnection connection = new LdapConnection(ConfigStore.SutLdapAddress))
            {
                //Authentication type is Basic
                connection.AuthType = AuthType.Basic;
                connection.Bind(new System.Net.NetworkCredential(ConfigStore.DomainNetbiosName + "\\" + ConfigStore.AdminName, ConfigStore.AdminPassword));

                //Search Request
                SearchRequest request = new SearchRequest(GuidString, GuidClass, System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { refString });
                //Search Response
                SearchResponse response = (SearchResponse)connection.SendRequest(request);
                Guid guid = Guid.Empty;
                foreach (SearchResultEntry entry in response.Entries)
                {
                    SearchResultAttributeCollection attributeCollection = entry.Attributes;
                    foreach (DirectoryAttribute attribute in attributeCollection.Values)
                    {
                        for (int j = 0; j < attribute.Count; j++)
                        {
                            //If Attribute is string type
                            if (attribute[j] is string)
                            {
                                flag = true;
                            }
                            //If Attribute is GUID
                            if (attribute[j] is byte[])
                            {
                                byte[] x = attribute[j] as byte[];
                                guid = new Guid(x);
                                if (guid != null)
                                {
                                    flag = true;
                                }
                            }

                            if (attribute[j] is bool)
                            {
                                if ((bool)attribute[j] == true)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }

                            }
                            //if attribute is Int
                            if (attribute[j] is int)
                            {
                                int attvalue = (int)attribute[j];
                                if (attvalue == 0)
                                {
                                    flag = true;
                                }
                            }
                        }
                    }
                }
            }
            return flag;

        }
        /// <summary>
        /// Adavalidation method is used to Validate AD related Requiremnts.
        /// This method compares integer type attribute values of the specified object
        /// </summary>
        /// <param name="GuidString">Distinguished Name</param>
        /// <param name="GuidClass">ladpFilter</param>
        /// <param name="refString">Attribute</param>
        /// <param name="comparreValue">ExprectedValue</param>
        /// <returns></returns>

        public bool AdValidation(String GuidString, string GuidClass, string refString, int comparreValue)
        {
            if (GuidString == null)
                return false;
            using (LdapConnection connection = new LdapConnection(ConfigStore.DomainDnsName))
            {
                connection.AuthType = AuthType.Basic;
                connection.Bind(new System.Net.NetworkCredential(ConfigStore.AdminName, ConfigStore.AdminPassword, ConfigStore.DomainNetbiosName));
                bool flag = false;
                SearchRequest request = new SearchRequest(GuidString, GuidClass, System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { refString });
                SearchResponse response = (SearchResponse)connection.SendRequest(request);
                Guid guid = Guid.Empty;
                foreach (SearchResultEntry entry in response.Entries)
                {
                    SearchResultAttributeCollection attributeCollection = entry.Attributes;
                    foreach (DirectoryAttribute attribute in attributeCollection.Values)
                    {

                        for (int j = 0; j < attribute.Count; j++)
                        {
                            if (attribute[j] is string)
                            {
                                int attvalue = Convert.ToInt32(attribute[j]);

                                if (attvalue == comparreValue)
                                {
                                    flag = true;
                                }
                            }
                        }
                    }
                }
                return flag;
            }
        }

        /// <summary>
        /// Adavalidation method is used to Validate AD related Requiremnts.
        /// This method compares boolean type attribute values of the specified object
        /// </summary>
        /// <param name="GuidString">Distinguished Name</param>
        /// <param name="GuidClass">ladpFilter</param>
        /// <param name="refString">Attribute</param>
        /// <param name="comparreValue">ExprectedValue</param>
        /// <returns></returns>

        public bool AdValidation(String GuidString, string GuidClass, string refString, bool comparreValue)
        {
            if (GuidString == null)
                return false;
            using (LdapConnection connection = new LdapConnection(ConfigStore.DomainDnsName))
            {
                connection.AuthType = AuthType.Basic;
                connection.Bind(new System.Net.NetworkCredential(ConfigStore.AdminName, ConfigStore.AdminPassword, ConfigStore.DomainNetbiosName));
                bool flag = false;
                SearchRequest request = new SearchRequest(GuidString, GuidClass, System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { refString });
                SearchResponse response = (SearchResponse)connection.SendRequest(request);
                Guid guid = Guid.Empty;
                foreach (SearchResultEntry entry in response.Entries)
                {
                    SearchResultAttributeCollection attributeCollection = entry.Attributes;
                    foreach (DirectoryAttribute attribute in attributeCollection.Values)
                    {
                        for (int j = 0; j < attribute.Count; j++)
                        {
                            if (attribute[j] is string)
                            {
                                bool attvalue = Convert.ToBoolean(attribute[j]);
                                if (attvalue == comparreValue)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
                return flag;
            }
        }

        /// <summary>
        /// SetAccessRights method sets the particular access right for a particular user on a particualr AD Container/Object
        /// </summary>
        /// <param name="DN">The distinguished name of the Container/Object.</param>
        /// <param name="user">The name of the user to whom the permissions to be set</param>
        /// <param name="domain">The name of the domain to which user is belongs to</param>
        /// <param name="accessRight">The name of the access right to be set</param>
        /// <param name="controlType">Allow/Deny particular ActiveDirecotyRights</param>
        /// <returns></returns>
        public bool SetACLs(string DN, string user, string domain, ActiveDirectoryRights accessRight, AccessControlType controlType)
        {
            using (DirectoryEntry entry = new DirectoryEntry("LDAP://" + DN, ConfigStore.AdminName, ConfigStore.AdminPassword, AuthenticationTypes.Secure))
            {
                ActiveDirectorySecurity sd = entry.ObjectSecurity;
                NTAccount accountName = new NTAccount(domain, user);
                IdentityReference acctSID = accountName.Translate(typeof(SecurityIdentifier));
                ActiveDirectoryAccessRule myRule = new ActiveDirectoryAccessRule(new SecurityIdentifier(acctSID.Value), accessRight, controlType);
                sd.AddAccessRule(myRule);
                entry.ObjectSecurity.AddAccessRule(myRule);
                entry.CommitChanges();
                return true;
            }

        }
        #endregion

        #region Initialize GUIDs
        /// <summary>
        /// InitializeGuid method is for initializeing ConnectionId,ContentSetId, ReplicationId
        /// </summary>
        /// <param name="GuidString">DistiguishedName</param>
        /// <param name="GuidClass">ldaptFilter</param>
        /// <returns></returns>
        public Guid InitializeGuid(String GuidString, string GuidClass)
        {
            Guid guid = Guid.Empty;
            try
            {
                using (LdapConnection connection = new LdapConnection(ConfigStore.SutLdapAddress))
                {
                    connection.AuthType = AuthType.Basic;
                    connection.SessionOptions.ProtocolVersion = 3;
                    connection.Bind(new System.Net.NetworkCredential(ConfigStore.DomainNetbiosName + "\\" + ConfigStore.AdminName, ConfigStore.AdminPassword));
                    SearchRequest request = new SearchRequest(GuidString, GuidClass, System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { "objectGUID" });
                    SearchResponse response = (SearchResponse)connection.SendRequest(request);

                    foreach (SearchResultEntry entry in response.Entries)
                    {
                        SearchResultAttributeCollection attributeCollection = entry.Attributes;
                        foreach (DirectoryAttribute attribute in attributeCollection.Values)
                        {
                            for (int j = 0; j < attribute.Count; j++)
                            {
                                if (attribute[j] is byte[])
                                {
                                    byte[] x = attribute[j] as byte[];
                                    guid = new Guid(x);

                                }
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            return guid;
        }
        #endregion

        #region Quering for the msDFSR-FileFilter

        public string GetDFSRFilters(int contentId, string filterString)
        {
            String fileType = null;
            using (LdapConnection connection = new LdapConnection(ConfigStore.SutLdapAddress))
            {
                connection.AuthType = AuthType.Basic;
                connection.Bind(new System.Net.NetworkCredential(ConfigStore.DomainNetbiosName + "\\" + ConfigStore.AdminName, ConfigStore.AdminPassword));
                SearchRequest request = new SearchRequest(typeof(ConfigStore).GetField("ContentId" + contentId.ToString()).GetValue(null).ToString(), "(objectClass=msDFSR-ContentSet)", System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { filterString });
                SearchResponse response = (SearchResponse)connection.SendRequest(request);
                foreach (SearchResultEntry entry in response.Entries)
                {
                    SearchResultAttributeCollection attributeCollection = entry.Attributes;
                    foreach (DirectoryAttribute attribute in attributeCollection.Values)
                    {
                        for (int j = 0; j < attribute.Count; j++)
                        {
                            //If Attribute is string type
                            if (attribute[j] is string)
                            {
                                fileType = attribute[j].ToString();

                            }
                        }
                    }
                }
            }
            return fileType;
        }
        #endregion

        public Guid GetNtdsConnectionGuid(string path, string fromServer)
        {
            Guid guid = Guid.Empty;
            string from = "";
            using (LdapConnection connection = new LdapConnection(ConfigStore.SutLdapAddress))
            {
                connection.AuthType = AuthType.Basic;
                connection.SessionOptions.ProtocolVersion = 3;
                connection.Bind(new System.Net.NetworkCredential(ConfigStore.DomainNetbiosName + "\\" + ConfigStore.AdminName, ConfigStore.AdminPassword));
                SearchRequest request = new SearchRequest(path, "(objectclass=ntdsconnection)", System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { "objectGUID", "fromServer" });
                SearchResponse response = null;
                try
                {
                     response = (SearchResponse)connection.SendRequest(request);
                }
                catch(DirectoryOperationException e)
                {
                    Site.Assume.Fail("Failed to find {0}: {1}", path, e.Message);
                }
                
                foreach (SearchResultEntry entry in response.Entries)
                {
                    SearchResultAttributeCollection attributeCollection = entry.Attributes;
                    foreach (DirectoryAttribute attribute in attributeCollection.Values)
                    {
                        for (int j = 0; j < attribute.Count; j++)
                        {
                            if (attribute[j] is byte[])
                            {
                                byte[] x = attribute[j] as byte[];
                                guid = new Guid(x);

                            }
                            if (attribute[j] is string)
                                from = attribute[j].ToString();


                        }
                        if (0 == string.Compare(from.ToLower(), fromServer.ToLower()))
                            return guid;
                    }
                }
            }
            throw new Exception("Failed to find replication partner's NTDS Connection guid");
        }
    }
}
