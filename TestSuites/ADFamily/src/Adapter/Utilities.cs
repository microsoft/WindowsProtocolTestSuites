// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.AccountManagement;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.IO;
using System.Net;

using Microsoft.Protocols.TestTools;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;
using System.Globalization;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts;
using System.Xml.Serialization;
using System.Xml;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Common
{
    /// <summary>
    /// Contains methods to check for Access rights
    /// </summary>
    public static class Utilities
    {
        #region Variables

        #endregion

        static SearchResult searchResult;
        static bool hasChild;

        public static string DomainName;
        public static string DomainAdmin;
        public static string DomainAdminPassword;
        public static string TargetServerFqdn;
        
        // The added list in the test case
        private static List<string> addedObjectDNList = new List<string>();
        public static Stack<string> testObjects = new Stack<string>();

        /// <summary>
        /// Return the added entries in the test case
        /// </summary>
        /// Disable CA1002, because according to current test suite design, there is no need to remove the expose list 
        /// Disable CA1024, as this property is appropriate to use 
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public static List<string> getAddedList()
        {
            return addedObjectDNList;
        }

        #region LDS

        /// <summary>
        /// GetLdsDomainDN method gets the LDS domain name present on server.
        /// </summary>
        /// <param name="objectGuid">LDS server name with port</param>
        /// <returns>Returns the LDS domain name</returns>
        public static string GetLdsDomainDN(string adamServerName)
        {
            string domainDN = null;
            using (AdamInstance adamInstance = 
                AdamInstance.GetAdamInstance(new DirectoryContext(DirectoryContextType.DirectoryServer, adamServerName)))
            {
                domainDN = adamInstance.ConfigurationSet.Schema.Name;
            }
            if (null != domainDN)
            {
                int index = domainDN.LastIndexOf(",");
                if (index >= 0)
                {
                    domainDN = domainDN.Substring(index + 1); // drop cn=schema and cn=configuration
                }
            }
            return domainDN;
        }

        #endregion

        /// <summary>
        /// Convert Guid object to octet string type
        /// </summary>
        /// <param name="objectGuid">Guid type object</param>
        /// <returns>octet string type</returns>
        public static string Guid2OctetString(Guid objectGuid)
        {
            byte[] byteGuid = objectGuid.ToByteArray();
            StringBuilder sb = new StringBuilder();
            foreach (byte b in byteGuid)
            {
                sb.Append(@"\" + b.ToString("x2"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Return whether a special optional feature is enabled.
        /// </summary>
        /// <param name="scopeDN">represents the optional feature enabled scope. There are three scopes according to TD
        /// 1. ForestScope, the RDN is CN=Partitions,CN=Configuration
        /// 2. DomainScope, the DN is the domain root NC
        /// 3. ServerScope, the object which represents this scope is an nTDSDSA object</param>
        /// <param name="featureDN">The object represents the optional feature</param>
        /// <returns>Whether the optional feature is enabled in the specified scope</returns>
        public static bool IsOptionalFeatureEnabled(string scopeDN, string featureDN)
        {
            bool retValue = false;
            DirectoryEntry entry = new DirectoryEntry("LDAP://" + TargetServerFqdn + "/" + scopeDN);
            if (entry.Properties["msDS-EnabledFeature"].Value == null)
            {
                retValue = false;
            }
            else if (entry.Properties["msDS-EnabledFeature"].Value.ToString().ToLower(CultureInfo.InvariantCulture)
                .Contains(featureDN.ToLower(CultureInfo.InvariantCulture)))
            {
                retValue = true;
            }

            return retValue;
        }

        #region Object Operations
        
        /// <summary>
        /// Return the added entries in the test case
        /// </summary>
        /// <param name="serverName">The server for connection.</param>
        /// <param name="serverPort">The port number for connection.</param>
        /// <param name="objectDN">The distinguished name of the entry.</param>
        /// <param name="newParentDN">The parent name of new object.</param>
        public static bool MoveEntry(
            string serverName,
            string serverPort,
            string objectDN,
            string newParentDN,
            string username = null,
            string password = null)
        {
            if (username == null
                && password == null)
            {
                username = DomainAdmin;
                password = DomainAdminPassword;
            }

            using (DirectoryEntry objectEntry = new DirectoryEntry(
                string.Format("LDAP://{0}:{1}/{2}", serverName, serverPort, objectDN),
                username,
                password))
            {
                using (DirectoryEntry newParentEntry = new DirectoryEntry(
                    string.Format("LDAP://{0}:{1}/{2}", serverName, serverPort, newParentDN),
                    username,
                    password))
                {
                    try
                    {
                        objectEntry.MoveTo(newParentEntry, objectEntry.Name);
                        objectEntry.CommitChanges();
                        newParentEntry.CommitChanges();
                        objectEntry.Close();
                        newParentEntry.Close();

                        testObjects.Push(string.Format("moveEntry:{0}->{1}", objectDN, newParentDN));
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Return the added entries in the test case
        /// </summary>
        /// <param name="serverName">The server for connection.</param>
        /// <param name="serverPort">The port number for connection.</param>
        /// <param name="objectDN">The distinguished name of the entry.</param>
        /// <param name="newName">The new name of the object.</param>
        public static bool RenameEntry(
            string serverName,
            string serverPort,
            string objectDN,
            string newName,
            string username = null,
            string password = null)
        {
            if (username == null
                && password == null)
            {
                username = DomainAdmin;
                password = DomainAdminPassword;
            }

            using (DirectoryEntry de = new DirectoryEntry(
                string.Format("LDAP://{0}:{1}/{2}", serverName, serverPort, objectDN),
                username,
                password))
            {
                try
                {
                    de.Rename("CN=" + newName);
                    de.CommitChanges();
                    de.Close();

                    testObjects.Push(string.Format("renameEntry:{0}->{1}", objectDN, newName));
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Return the entry distinguishedName
        /// </summary>
        /// <param name="serverName">The server for connection.</param>
        /// <param name="serverPort">The port number for connection.</param>
        /// <param name="parentDN">The parent DN to look for the object.</param>
        /// <param name="objectClass">The objectClass of the entry.</param>
        /// <param name="objectName">The object CN of the entry.</param>
        /// <param name="newName">The new name of the object.</param>
        public static string GetEntryDistinguishedName(
            string serverName,
            string serverPort,
            string parentDN,
            string objectClass,
            string objectName,
            string username = null,
            string password = null)
        {
            if (username == null
                && password == null)
            {
                username = DomainAdmin;
                password = DomainAdminPassword;
            }

            string result;

            using (DirectoryEntry de = new DirectoryEntry(
                string.Format("LDAP://{0}:{1}/{2}", serverName, serverPort, parentDN),
                username,
                password))
            {
                if(string.IsNullOrEmpty(objectName))
                {
                    objectName = "*";
                }
                using (DirectorySearcher searcher = new DirectorySearcher(
                    de,
                    string.Format("(&(objectClass={0})(cn={1}))", objectClass, objectName)))
                    {
                        try
                        {
                            result = searcher.FindOne().Properties["distinguishedName"][0].ToString();
                        }
                        catch
                        {
                            result = null;
                        }
                    }
                de.Close();
            }

            return result;
        }

        /// <summary>
        /// Set the entry distinguishedName
        /// </summary>
        /// <param name="serverName">The server for connection.</param>
        /// <param name="serverPort">The port number for connection.</param>
        /// <param name="objectDN">The object DN to be set.</param>
        /// <param name="attributeName">The name of the attribute to be set.</param>
        /// <param name="attributeValue">The value of the attribute to be set.</param>
        public static bool SetAttributeForEntry(
            string serverName,
            string serverPort,
            string objectDN,
            string attributeName,
            string attributeValue,
            string username = null,
            string password = null)
        {
            if (username == null
                && password == null)
            {
                username = DomainAdmin;
                password = DomainAdminPassword;
            }

            using (DirectoryEntry de = new DirectoryEntry(
                string.Format("LDAP://{0}:{1}/{2}", serverName, serverPort, objectDN),
                username,
                password))
            {   
                try
                {
                    de.InvokeSet(attributeName, attributeValue);
                    de.CommitChanges();
                    de.Close();

                    testObjects.Push(string.Format("modify:{0}", objectDN));
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        #endregion

        #region User Operations

        /// <summary>
        /// Create a user account
        /// </summary>
        /// <param name="serverName">The server for connection.</param>
        /// <param name="serverPort">The port number for connection.</param>
        /// <param name="parentDN">The distinguished name of the entry.</param>
        /// <param name="newUserName">The name of the new user.</param>
        /// <param name="newUserPassword">The password of the new user.</param>
        /// <returns>true for success, false for failed</returns>
        public static bool NewUser(
            string serverName,
            string serverPort,
            string parentDN,
            string newUserName,
            string newUserPassword,
            string username = null,
            string password = null,
            AdtsUserAccountControl userAccountControl = AdtsUserAccountControl.ADS_UF_DONT_EXPIRE_PASSWD | AdtsUserAccountControl.ADS_UF_NORMAL_ACCOUNT)
        {
            if (username == null
                && password == null)
            {
                username = DomainAdmin;
                password = DomainAdminPassword;
            }

            string entryPath = string.Format("LDAP://{0}:{1}/{2}", serverName, serverPort, parentDN);
            DirectoryEntry de = new DirectoryEntry(entryPath, username, password);

            try
            {
                DirectoryEntry newUser = de.Children.Add(string.Format("CN={0}", newUserName), "user");
                newUser.InvokeSet("displayName", newUserName);
                newUser.InvokeSet("sAMAccountName", newUserName);
                newUser.CommitChanges();

                PrincipalContext ctx = new PrincipalContext(ContextType.Domain, serverName);
                UserPrincipal user = UserPrincipal.FindByIdentity(ctx, newUserName);
                user.SetPassword(newUserPassword);

                newUser.Properties["userAccountControl"].Value = (int)userAccountControl;
                newUser.CommitChanges();

                newUser.Close();
                de.Close();

                testObjects.Push(string.Format("add:{0}", newUser.Properties["distinguishedName"]));
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Set the account to user must change password.
        /// </summary>
        /// <param name="serverName">The server for connection.</param>
        /// <param name="serverPort">The port number for connection.</param>
        /// <param name="parentDN">The distinguished name of the entry.</param>
        /// <param name="newUserName">The name of the new user.</param>
        /// <returns>true for success, false for failed</returns>
        public static bool UserMustChangePassword(
            string serverName,
            string serverPort,
            string parentDN,
            string targetUser,
            string username = null,
            string password = null)
        {
            if (username == null
                && password == null)
            {
                username = DomainAdmin;
                password = DomainAdminPassword;
            }

            string entryPath = string.Format("LDAP://{0}:{1}/CN={2},{3}", serverName, serverPort, targetUser, parentDN);
            DirectoryEntry de = new DirectoryEntry(entryPath, username, password);

            try
            {
                de.InvokeSet("pwdLastSet", 0);
                de.CommitChanges();
                de.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Create an lds user account
        /// </summary>
        /// <param name="serverName">The server for connection.</param>
        /// <param name="serverPort">The port number for connection.</param>
        /// <param name="parentDN">The distinguished name of the entry.</param>
        /// <param name="newUserName">The name of the new user.</param>
        /// <param name="newUserPassword">The password of the new user.</param>
        /// <returns>true for success, false for failed</returns>
        public static bool NewLDSUser(
            string serverName,
            string serverPort,
            string parentDN,
            string newUserName,
            string newUserPassword,
            string username,
            string password)
        {

            string entryPath = string.Format("LDAP://{0}:{1}/{2}", serverName, serverPort, parentDN);
            DirectoryEntry de = new DirectoryEntry(entryPath, username, password);

            try
            {
                DirectoryEntry newUser = de.Children.Add(string.Format("CN={0}", newUserName), "user");
                newUser.InvokeSet("displayName", newUserName);
                newUser.InvokeSet("msDS-UserDontExpirePassword", true);
                newUser.CommitChanges();
                PrincipalContext ctx = new PrincipalContext(ContextType.ApplicationDirectory, serverName + ":" + serverPort, parentDN, username, password);
                UserPrincipal user = UserPrincipal.FindByIdentity(ctx, newUserName);
                user.SetPassword(newUserPassword);
                newUser.Properties["msDS-UserAccountDisabled"].Value = false;
                newUser.CommitChanges();
                newUser.Close();
                de.Close();
                testObjects.Push(string.Format("add:{0}", newUser.Properties["distinguishedName"]));
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Create a new group.
        /// </summary>
        /// <param name="serverName">The server for connection.</param>
        /// <param name="serverPort">The port number for connection.</param>
        /// <param name="parentDN">The distinguished name of the entry.</param>
        /// <param name="newGroupName">The name of the new group.</param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public static void NewGroup(
            string serverName,
            string serverPort,
            string parentDN,
            string newGroupName,
            string username = null,
            string password = null)
        {
            if (username == null
                && password == null)
            {
                username = DomainAdmin;
                password = DomainAdminPassword;
            }
            string entryPath = string.Format("LDAP://{0}:{1}/{2}", serverName, serverPort, parentDN);
            DirectoryEntry de = new DirectoryEntry(entryPath, username, password);
            DirectoryEntry newGroup = de.Children.Add(string.Format("CN={0}", newGroupName), "group");
            testObjects.Push(string.Format("add:{0}", newGroup.Properties["distinguishedName"]));
            newGroup.CommitChanges();
            newGroup.Close();
            de.Close();
        }

        /// <summary>
        /// Delete a group.
        /// </summary>
        /// <param name="serverName">The server for connection.</param>
        /// <param name="serverPort">The port number for connection.</param>
        /// <param name="parentDN">The distinguished name of the entry.</param>
        /// <param name="removeGroupName">The name of the group to be deleted.</param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public static void RemoveGroup(
            string serverName,
            string serverPort,
            string parentDN,
            string removeGroupName,
            string username = null,
            string password = null)
        {
            if (username == null
                && password == null)
            {
                username = DomainAdmin;
                password = DomainAdminPassword;
            }

            string entryPath = string.Format("LDAP://{0}:{1}/{2}", serverName, serverPort, parentDN);
            DirectoryEntry de = new DirectoryEntry(entryPath, username, password);

            DirectoryEntry removeGroup = de.Children.Find(string.Format("CN={0}", removeGroupName), "group");
            testObjects.Push(string.Format("delete:{0}", removeGroup.Properties["distinguishedName"]));
            removeGroup.DeleteTree();
            removeGroup.CommitChanges();
            removeGroup.Close();
            de.Close();
        }

        /// <summary>
        /// Create a computer account
        /// </summary>
        /// <param name="serverName">The server for connection.</param>
        /// <param name="serverPort">The port number for connection.</param>
        /// <param name="parentDN">The distinguished name of the entry.</param>
        /// <param name="newUserName">The name of the new user.</param>
        /// <param name="newUserPassword">The password of the new user.</param>
        /// <returns>true for success, false for failed</returns>
        public static bool NewComputer(
            string serverName,
            string serverPort,
            string parentDN,
            string newComputerName,
            string username = null,
            string password = null)
        {
            if (username == null
                && password == null)
            {
                username = DomainAdmin;
                password = DomainAdminPassword;
            }

            string entryPath = string.Format("LDAP://{0}:{1}/{2}", serverName, serverPort, parentDN);
            DirectoryEntry de = new DirectoryEntry(entryPath, username, password);
            addedObjectDNList.Add(string.Format("CN={0},{1}", newComputerName, parentDN));

            try
            {
                DirectoryEntry newComputer = de.Children.Add(string.Format("CN={0}", newComputerName), "computer");
                newComputer.InvokeSet("displayName", newComputerName);
                newComputer.InvokeSet("sAMAccountName", newComputerName);
                newComputer.CommitChanges();
                newComputer.Close();
                de.Close();

                testObjects.Push(string.Format("add:{0}", newComputer.Properties["distinguishedName"]));
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Remove a user account
        /// </summary>
        /// <param name="serverName">The server for connection.</param>
        /// <param name="serverPort">The port number for connection.</param>
        /// <param name="parentDN">The distinguished name of the entry.</param>
        /// <param name="removeUserName">The name of the new user.</param>
        /// <returns>true for success, false for failed</returns>
        public static bool RemoveUser(
            string serverName,
            string serverPort,
            string parentDN,
            string removeUserName,
            string username = null,
            string password = null)
        {
            if (username == null
                && password == null)
            {
                username = DomainAdmin;
                password = DomainAdminPassword;
            }

            string entryPath = string.Format("LDAP://{0}:{1}/{2}", serverName, serverPort, parentDN);
            DirectoryEntry de = new DirectoryEntry(entryPath, username, password);

            try
            {
                DirectoryEntry removeUser = de.Children.Find(string.Format("CN={0}", removeUserName), "user");
                removeUser.DeleteTree();
                removeUser.CommitChanges();
                removeUser.Close();
                de.Close();

                testObjects.Push(string.Format("delete:{0}", removeUser.Properties["distinguishedName"]));
                return true;
            }
            catch
            {
                return false;
            }
        }

        
        #endregion

        #region Delete(TombStone) Helpter Methods

        /// <summary>
        /// BuildDeleteEntry method Prepares Directory Entry for Displaying Tombstone objects
        /// </summary>
        /// <param name="serverName">The name of the server.</param>
        /// <param name="deletedObjectsGuid">Guid of the Deleted objects container(GUID_DELETED_OBJECTS_CONTAINER_W).</param>
        /// <param name="defaultNC">The name of the default naming context </param>
        /// <returns>Directory Entry </returns>
        public static DirectoryEntry BuildDeletedEntry(string serverName, string deletedObjectsGuid, string defaultNC)
        {
            DirectoryEntry buildDeletedEntry;
            string buildString = String.Format(CultureInfo.InvariantCulture,
                "LDAP://{0}<WKGUID={1},{2}>",
                serverName != null ? serverName + "/" : String.Empty,
                deletedObjectsGuid,
                defaultNC);
            buildDeletedEntry = new DirectoryEntry(
                buildString,
                null,
                null,
                AuthenticationTypes.FastBind
             );
            return buildDeletedEntry;
        }

        /// <summary>
        /// GetTombstone method Returns the information about the deleted object
        /// </summary>
        /// <param name="deletedEntry"> Entry Getting from the BuildDeletedEntry method</param>
        /// <param name="deletedObjectName">The name of the deleted object.</param>
        /// <returns>Search result value </returns>
        public static SearchResult GetTombstone(DirectoryEntry deletedEntry, string deletedObjectName)
        {
            DirectorySearcher ds = new DirectorySearcher(
                deletedEntry,
                string.Format(CultureInfo.InvariantCulture, "(&(isDeleted=TRUE)(name={0}*))", deletedObjectName),
                new string[] { "*" },
                System.DirectoryServices.SearchScope.OneLevel
                );

            ds.Tombstone = true;
            try
            {
                searchResult = ds.FindOne();
            }
            catch
            {
                return null;
            }

            return searchResult;
        }

        /// <summary>
        /// Get the deleted object directory entry.
        /// </summary>
        /// <param name="deletedObjectGuid">The Guid of the deleted object.</param>
        /// <param name="defaultNC">The default naming context.</param>
        /// <returns>Search result entry.</returns>
        public static SearchResult GetDeletedObject(
            string deletedObjectGuid,
            string defaultNC,
            string serverName,
            string serverPort,
            string username = null,
            string password = null)
        {
            if (username == null
                && password == null)
            {
                username = DomainAdmin;
                password = DomainAdminPassword;
            }

            string entryPath = string.Format("LDAP://{0}:{1}/CN=Deleted Objects,{2}", serverName, serverPort, defaultNC);
            DirectoryEntry de = new DirectoryEntry(entryPath, username, password);
            de.AuthenticationType = AuthenticationTypes.FastBind | AuthenticationTypes.Secure;
            
            DirectorySearcher ds = new DirectorySearcher(
                de,
                string.Format("(&(isDeleted=TRUE)(objectGuid={0}))",
                Guid2OctetString(new Guid(deletedObjectGuid))));
            ds.SearchScope = System.DirectoryServices.SearchScope.OneLevel;
            ds.Tombstone = true;
            try
            {
                searchResult = ds.FindOne();
            }
            catch
            {
                return null;
            }
            return searchResult;
        }

        /// <summary>
        /// IsObjectExist method Checks the specified Distinguished Name exists or not
        /// </summary>
        /// <param name="dn">The distinguished name of the object.</param>
        /// <returns>Presence of the Distinguished name </returns>
        public static bool IsObjectExist(
            string objectDN,
            string serverName,
            string serverPort,
            string username = null,
            string password = null)
        {
            //      Change AdLdapAdapter later to support multiple instances and 1 for test suite, 1 for utilities

            if (username == null
                && password == null)
            {
                username = DomainAdmin;
                password = DomainAdminPassword;
            }

            string entryPath = string.Format("LDAP://{0}:{1}/{2}", serverName, serverPort, objectDN);

            DirectoryEntry de = new DirectoryEntry(entryPath, username, password);

            try
            {
                var name = de.Name;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// HasChildren method Checks whether the specified dn contains the children or not
        /// </summary>
        /// <param name="dn">The distinguished name of the object.</param>
        /// <returns>True if it the object contains children otherwise false </returns> 
        public static bool HasChildren(string dn)
        {
            hasChild = false;
            DirectoryEntry parentEntry = new DirectoryEntry("LDAP://" + TargetServerFqdn + "/" + dn);
            foreach (DirectoryEntry childEntry in parentEntry.Children)
            {
                if (childEntry != null)
                {
                    hasChild = true;
                    break;
                }
            }
            return hasChild;

        }

        /// <summary>
        /// GetFsmo method Returns the FSMO Owner
        /// </summary>
        /// <param name="dn">The distinguished name of the FSMO Object</param>
        /// <returns> Fsmo Owner of an object</returns>
        public static string GetFsmo(string dn)
        {
            DirectoryEntry fsmoEntry = new DirectoryEntry("LDAP://" + TargetServerFqdn + "/" + dn);
            if (fsmoEntry != null)
            {
                string result = fsmoEntry.Properties["fsmoroleowner"].Value.ToString();
                fsmoEntry = new DirectoryEntry("LDAP://" + TargetServerFqdn + "/" + result);
                return fsmoEntry.Parent.Name.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// CreateNewSite method Creates Site in the Configuration NC
        /// </summary>
        /// <param name="siteName">The name of the Site.</param>
        /// <param name="topologyGeneratorDc">The name of the Domain Controller which will generate topology.</param>
        /// <returns> Nothing</returns>
        /// This is required to validate requirements based on this flag FLAG_DISALLOW_MOVE_ON_DELETE 
        /// Only when we create our own site this flag will be assigned.
        public static void CreateNewSite(string siteName, string topologyGeneratorDc)
        {
            // Set other settings, such as the inter-site topology generator
            // To do this, get context to an existing domain controller
            DirectoryContext dcContext = new DirectoryContext(DirectoryContextType.DirectoryServer, topologyGeneratorDc);
            //Set the site to the specify domain controller server
            ActiveDirectorySite site = new ActiveDirectorySite(dcContext, siteName);
            // Get a domain controller object
            System.DirectoryServices.ActiveDirectory.DomainController dc = 
                System.DirectoryServices.ActiveDirectory.DomainController.GetDomainController(dcContext);
            // Assign that object to the InterSiteTopologyGenerator property of the site.
            // This automatically adds the site name of the site containing the domain controller
            site.InterSiteTopologyGenerator = dc;
            //Commit the site to the directory
            site.Save();
        }

        #endregion

        #region Access Check Helper Methods

        #region Return AuthorizationRules

        /// <summary>
        /// GetPermission method returns AuthorizationRuleCollection of a particular user which means
        /// For which ActiveDirectory Rights the specified user is given permissions and for which he did not
        /// For which user groups he is a member and corresponding Rights and Allow/Deny access permissions
        /// </summary>
        /// <param name="objectDN">The distinguished name of the Container/Object.</param>
        /// <param name="domainUser">The name of the user to whom the permissions to be set</param>
        /// <param name="domain">The name of the domain to which user is belongs to </param>
        /// <returns> AuthorizationRuleCollection</returns>
        public static AuthorizationRuleCollection GetPermission(
            string objectDN,
            string domainUser,
            string domain,
            string serverName = null,
            string username = null,
            string password = null)
        {
            if (username == null
                && password == null)
            {
                username = DomainAdmin;
                password = DomainAdminPassword;
            }

            if (serverName == null)
            {
                serverName = TargetServerFqdn;
            }

            string entryPath = string.Format("LDAP://{0}/{1}", serverName, objectDN);
            AuthorizationRuleCollection rules = null;
            using (DirectoryEntry entry = new DirectoryEntry(
                entryPath,
                username,
                password,
                AuthenticationTypes.Secure))
            {
                ActiveDirectorySecurity objSec = entry.ObjectSecurity;
                string domainNetBIOSName = domain.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries)[0];
                NTAccount account = new NTAccount(domainNetBIOSName, domainUser);
                rules = objSec.GetAccessRules(true, true, account.GetType());
                entry.Close();
            }

            return rules;
        }

        #endregion

        #region IsAuthorizedOrNot

        /// <summary>
        /// GetPermission method returns AuthorizationRuleCollection of a particular user which means
        /// For which ActiveDirectory Rights the specified user is given permissions and for which he did not
        /// For which user groups he is a member and corresponding Rights and Allow/Deny access permissions
        /// </summary>
        /// <param name="objectDN">The distinguished name of the Container/Object.</param>
        /// <param name="domainUser">The name of the user to whom the permissions to be set</param>
        /// <param name="domain">The name of the domain to which user is belongs to </param>
        /// <param name="adRight">The name of the active directory right to which the user has permission or not </param>
        /// <returns> Returns True, if the user possesses specific access right and false if the user does not possesses 
        ///           specific access right  </returns>
        public static bool isAuthorizedOrNot(
            string objectDN,
            string domainUser,
            string domain,
            ActiveDirectoryRights adRight,
            string serverName = null,
            string username = null,
            string password = null)
        {
            if (username == null
                && password == null)
            {
                username = DomainAdmin;
                password = DomainAdminPassword;
            }

            if (serverName == null)
            {
                serverName = TargetServerFqdn;
            }

            AuthorizationRuleCollection rules = GetPermission(
                objectDN,
                domainUser,
                domain,
                serverName,
                username,
                password);
            bool isAuthorized = false;
            foreach (ActiveDirectoryAccessRule rule in rules)
            {
                if (rule.IdentityReference.ToString().Equals(
                    (domain.Split('.')[0].Trim() + "\\" + domainUser),
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    if (rule.ActiveDirectoryRights.ToString().Contains(adRight.ToString()))
                    {
                        if (rule.AccessControlType.Equals(AccessControlType.Allow))
                        {
                            isAuthorized = true;
                            return isAuthorized;
                        }
                        if (rule.AccessControlType.Equals(AccessControlType.Deny))
                        {
                            isAuthorized = false;
                            return isAuthorized;
                        }
                    }
                }
            }
            return isAuthorized;
        }

        /// <summary>
        /// GetPermission method returns AuthorizationRuleCollection of a particular user which means
        /// For which ActiveDirectory Rights the specified user is given permissions and for which he did not
        /// For which user groups he is a member and corresponding Rights and Allow/Deny access permissions
        /// </summary>
        /// <param name="objectDN">The distinguished name of the Container/Object.</param>
        /// <param name="domainUser">The name of the user to whom the permissions to be set</param>
        /// <param name="domain">The name of the domain to which user is belongs to </param>
        /// <param name="adRight">The name of the active directory right to which the user has permission or not </param>
        /// <param name="controlAccessRightGuid">The Guid of the control access right</param>
        /// <returns> Returns True,if the user possesses specific access right and false if the user does not possesses 
        ///           specific access right  </returns>
        public static bool isAuthorizedOrNotWithGuid(
            string objectDN,
            string domainUser,
            string domain,
            ActiveDirectoryRights adRight,
            Guid controlAccessRightGuid,
            string serverName = null,
            string username = null,
            string password = null)
        {
            if (username == null
                && password == null)
            {
                username = DomainAdmin;
                password = DomainAdminPassword;
            }

            if (serverName == null)
            {
                serverName = TargetServerFqdn;
            }

            AuthorizationRuleCollection rules = GetPermission(
                objectDN,
                domainUser,
                domain,
                serverName,
                username,
                password);
            bool isAuthorized = false;
            foreach (ActiveDirectoryAccessRule rule in rules)
            {
                if (rule.IdentityReference.ToString().Equals(
                    (domain.Split('.')[0].Trim() + "\\" + domainUser),
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    if (rule.ActiveDirectoryRights.ToString().Contains(adRight.ToString()) &&
                        rule.ObjectType.Equals(controlAccessRightGuid))
                    {
                        if (rule.AccessControlType.Equals(AccessControlType.Allow))
                        {
                            isAuthorized = true;
                            return isAuthorized;
                        }
                        if (rule.AccessControlType.Equals(AccessControlType.Deny))
                        {
                            isAuthorized = false;
                            return isAuthorized;
                        }
                    }
                }
            }
            return isAuthorized;
        }

        #endregion

        #region SetAccessRights

        /// <summary>
        /// SetAccessRights method sets the particular access right for a particular user on a particular 
        /// AD Container/Object
        /// </summary>
        /// <param name="objectDN">The distinguished name of the Container/Object.</param>
        /// <param name="domainUser">The name of the user to whom the permissions to be set</param>
        /// <param name="domain">The name of the domain to which user is belongs to </param>
        /// <param name="accessRight">The name of the access right to be set</param>
        /// <param name="controlType">Allow/Deny particular ActiveDirectoryRights</param>
        public static void SetAccessRights(
            string objectDN,
            string domainUser,
            string domain,
            ActiveDirectoryRights accessRight,
            AccessControlType controlType,
            string serverName = null,
            string username = null,
            string password = null)
        {
            if (username == null
                && password == null)
            {
                username = DomainAdmin;
                password = DomainAdminPassword;
            }

            if (serverName == null)
            {
                serverName = TargetServerFqdn;
            }

            string entryPath = string.Format("LDAP://{0}/{1}", serverName, objectDN);

            using (DirectoryEntry entry = new DirectoryEntry(
                entryPath,
                username,
                password,
                AuthenticationTypes.Secure))
            {
                ActiveDirectorySecurity objSec = entry.ObjectSecurity;
                string domainNetBIOSName = domain.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries)[0];
                NTAccount accountName = new NTAccount(domainNetBIOSName, domainUser);
                IdentityReference acctSID = accountName.Translate(typeof(SecurityIdentifier));
                ActiveDirectoryAccessRule myRule = new ActiveDirectoryAccessRule(
                    new SecurityIdentifier(acctSID.Value),
                    accessRight,
                    controlType);
                bool modified = false;
                while (!modified)
                {
                    objSec.ModifyAccessRule(AccessControlModification.Add, myRule, out modified);
                }
                entry.CommitChanges();
                entry.Close();
            }
        }

        #endregion

        #region RemoveAccessRights

        /// <summary>
        /// RemoveAccessRights method removes the particular access right for a particular user on a particular
        /// AD Container/Object
        /// </summary>
        /// <param name="objectDN">The distinguished name of the Container/Object.</param>
        /// <param name="domainUser">The name of the user to whom the permissions to be set</param>
        /// <param name="domain">The name of the domain to which user is belongs to </param>
        /// <param name="accessRight">The name of the access right to be removed </param>
        /// <param name="controlType">Allow/Deny particular ActiveDirectoryRights</param>
        /// <returns> Nothing</returns>
        public static void RemoveAccessRights(
            string objectDN,
            string domainUser,
            string domain,
            ActiveDirectoryRights accessRight,
            AccessControlType controlType,
            string serverName = null,
            string username = null,
            string password = null)
        {
            if (username == null
                && password == null)
            {
                username = DomainAdmin;
                password = DomainAdminPassword;
            }

            if (serverName == null)
            {
                serverName = TargetServerFqdn;
            }

            string entryPath = string.Format("LDAP://{0}/{1}", serverName, objectDN);

            using (DirectoryEntry entry = new DirectoryEntry(
                entryPath,
                username,
                password,
                AuthenticationTypes.Secure))
            {
                ActiveDirectorySecurity objSec = entry.ObjectSecurity;
                string[] domainNetBIOSName = domain.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                NTAccount accountName = new NTAccount(domainNetBIOSName[0], domainUser);
                IdentityReference acctSID = accountName.Translate(typeof(SecurityIdentifier));
                ActiveDirectoryAccessRule myRule = new ActiveDirectoryAccessRule(
                    new SecurityIdentifier(acctSID.Value),
                    accessRight,
                    controlType);
                bool modified = false;
                while (!modified)
                {
                    objSec.ModifyAccessRule(AccessControlModification.Remove, myRule, out modified);
                }
                entry.CommitChanges();
                entry.Close();
            }
        }

        #endregion

        #region SetControlAccessRights

        /// <summary>
        /// Sets requested control access rights for a specified user
        /// </summary>
        /// <param name="objectDN">Distinguished name of the object</param>
        /// <param name="domainUser">User on which the right is to be set</param>
        /// <param name="domain">Domain name</param>
        /// <param name="controlAccessRightGuid">GUID of the control access right</param>
        /// <param name="right">Active directory extended right</param>
        /// <param name="accessControl">Allow or deny a particular access right</param>
        public static void SetControlAcessRights(
            string objectDN,
            string domainUser,
            string domain,
            Guid controlAccessRightGuid,
            ActiveDirectoryRights right,
            AccessControlType accessControl,
            string serverName = null,
            string username = null,
            string password = null)
        {
            if (username == null
                && password == null)
            {
                username = DomainAdmin;
                password = DomainAdminPassword;
            }

            if (serverName == null)
            {
                serverName = TargetServerFqdn;
            }

            string entryPath = string.Format("LDAP://{0}/{1}", serverName, objectDN);

            using (DirectoryEntry entry = new DirectoryEntry(
                entryPath,
                username,
                password,
                AuthenticationTypes.Secure))
            {
                ActiveDirectorySecurity objSec = entry.ObjectSecurity;
                string domainNetBIOSName = domain.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries)[0];
                NTAccount accountName = new NTAccount(domainNetBIOSName, domainUser);
                IdentityReference acctSID = accountName.Translate(typeof(SecurityIdentifier));
                ActiveDirectoryAccessRule myRule = new ActiveDirectoryAccessRule(
                    new SecurityIdentifier(acctSID.Value),
                    right,
                    accessControl,
                    controlAccessRightGuid);
                bool modified = false;
                while (!modified)
                {
                    objSec.ModifyAccessRule(AccessControlModification.Add, myRule, out modified);
                }
                entry.CommitChanges();
                entry.Close();
            }
        }

        #endregion

        #region RemoveControlAccessRights

        /// <summary>
        /// Removes requested control access rights for a specified user
        /// </summary>
        /// <param name="objectDN">Distinguished name of the object</param>
        /// <param name="domainUser">User on which the right is to be removed</param>
        /// <param name="domain">Domain name</param>
        /// <param name="controlAccessRightGuid">GUID of the control access right</param>
        /// <param name="right">Active directory extended right</param>
        /// <param name="accessControl">Allow or deny a particular access right</param>
        public static void RemoveControlAcessRights(
            string objectDN,
            string domainUser,
            string domain,
            Guid controlAccessRightGuid,
            ActiveDirectoryRights right,
            AccessControlType accessControl,
            string serverName = null,
            string username = null,
            string password = null)
        {
            if (username == null
                && password == null)
            {
                username = DomainAdmin;
                password = DomainAdminPassword;
            }

            if (serverName == null)
            {
                serverName = TargetServerFqdn;
            }

            string entryPath = string.Format("LDAP://{0}/{1}", serverName, objectDN);

            using (DirectoryEntry entry = new DirectoryEntry(
                entryPath,
                username,
                password,
                AuthenticationTypes.Secure))
            {
                ActiveDirectorySecurity objSec = entry.ObjectSecurity;
                Guid controlAccessRight = new Guid(controlAccessRightGuid.ToByteArray());
                NTAccount accountName = new NTAccount(domain, domainUser);
                IdentityReference acctSID = accountName.Translate(typeof(SecurityIdentifier));
                ActiveDirectoryAccessRule myRule = new ActiveDirectoryAccessRule(
                    new SecurityIdentifier(acctSID.Value),
                    right,
                    accessControl,
                    controlAccessRight);
                bool modified = false;
                while (!modified)
                {
                    objSec.ModifyAccessRule(AccessControlModification.Remove, myRule, out modified);
                }
                entry.CommitChanges();
                entry.Close();
            }
        }

        #endregion

        #region DisableInheritance

        /// <summary>
        /// Disable inheritance
        /// </summary>
        /// <param name="serverName">The server for connection.</param>
        /// <param name="serverPort">The port number for connection.</param>
        /// <param name="objectDN">The distinguished name of the entry.</param>
        /// <param name="username">The username for authentication.</param>
        /// <param name="password">The user password for authentication.</param>
        public static void DisableInheritance(
            string serverName,
            string serverPort,
            string objectDN,
            string username = null,
            string password = null)
        {
            DirectoryEntry de = new DirectoryEntry(string.Format("LDAP://{0}:{1}/{2}", serverName, serverPort, objectDN), username, password);
            de.ObjectSecurity.SetAccessRuleProtection(true, true);
            de.CommitChanges();

            testObjects.Push(string.Format("disableInheritance:{0}", de.Properties["distinguishedName"]));
        }

        #endregion

        #endregion

        #region CheckingPorts

        /// <summary>
        /// CheckPorts method Returns Whether the DC supports given port and provider or not
        /// </summary>
        /// <param name="provider">Permissible values are LDAP and GC</param>
        /// <param name="serverName">Domain controller name</param>
        /// <param name="portNo">Fixed port number 3268</param>
        /// <returns>True if DC supports Provider and Port, false otherwise  </returns>
        public static bool CheckPorts(string provider, string serverName, string portNo)
        {
            string buildConnection = string.Format(CultureInfo.InvariantCulture, "{0}://{1}:{2}", provider, serverName, portNo);
            DirectoryEntry connection = new DirectoryEntry(buildConnection);
            if (connection != null)
                return true;
            else
                return false;
        }

        #endregion

        #region SdFlagsHelper Methods

        /// <summary>
        /// Returns OwnerPart of SecurityDescriptor
        /// </summary>
        /// <param name="securityDescriptor"></param>
        /// <returns></returns>
        /// Disable CA1011, it is require more derived type in this method 
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static string GetSecurityDescriptorOwner(ActiveDirectorySecurity securityDescriptor)
        {
            return securityDescriptor.GetOwner(typeof(NTAccount)).Value;
        }

        /// <summary>
        /// Returns Group Part of SecurityDescriptor
        /// </summary>
        /// <param name="securityDescriptor"></param>
        /// <returns></returns>
        /// Disable CA1011, it is require more derived type in this method 
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static string GetSecurityDescriptorGroup(ActiveDirectorySecurity securityDescriptor)
        {
            return securityDescriptor.GetGroup(typeof(NTAccount)).Value;
        }

        /// <summary>
        /// Returns DACL and SACL of SecurityDescriptor
        /// </summary>
        /// <param name="securityDescriptor"></param>
        /// <returns></returns>
        /// Disable CA1011, it is require more derived type in this method 
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static AuthorizationRuleCollection GetDaclSacl(ActiveDirectorySecurity securityDescriptor)
        {
            return securityDescriptor.GetAccessRules(true, true, typeof(NTAccount));
        }

        #endregion

        #region DirectorySyncChangesHelperMethods

        /// <summary>
        /// Returns Path of RootDomain Nc
        /// </summary>
        /// <returns> Path of Root Domain NC</returns>
        /// Disable CA1024, as this property is appropriate to use 
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public static string GetDefaultContext()
        {
            DirectoryEntry root = new DirectoryEntry(
                "LDAP://rootDSE",
                null,
                null,
                AuthenticationTypes.Secure
                );
            using (root)
            {
                string dnsHostName = root.Properties["dnsHostName"][0].ToString();
                string defaultNamingContext = root.Properties["defaultNamingContext"][0].ToString();
                return String.Format(CultureInfo.InvariantCulture,
                    "LDAP://{0}/{1}",
                    dnsHostName,
                    defaultNamingContext
                    );
            }
        }

        /// <summary>
        /// Stores the present state of an Active Directory in the form of cookie
        /// </summary>
        /// <param name="filter"> Any Valid Search Filter </param>
        /// <returns> Total Number Of Objects in AD before making any Changes</returns>
        public static int InitializeCookie(string filter)
        {
            int totalObjectsBeforeChange = 0;
            DirectoryEntry entry = new DirectoryEntry(
                    GetDefaultContext(),
                    null,
                    null,
                    AuthenticationTypes.None
                    );
            using (entry)
            {
                DirectorySearcher ds = new DirectorySearcher(
                        entry,
                        filter,
                        null
                        );
                //We must use Subtree scope
                ds.SearchScope = System.DirectoryServices.SearchScope.Subtree;
                //Pass in the flags we wish here
                DirectorySynchronization dSynch = new DirectorySynchronization(
                        System.DirectoryServices.DirectorySynchronizationOptions.None
                        );
                ds.DirectorySynchronization = dSynch;
                using (SearchResultCollection src = ds.FindAll())
                {
                    totalObjectsBeforeChange = src.Count;
                    //Get and store the cookie
                    StoreCookie(dSynch.GetDirectorySynchronizationCookie());
                }
            }
            return totalObjectsBeforeChange;
        }

        /// <summary>
        /// Identifies the Changes made to AD by considering latest cookie information.
        /// </summary>
        /// <param name="filter">Any Valid Search Filter</param>
        /// <param name="saveState">True means stores information in the form of cookie,
        ///                         False means won't store information</param>
        /// <returns>Returns SearchResultEntries(those objects which are added or modified or deleted)</returns>
        public static SearchResultCollection GetSynchedChanges(string filter, bool saveState)
        {
            //This is our searchroot
            string dnname = GetDefaultContext();
            DirectoryEntry entry = new DirectoryEntry(
                    dnname,
                    null,
                    null,
                    AuthenticationTypes.None
                    );
            string[] attribs = null;
            DirectorySearcher searcher = new DirectorySearcher(
                    entry,
                    filter,
                    attribs
                    );
            //We must use Subtree scope
            searcher.SearchScope = System.DirectoryServices.SearchScope.Subtree;
            //Pass back in our saved cookie
            byte[] restoreCookie = RestoreCookie();
            DirectorySynchronization dSynch = new DirectorySynchronization(
                    System.DirectoryServices.DirectorySynchronizationOptions.None,
                    restoreCookie
                   );
            searcher.DirectorySynchronization = dSynch;
            if (saveState)
            {
                //Get and store the cookie again
                StoreCookie(dSynch.GetDirectorySynchronizationCookie());
            }
            return searcher.FindAll();
        }

        /// <summary>
        /// Stores present state of AD in the form of cookie
        /// </summary>
        /// <param name="cookieBytes">Byte array which stores cookie information</param>
        public static void StoreCookie(byte[] cookieBytes)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fs = new FileStream(
                    "..\\..\\cookie.bin",
                    FileMode.Create
                    );
            using (fs)
            {
                formatter.Serialize(fs, cookieBytes);
            }
        }

        /// <summary>
        /// Returns the cookie information
        /// </summary>
        /// <returns> Byte array which contains cookie information.</returns>
        public static byte[] RestoreCookie()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fs = new FileStream(
                    "..\\..\\cookie.bin",
                    FileMode.Open
                    );
            using (fs)
            {
                return (byte[])formatter.Deserialize(fs);
            }
        }

        #endregion

        /// <summary>
        /// GetAttributeFromEntry method Returns the Attribute Value of an object
        /// </summary>
        /// <param name="objectDN">The Distinguishedname of the object.</param>
        /// <param name="attributeName">The AttributeName of the object.</param>
        /// <returns> Attribute Value of an object</returns>
        public static object GetAttributeFromEntry(
            string objectDN,
            string attributeName,
            string serverName,
            string serverPort,
            string username = null,
            string password = null)
        {
            if (username == null
                && password == null)
            {
                username = DomainAdmin;
                password = DomainAdminPassword;
            }

            string entryPath = string.Format("LDAP://{0}:{1}/{2}", serverName, serverPort, objectDN);

            DirectoryEntry entry = new DirectoryEntry(entryPath, username, password);

            try
            {
                var name = entry.Name;
                entry.RefreshCache(new string[] { attributeName });
                return entry.Properties[attributeName].Value;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// GetAttributeFromEntry method Returns the Attribute Value of an object
        /// </summary>
        /// <param name="objectDN">The Distinguishedname of the object.</param>
        /// <param name="attributeName">The AttributeName of the object.</param>
        /// <returns> Attribute Value of an object</returns>
        public static Dictionary<string, object> GetAttributesFromEntry(
            string objectDN,
            string[] attributeNames,
            string serverName,
            string serverPort,
            string username = null,
            string password = null)
        {
            if (username == null && password == null)
            {
                username = DomainAdmin;
                password = DomainAdminPassword;
            }

            string entryPath = string.Format("LDAP://{0}:{1}/{2}", serverName, serverPort, objectDN);

            Dictionary<string, object> attributes = new Dictionary<string, object>();
            using (DirectoryEntry entry = new DirectoryEntry(entryPath, username, password))
            {
                var name = entry.Name;
                entry.RefreshCache(attributeNames);
                foreach (var s in attributeNames)
                {
                    attributes.Add(s, entry.Properties[s].Value);
                }
            }
            return attributes;
        }


        /// <summary>
        /// GetObjectGUIDFromEntry method Returns the objectGUID of an object
        /// </summary>
        /// <param name="dn">The Distinguishedname of the object.</param>
        /// <returns> objectGUID of an object</returns>
        public static object GetObjectGUIDFromEntry(
            string objectDN,
            string serverName,
            string serverPort,
            string username = null,
            string password = null)
        {
            return GetAttributeFromEntry(
                objectDN,
                "objectGUID",
                serverName,
                serverPort,
                username,
                password);
        }

        /// <summary>
        /// GetRDNFromEntry method Returns the rdnType of an object
        /// </summary>
        /// <param name="dn">The Distinguishedname of the object.</param>
        /// <returns> rdnType of an object</returns>
        public static string GetRDNFromEntry(
            string objectDN,
            string serverName,
            string serverPort,
            string username = null,
            string password = null)
        {
            string distinguishedName = GetAttributeFromEntry(
                objectDN,
                "distinguishedName",
                serverName,
                serverPort,
                username,
                password).ToString();

            if (distinguishedName != null)
            {
                return distinguishedName.Split(',')[0].Split('=')[1];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// GetParentFromEntry method Returns the parent of an object
        /// </summary>
        /// <param name="dn">The Distinguishedname of the object.</param>
        /// <returns> Parent of an object</returns>
        public static object GetParentFromEntry(
            string objectDN,
            string serverName,
            string serverPort,
            string username = null,
            string password = null)
        {
            if (username == null
                && password == null)
            {
                username = DomainAdmin;
                password = DomainAdminPassword;
            }

            string entryPath = string.Format("LDAP://{0}:{1}/{2}", serverName, serverPort, objectDN);
            
            DirectoryEntry entry = new DirectoryEntry(objectDN, username, password);
            try
            {
                var name = entry.Name;
                return entry.Parent;
            }
            catch
            {
                return null;
            }
        }

        public static string ParseDN(string domainDnsName)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string seg in domainDnsName.Split('.'))
                sb.AppendFormat(",DC={0}", seg);
            return sb.ToString().Trim(',');
        }

        public static void CreateNewSite(
            string dcName,
            string siteName,
            string domainDNSName,
            string username,
            string password)
        {
            string dn = ParseDN(domainDNSName);
            string siteObject = string.Format(
                "CN={0},CN=Sites,CN=Configuration,{1}",
                siteName,
                dn);

            string siteContainer = "CN=Sites,CN=Configuration," + dn;

            DirectoryEntry siteContainerEntry = new DirectoryEntry(
                string.Format("LDAP://{0}/{1}", dcName, siteContainer),
                username,
                password,
                AuthenticationTypes.Secure);

            siteContainerEntry.Children
                .Add("CN=" + siteName, "site")
                .CommitChanges();

            string newSiteDn = string.Format("CN={0},{1}", siteName, siteContainer);

            new DirectoryEntry(
                string.Format("LDAP://{0}/{1}", dcName, newSiteDn),
                username,
                password,
                AuthenticationTypes.Secure).Children
                .Add("CN=NTDS Site Settings", "nTDSSiteSettings")
                .CommitChanges();

            new DirectoryEntry(
                string.Format("LDAP://{0}/{1}", dcName, newSiteDn),
                username,
                password,
                AuthenticationTypes.Secure).Children
                .Add("CN=Servers", "serversContainer")
                .CommitChanges();

            string siteLinkDN = "CN=DEFAULTIPSITELINK,CN=IP,CN=Inter-Site Transports,CN=Sites,CN=Configuration," + dn;

            DirectoryEntry siteLinkEntry = new DirectoryEntry(
                string.Format("LDAP://{0}/{1}", dcName, siteLinkDN),
                username,
                password,
                AuthenticationTypes.Secure);
            siteLinkEntry.Properties["siteList"].Add(newSiteDn);
            siteLinkEntry.CommitChanges();
        }

        public static string[] SearchSites(
            string dcName,
            int port,
            string domainDNSName,
            string username,
            string password)
        {
            string dn = ParseDN(domainDNSName);
            DirectoryEntry siteContainerEntry = new DirectoryEntry(
                string.Format("CN=Sites,CN=Configuration,{0}",  dn),
                username,
                password,
                AuthenticationTypes.Secure);
            NetworkCredential credential = new System.Net.NetworkCredential(username, password, domainDNSName);
            LdapConnection conn = new LdapConnection(
                        new LdapDirectoryIdentifier(dcName, port),
                        credential);
            conn.Bind();
            SearchRequest sr = new SearchRequest(
                 string.Format("CN=Sites,CN=Configuration,{0}", dn),
                 "(objectclass=site)",
                 System.DirectoryServices.Protocols.SearchScope.OneLevel,
                 "name");
            SearchResponse mrep = conn.SendRequest(sr) as SearchResponse;
            string[] results = new string[mrep.Entries.Count];
            for (int i = 0; i < results.Length; i++)
                results[i] = mrep.Entries[i].Attributes["name"].GetValues(typeof(string))[0] as string;
            return results;
        }

        /// <summary>
        /// Get user SID
        /// </summary>
        /// <param name="dcName">DC name</param>
        /// <param name="domainName">Domain name</param>
        /// <param name="userName">User name</param>
        /// <param name="userPassword">User password</param>
        /// <param name="user">User whose sid is to be gotten</param>
        /// <returns>User sid</returns>
        public static string GetUserSid(
            string dcName,
            string domainName,
            string userName,
            string userPassword,
            string user)
        {
            if (dcName == null || domainName == null || userName == null || userPassword == null || user == null)
            {
                return null;
            }
            // Path format : LDAP://server.contoso.com/CN=administrator,CN=Users,DC=contoso,DC=com
            string path = "LDAP://" + dcName + "." + domainName + "/CN=" + user + ",CN=Users";
            string[] domains = domainName.Split('.');
            for (int i = 0; i < domains.Length; ++i)
            {
                path += (",DC=" + domains[i]);
            }
            DirectoryEntry entry = new DirectoryEntry(path, userName, userPassword);
            byte[] sid = entry.Properties["objectSid"].Value as byte[];
            entry.Close();
            return ConvertSidToStringSid(sid);
        }

        /// <summary>
        /// ConvertSidToStringSid
        /// </summary>
        /// <param name="sid">Sid byte array</param>
        /// <returns>Sid string representation</returns>
        public static string ConvertSidToStringSid(byte[] sid)
        {
            string ret = null;
            IntPtr pSid = Marshal.AllocHGlobal(sid.Length);
            Marshal.Copy(sid, 0, pSid, sid.Length);
            IntPtr pRet = IntPtr.Zero;
            int hr = ConvertSidToStringSidW(pSid, ref pRet);
            Marshal.FreeHGlobal(pSid);
            if (hr != 0)
            {
                try
                {
                    ret = Marshal.PtrToStringUni(pRet);
                }
                finally
                {
                    Marshal.FreeHGlobal(pRet);
                }
            }
            return ret;
        }

        /// <summary>
        /// ConvertSidToStringSidW
        /// </summary>
        /// <param name="pSid">IntPtr to sid byte array</param>
        /// <param name="strSid">IntPtr to sid string representation</param>
        /// <returns>A int zero means failure and non-zero means success</returns>
        /// Disable CA1060, because according to current test suite design, there is no need to move pinvoke to native
        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport("Advapi32.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        private extern static int ConvertSidToStringSidW(IntPtr pSid, ref IntPtr strSid);

        /// <summary>
        /// Get user GUID
        /// </summary>
        /// <param name="dcName">DC name</param>
        /// <param name="domainName">Domain name</param>
        /// <param name="userName">User name</param>
        /// <param name="userPassword">User password</param>
        /// <param name="user">User whose Guid is to be gotten</param>
        /// <returns>User Guid string representation</returns>
        public static string GetUserGuid(
            string dcName,
            string domainName,
            string serverPort,
            string userName,
            string userPassword,
            string user)
        {
            string objectDN = "CN=" + user + ",CN=Users,DC=" + domainName.Replace(".", ",DC=");

            byte[] GUID = (byte[])GetObjectGUIDFromEntry(
                objectDN,
                dcName + "." + domainName,
                serverPort,
                userName,
                userPassword);

            return new Guid(GUID).ToString();
        }

        /// <summary>
        /// Check if groupMemberDN is in the groupDN membership
        /// </summary>
        /// <param name="groupDN">group dn</param>
        /// <param name="groupMemberDN">group member dn</param>
        /// <returns>true for yes; false for no</returns>
        public static bool IsGroupMember(
            string dcName,
            string fullDomainName,
            string userName,
            string userPassword,
            string groupDN,
            string groupMemberDN)
        {
            if (dcName == null || fullDomainName == null || userName == null || userPassword == null)
            {
                return false;
            }

            string groupPath = string.Format("LDAP://{0}/{1}", dcName + "." + fullDomainName, groupDN);
            string groupMemberPath = string.Format("LDAP://{0}/{1}", dcName + "." + fullDomainName, groupMemberDN);

            DirectoryEntry groupEntry = new DirectoryEntry(groupPath, userName, userPassword, AuthenticationTypes.Secure);;
            DirectoryEntry groupMemberEntry = new DirectoryEntry(groupMemberPath, userName, userPassword, AuthenticationTypes.Secure);

            try
            {
                bool isMember = false;
                if (groupEntry.Properties["member"].Contains(groupMemberEntry.Properties["distinguishedName"].Value))
                {
                    isMember = true;
                }
                else
                {
                    isMember = false;
                }
                groupMemberEntry.Close();
                groupEntry.Close();

                return isMember;
            }
            catch
            {
                groupMemberEntry.Close();
                groupEntry.Close();

                return false;
            }
        }

        /// <summary>
        /// add a group member to the LDS group
        /// </summary>
        /// <param name="dcName">DC name</param>
        /// <param name="portnum">The port number for connection</param>
        /// <param name="userName">user name</param>
        /// <param name="userPassword">user password</param>
        /// <param name="groupDN">group distinguished name</param>
        /// <param name="groupMemberDN">group member distinguished name</param>
        public static bool AddLDSGroupMember(
            string dcName,
            string portnum,
            string userName,
            string userPassword,
            string groupDN,
            string groupMemberDN)
        {
            if (dcName == null || groupDN == null || groupMemberDN == null)
            {
                return false;
            }

            string groupPath = string.Format("LDAP://{0}:{1}/{2}", dcName, portnum, groupDN);
            string groupMemberPath = string.Format("LDAP://{0}:{1}/{2}", dcName, portnum, groupMemberDN);

            DirectoryEntry groupEntry = new DirectoryEntry(groupPath, userName, userPassword, AuthenticationTypes.Secure); ;
            DirectoryEntry groupMemberEntry = new DirectoryEntry(groupMemberPath, userName, userPassword, AuthenticationTypes.Secure);

            try
            {
                groupEntry.Properties["member"].Add(groupMemberEntry.Properties["distinguishedName"].Value);
                groupEntry.CommitChanges();
                groupMemberEntry.Close();
                groupEntry.Close();

                testObjects.Push(string.Format("add group member:{0}->{1}", groupMemberDN, groupDN));
                return true;
            }
            catch
            {
                groupMemberEntry.Close();
                groupEntry.Close();

                return false;
            }
        }   

        /// <summary>
        /// add a group member to the group
        /// </summary>
        /// <param name="dcName">DC name</param>
        /// <param name="userName">user name</param>
        /// <param name="userPassword">user password</param>
        /// <param name="groupDN">group distinguished name</param>
        /// <param name="groupMemberDN">group member distinguished name</param>
        public static bool AddGroupMember(
            string dcName,
            string userName,
            string userPassword,
            string groupDN,
            string groupMemberDN)
        {
            if (dcName == null || groupDN == null || groupMemberDN == null)
            {
                return false;
            }

            string groupPath = string.Format("LDAP://{0}/{1}", dcName, groupDN);
            string groupMemberPath = string.Format("LDAP://{0}/{1}", dcName, groupMemberDN);

            DirectoryEntry groupEntry = new DirectoryEntry(groupPath, userName, userPassword, AuthenticationTypes.Secure); ;
            DirectoryEntry groupMemberEntry = new DirectoryEntry(groupMemberPath, userName, userPassword, AuthenticationTypes.Secure);

            try
            {
                groupEntry.Properties["member"].Add(groupMemberEntry.Properties["distinguishedName"].Value);
                groupEntry.CommitChanges();
                groupMemberEntry.Close();
                groupEntry.Close();

                testObjects.Push(string.Format("add group member:{0}->{1}", groupMemberDN, groupDN));
                return true;
            }
            catch
            {
                groupMemberEntry.Close();
                groupEntry.Close();

                return false;
            }
        }   

        /// <summary>
        /// remove a group member
        /// </summary>
        /// <param name="dcName">DC name</param>
        /// <param name="userName">user name</param>
        /// <param name="userPassword">user password</param>
        /// <param name="groupDN">group distinguished name</param>
        /// <param name="groupMemberDN">group member distinguished name</param>
        public static bool RemoveGroupMember(
            string dcName,
            string userName,
            string userPassword,
            string groupDN,
            string groupMemberDN)
        {
            if (dcName == null || groupDN == null || groupMemberDN == null)
            {
                return false;
            }

            string groupPath = string.Format("LDAP://{0}/{1}", dcName, groupDN);
            string groupMemberPath = string.Format("LDAP://{0}/{1}", dcName, groupMemberDN);

            DirectoryEntry groupEntry = new DirectoryEntry(groupPath, userName, userPassword, AuthenticationTypes.Secure); ;
            DirectoryEntry groupMemberEntry = new DirectoryEntry(groupMemberPath, userName, userPassword, AuthenticationTypes.Secure);

            try
            {
                groupEntry.Properties["member"].Remove(groupMemberEntry.Properties["distinguishedName"].Value);
                groupEntry.CommitChanges();
                groupMemberEntry.Close();
                groupEntry.Close();

                testObjects.Push(string.Format("remove group member:{0}->{1}", groupMemberDN, groupDN));
                return true;
            }
            catch
            {
                groupMemberEntry.Close();
                groupEntry.Close();

                return false;
            }
        }

        /// <summary>
        /// Check whether an attribute is reserved
        /// </summary>
        /// <param name="searchFlags">The name of the special attribute - LDAP display name</param>
        /// <returns>Whether the reserved flag is set in the special attribute</returns>
        public static bool IsAttributeReserved(int searchFlags)
        {
            bool retValue = false;
            // From TD, we could see 0x08 indicates PR bit is set to 1.
            if ((searchFlags &= 0x08) == 0x08)
            {
                retValue = true;
            }
            return retValue;
        }

        /// <summary>
        /// Get deleted-object DN
        /// </summary>
        /// <param name="distinguishedName">The distinguished name of the object</param>
        /// <param name="deletedObjectContainer">The deleted-object container</param>
        public static string GetDeletedObjectDN(
            string distinguishedName,
            string deletedObjectContainer,
            string serverName,
            string serverPort,
            string username = null,
            string password = null)
        {
            if (username == null
                && password == null)
            {
                username = DomainAdmin;
                password = DomainAdminPassword;
            }

            byte[] guidInBytes = (byte[])GetObjectGUIDFromEntry(
                distinguishedName,
                serverName,
                serverPort,
                username,
                password);
            Guid guid = new Guid(guidInBytes);
            string objCN = distinguishedName.Split(',')[0].Trim().ToString();
            return objCN + "\\0ADEL:" + guid.ToString() + "," + deletedObjectContainer;
        }

        /// <summary>
        /// Get search flags from schema attribute
        /// </summary>
        /// <param name="schemaNCPath">Indicates schema path</param>
        /// <param name="attributeName">The name of the attribute</param>
        /// <returns>Return system flag from schema attribute</returns>
        public static int GetSearchFlagsFromSchemaAttribute(
            string schemaNCPath,
            string attributeName)
        {
            ICollection<AdtsSearchResultEntryPacket> searchResults;
            string[] searchAttrVals;

            string ret = AdLdapClient.Instance().SearchObject(
                schemaNCPath,
                System.DirectoryServices.Protocols.SearchScope.Subtree,
                string.Format("(ldapDisplayName={0})", attributeName),
                new string[] { "searchFlags" },
                null,
                out searchResults);

            if (ret.Contains(Enum.GetName(typeof(ResultCode), ResultCode.Success))
                && searchResults.Count >= 1)
            {
                foreach (AdtsSearchResultEntryPacket entrypacket in searchResults)
                {
                    searchAttrVals = AdLdapClient.Instance().GetAttributeValuesInString(entrypacket, "searchFlags");
                    return Convert.ToInt32(searchAttrVals[0]);
                }
            }

            return -1;
        }

        /// <summary>
        /// Get Entries Counts
        /// </summary>
        /// <param name="serverName">The server for connection.</param>
        /// <param name="serverPort">The port number for connection.</param>
        /// <param name="parentDN">The distinguished name of the container.</param>
        /// <param name="objectClass">The objectClass that is been counted.</param>
        /// <param name="searchPageSize">search page size</param>
        /// <returns></returns>
        public static int GetEntriesCount(
            string serverName,
            string serverPort,
            string parentDN,
            string objectClass,
            string username = null,
            string password = null,
            int searchPageSize = 1500)
        {
            if (username == null
                && password == null)
            {
                username = DomainAdmin;
                password = DomainAdminPassword;
            }

            string entryPath = string.Format("LDAP://{0}:{1}/{2}", serverName, serverPort, parentDN);
            DirectoryEntry de = new DirectoryEntry(entryPath, username, password);
            DirectorySearcher searcher = new DirectorySearcher(de, string.Format("(objectClass={0})", objectClass));
            searcher.PageSize = searchPageSize;
            int count = searcher.FindAll().Count;
            de.Close();
            
            return count;
        }

        public static string DomainDnsNameToDN(string dnsName)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string seg in dnsName.Split('.'))
            {
                sb.AppendFormat(",DC={0}", seg);
            }
            return sb.ToString().Trim(',');
        }

        public static string SecurityDescriptorBackupFilename = @"c:\SecurityDescriptor.xml";

        /// <summary>
        /// Backups or restores the nTSecurityDescriptor of the object.
        /// If the nTSecurityDescriptor has been backuped, restores it.
        /// If the nTSecurityDescriptor is never backuped, backups it.
        /// </summary>
        /// <param name="serverName">Server</param>
        /// <param name="port">Port number</param>
        /// <param name="objectDN">The DN of the object.</param>
        /// <param name="filename">The file name to back up the security descriptor</param>
        public static void BackupOrRestoreNtSecurityDescriptor(
            string serverName,
            int port,
            string objectDN,
            string filename,
            NetworkCredential credential)
        {
            List<NtSecurityDescriptorItem> items = new List<NtSecurityDescriptorItem>();
            NtSecurityDescriptorItem currentObject = null;
            if (File.Exists(filename))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<NtSecurityDescriptorItem>));
                using (XmlTextReader reader = new XmlTextReader(filename))
                {
                    reader.XmlResolver = null;
                    reader.DtdProcessing = DtdProcessing.Prohibit;
                    items = (List<NtSecurityDescriptorItem>)serializer.Deserialize(reader);
                }
                for (var i = items.GetEnumerator(); currentObject == null && i.MoveNext(); )
                    if (i.Current.ObjectDN == objectDN) currentObject = i.Current;
                if (currentObject != null)
                {
                    LdapConnection conn = new LdapConnection(
                        new LdapDirectoryIdentifier(serverName, port),
                        credential);
                    conn.Bind();
                    ModifyRequest mr = new ModifyRequest(
                         objectDN,
                         DirectoryAttributeOperation.Replace,
                         "nTSecurityDescriptor",
                         currentObject.SecurityDescriptor);
                    var mrep = conn.SendRequest(mr);
                    return;
                }
            }
            LdapConnection conn1 = new LdapConnection(
                new LdapDirectoryIdentifier(serverName, port),
                credential);
            conn1.Bind();
            SearchRequest sr = new SearchRequest(
                objectDN,
                "(objectclass=*)",
                System.DirectoryServices.Protocols.SearchScope.Base, "nTSecurityDescriptor");
            var rep = conn1.SendRequest(sr) as SearchResponse;
            if (rep != null)
            {
                items.Add(new NtSecurityDescriptorItem()
                {
                    ObjectDN = objectDN,
                    SecurityDescriptor = rep.Entries[0].Attributes["nTSecurityDescriptor"][0] as byte[]
                });
                XmlSerializer serializer = new XmlSerializer(typeof(List<NtSecurityDescriptorItem>));
                using (FileStream fs = new FileStream(filename, FileMode.Create))
                {
                    serializer.Serialize(fs, items);
                    fs.Close();
                }
            }
            else
            {
                throw new InvalidOperationException("Cannot backup the ntSecurityDescriptor of object " + objectDN);
            }

        }
    }
    public class NtSecurityDescriptorItem
    {
        public string ObjectDN { get; set; }
        public byte[] SecurityDescriptor;
    }
    /// <summary>
    /// Provides a method to retrieve enumeration types for representing symbolic values of certain attributes.
    /// </summary>
    public static class IntegerSymbols
    {
        /// <summary>
        /// Gets an enumeration type which provide symbols 
        /// Used in the TD to represent certain numeric values for attributes.
        /// </summary>
        /// <param name="attrName"> Any of the Flag type like system | schema | instance | search </param>
        /// <returns>Type of the Flag </returns>
        public static Type GetSymbolEnumType(string attrName)
        {
            switch (attrName.ToLower(CultureInfo.InvariantCulture))
            {
                case "schemaflagsex":
                    return typeof(SchemaFlagsEx);
                case "searchflags":
                    return typeof(SearchFlags);
                case "systemflags":
                    return typeof(SystemFlags);
                case "instanceflags":
                    return typeof(InstanceTypeFlags);
                case "objectclasscategory":
                    return typeof(ObjectClassCategory);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Returns the actual FLAG name when type and Flag value is given as an input
        /// </summary>
        /// <param name="type"> Type of the Flag : System | Search | Schema | Instance </param>
        /// <param name="value">The value of the Flag </param>
        /// <returns>FLAG Name </returns>
        public static string UnparseUInt32Enum(Type type, uint value)
        {
            return Enum.Format(type, value, "g");
        }

        /// <summary>
        /// Returns Flag value when Flag name is given as an input
        /// </summary>
        /// <param name="type"> Type of the Flag : System | Search | Schema | Instance </param>
        /// <param name="strFlag">The name of the Flag </param>
        /// <returns>FLAG Value </returns>
        public static Int32 ParseSystemFlagsValue(string type, string strFlag)
        {
            Type flagType = IntegerSymbols.GetSymbolEnumType(type);
            uint result;
            if (strFlag.Contains("|"))
            {
                strFlag = strFlag.Replace('|', ',');
            }
            result = (uint)Enum.Parse(flagType, strFlag, true);
            return (int)result;
        }

    }

    /// <summary>
    /// Symbols used in representing values of systemFlags attribute.
    /// </summary>
    /// Disable warning CA1028 because System.Int32 cannot match the enumeration design according to actual Model logic.
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum SystemFlags : uint
    {
        /// <summary>
        /// FLAG_ATTR_NOT_REPLICATED
        /// </summary>
        FLAG_CR_NTDS_NC = 0x1u,

        /// <summary>
        /// FLAG_ATTR_REQ_PARTIAL_SET_MEMBER
        /// </summary>
        FLAG_CR_NTDS_DOMAIN = 0x1u << 1,

        /// <summary>
        /// FLAG_ATTR_IS_CONSTRUCTED
        /// </summary>
        FLAG_CR_NTDS_NOT_GC_REPLICATED = 0x1u << 2,

        /// <summary>
        /// FLAG_ATTR_IS_OPERATIONAL
        /// </summary>
        FLAG_ATTR_IS_OPERATIONAL = 0x1u << 3,

        /// <summary>
        /// FLAG_SCHEMA_BASE_OBJECT
        /// </summary>
        FLAG_SCHEMA_BASE_OBJECT = 0x1u << 4,

        /// <summary>
        /// FLAG_ATTR_IS_RDN
        /// </summary>
        FLAG_ATTR_IS_RDN = 0x1u << 5,

        /// <summary>
        /// FLAG_DISALLOW_MOVE_ON_DELETE
        /// </summary>
        FLAG_DISALLOW_MOVE_ON_DELETE = 0x1u << 25,

        /// <summary>
        /// FLAG_DOMAIN_DISALLOW_MOVE
        /// </summary>
        FLAG_DOMAIN_DISALLOW_MOVE = 0x1u << 26,

        /// <summary>
        /// FLAG_DOMAIN_DISALLOW_RENAME
        /// </summary>
        FLAG_DOMAIN_DISALLOW_RENAME = 0x1u << 27,

        /// <summary>
        /// FLAG_CONFIG_ALLOW_LIMITED_MOVE
        /// </summary>
        FLAG_CONFIG_ALLOW_LIMITED_MOVE = 0x1u << 28,

        /// <summary>
        /// FLAG_CONFIG_ALLOW_MOVE
        /// </summary>
        FLAG_CONFIG_ALLOW_MOVE = 0x1u << 29,

        /// <summary>
        /// FLAG_CONFIG_ALLOW_RENAME
        /// </summary>
        FLAG_CONFIG_ALLOW_RENAME = 0x1u << 30,

        /// <summary>
        /// FLAG_DISALLOW_DELETE
        /// </summary>
        FLAG_DISALLOW_DELETE = 0x1u << 31,
    }

    /// <summary>
    /// Symbols used in representing values of schemaExFlags attribute.
    /// </summary>
    /// Disable warning CA1028 because System.Int32 cannot match the enumeration design according to actual Model logic.
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum SchemaFlagsEx : uint
    {
        /// <summary>
        /// FLAG_ATTR_IS_CRITICAL
        /// </summary>
        FLAG_ATTR_IS_CRITICAL = 0x1u,
    }

    /// <summary>
    /// Symbols used in representing values of searchFlags attribute.
    /// </summary>
    /// Disable warning CA1028 because System.Int32 cannot match the enumeration design according to actual Model logic.
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum SearchFlags : uint
    {
        /// <summary>
        /// fATTINDEX
        /// </summary>
        fATTINDEX = 0x1u,

        /// <summary>
        /// fPDNTATTINDEX
        /// </summary>
        fPDNTATTINDEX = 0x1u << 1,

        /// <summary>
        /// fANR
        /// </summary>
        fANR = 0x1u << 2,

        /// <summary>
        /// fPRESERVEONDELETE
        /// </summary>
        fPRESERVEONDELETE = 0x1u << 3,

        /// <summary>
        /// fCOPY
        /// </summary>
        fCOPY = 0x1u << 4,

        /// <summary>
        /// fTUPLEINDEX
        /// </summary>
        fTUPLEINDEX = 0x1u << 5,

        /// <summary>
        /// fSUBTREEATTINDEX
        /// </summary>
        fSUBTREEATTINDEX = 0x1u << 6,

        /// <summary>
        /// fCONFIDENTIAL
        /// </summary>
        fCONFIDENTIAL = 0x1u << 7,

        /// <summary>
        /// fCONFIDENTAIL
        /// </summary>
        fCONFIDENTAIL = 0x1u << 7, // TDI! Spelled wrongly in TD

        /// <summary>
        /// fNEVERVALUEAUDIT
        /// </summary>
        fNEVERVALUEAUDIT = 0x1u << 8,

        /// <summary>
        /// fRODCFILTEREDATTRIBUTE
        /// </summary>
        fRODCFILTEREDATTRIBUTE = 0x1u << 9,
    }

    /// <summary>
    /// Symbols used in representing instance type flags.
    /// </summary>
    /// Disable warning CA1028 because System.Int32 cannot match the enumeration design according to actual Model logic.
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum InstanceTypeFlags : uint
    {
        /// <summary>
        /// NC head bit
        /// </summary>
        IT_NC_HEAD = 0x1u,

        /// <summary>
        /// Write bit
        /// </summary>
        IT_WRITE = 0x1u << 2,

        /// <summary>
        /// NC above bit
        /// </summary>
        IT_NC_ABOVE = 0x1u << 3,
    }

    /// <summary>
    /// Symbols used in representing object class category.
    /// </summary>
    /// Disable warning CA1028 because System.Int32 cannot match the enumeration design according to actual Model logic.
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum ObjectClassCategory : uint
    {
        /// <summary>
        /// _88Class
        /// </summary>
        _88Class = 0x0,

        /// <summary>
        /// StructuralClass
        /// </summary>
        StructuralClass = 0x1,

        /// <summary>
        /// AbstractClass
        /// </summary>
        AbstractClass = 0x2,

        /// <summary>
        /// AuxiliaryClass
        /// </summary>
        AuxiliaryClass = 0x3
    }
}
