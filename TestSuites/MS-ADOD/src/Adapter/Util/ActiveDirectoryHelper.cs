// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools;
using System.Globalization;

namespace Microsoft.Protocol.TestSuites.ADOD.Adapter.Util
{
    public class ActiveDirectoryHelper
    {
        static ITestSite testSite;
        static ADODTestConfig testConfig;
        static ActiveDirectoryHelper()
        {
            testSite = testSite ?? TestClassBase.BaseTestSite;
            testConfig = new ADODTestConfig(testSite);
        }

        /// <summary>
        /// Delete an object from active directory
        /// </summary>
        /// <param name="objectPath">Specifies the path of entry to be deleted.</param>
        public static void DeleteObject(string objectPath)
        {
            if (DirectoryEntry.Exists("LDAP://" + objectPath))
            {
                try
                {
                    DirectoryEntry objectEntry = new DirectoryEntry("LDAP://" + objectPath);
                    DirectoryEntry parentEntry = objectEntry.Parent;
                    parentEntry.Children.Remove(objectEntry);
                    objectEntry.CommitChanges();
                }
                catch (Exception ex)
                {
                    testSite.Log.Add(LogEntryKind.Debug, ex.Message);
                }
            }
            else
            {
                testSite.Log.Add(LogEntryKind.Debug, objectPath + " doesn't exist");
            }
        }

        /// <summary>
        /// Delete a computer member from active directory
        /// </summary>
        /// <param name="computerName">Specifies the computer name to be deleted.</param>
        public static void DeleteComputer(string computerName)
        {
            DeleteObject(ObjectClass.Computer, computerName);
        }

        /// <summary>
        /// Delete an user from active directory
        /// </summary>
        /// <param name="userName">Specifies the user name to be deleted.</param>
        public static void DeleteUser(string userName)
        {
            DeleteObject(ObjectClass.User, userName);
        }

        /// <summary>
        /// Delete a group from active directory
        /// </summary>
        /// <param name="groupName">Specifies the group name to be deleted.</param>
        public static void DeleteGroup(string groupName)
        {
            DeleteObject(ObjectClass.Group, groupName);
        }

        /// <summary>
        /// Delete object by searching it against the directory
        /// </summary>
        /// <param name="objectClass">Specifies the type of the object to be deleted.</param>
        /// <param name="objectName">Specifies the name of the object to be deleted.</param>
        public static void DeleteObject(ObjectClass objectClass, string objectName)
        {
            DirectoryEntry objectEntry = SearchObject(objectClass, objectName);
            try
            {
                objectEntry.DeleteTree();                
            }
            catch (Exception ex)
            {
                testSite.Log.Add(LogEntryKind.Debug, ex.Message);
            }
        }

        /// <summary>
        /// Determines if the specified path exists in the directory service
        /// </summary>
        /// <param name="objectPath">Specifies the path of the entry to be verified.</param>
        /// <returns>Return true if object exists.</returns>
        public static bool IsObjectExist(string objectPath)
        {
            return DirectoryEntry.Exists("LDAP://" + objectPath);
        }

        /// <summary>
        /// Check if computer exists in the active directory
        /// </summary>
        /// <param name="computerName">Specifies the computer name to be verified.</param>
        /// <returns>If computer exists, return TRUE; else return FALSE.</returns>
        public static bool IsComputerExist(string computerName)
        {
            return IsObjectExist(ObjectClass.Computer, computerName);
        }

        /// <summary>
        /// Check if user exists in active directory
        /// </summary>
        /// <param name="userName">Specifies the user name to be verified.</param>
        /// <returns>If user exists, return TRUE; else return FALSE.</returns>
        public static bool IsUserExist(string userName)
        {
            return IsObjectExist(ObjectClass.User, userName);
        }

        /// <summary>
        /// Check if group exists in the active directory
        /// </summary>
        /// <param name="groupName">Specifies the group name to be verified.</param>
        /// <returns>If group exists, return TRUE; else return FALSE.</returns>
        public static bool IsGroupExist(string groupName)
        {
            return IsObjectExist(ObjectClass.Group, groupName);
        }

        /// <summary>
        /// Check if object exists by searching it against the directory
        /// </summary>
        /// <param name="objectClass">Specifies the object class, current supports user, computer, group.</param>
        /// <param name="objectName">Specifies the name of object to be checked.</param>
        /// <returns>If object exists, return TRUE; else, return FALSE.</returns>
        public static bool IsObjectExist(ObjectClass objectClass, string objectName)
        {
            DirectoryEntry objectEntry = SearchObject(objectClass, objectName);
            return objectEntry != null;
        }

        /// <summary>
        /// Search an object by object class and object name
        /// </summary>
        /// <param name="objectClass">Specifies the object class, current supports user, computer, group.</param>
        /// <param name="objectName">Specifies the name of object to be searched.</param>
        /// <returns>Return search result entry</returns>
        public static DirectoryEntry SearchObject(ObjectClass objectClass, string objectName)
        {
            DirectoryEntry rootEntry = new DirectoryEntry(string.Format(CultureInfo.InvariantCulture, "LDAP://{0}/{1}", testConfig.PDCIP, testConfig.RootDomainNC), testConfig.DomainAdminUsername, testConfig.DomainAdminPwd);
            DirectorySearcher searcher = new DirectorySearcher(rootEntry);

            switch (objectClass)
            {
                case ObjectClass.Computer:
                    searcher.Filter = "(&(objectCategory=computer)(objectClass=user)(cn=" + objectName + "))";
                    break;
                case ObjectClass.User:
                    searcher.Filter = "(&(objectCategory=person)(objectClass=user)(|(cn=" + objectName + ")(saMAccountName=" + objectName + ")(displayName=" + objectName + ")(givenName=" + objectName + ")))";
                    break;
                case ObjectClass.Group:
                    searcher.Filter = "(&(objectClass=group)(cn=" + objectName + "))";
                    break;
            }

            SearchResult searchResult = searcher.FindOne();
            if (searchResult == null)
                return null;
            else
            {
                DirectoryEntry entry = searchResult.GetDirectoryEntry();
                return entry;
            }

        }

        /// <summary>
        /// Authenticate user against the directory
        /// </summary>
        /// <param name="userName">Specifies the user name to be authenticated.</param>
        /// <param name="password">Specifies the password of user name.</param>
        /// <returns>Return true if user name and password is correct.</returns>
        public static bool AuthenticateUser(string userName, string password)
        {
            try
            {
                DirectoryEntry entry = new DirectoryEntry(string.Format(CultureInfo.InvariantCulture, "LDAP://{0}/{1}", testConfig.PDCIP, testConfig.RootDomainNC),
                   userName, password);
                object nativeObject = entry.NativeObject;
                return true;
            }
            catch (DirectoryServicesCOMException ex)
            {
                testSite.Log.Add(LogEntryKind.Debug, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Create a Computer Account
        /// </summary>
        /// <param name="computerName">Specifies the computer name.</param>
        public static void CreateComputer(string computerName)
        {
            if (!IsComputerExist(computerName))
            {
                try
                {
                    DirectoryEntry parentContainer = new DirectoryEntry(string.Format(CultureInfo.InvariantCulture, "LDAP://{0}/{1}", testConfig.PDCIP, "CN=Computers," + testConfig.RootDomainNC), testConfig.DomainAdminUsername, testConfig.DomainAdminPwd);
                    DirectoryEntry newComputer = parentContainer.Children.Add("CN=" + computerName, "computer");
                    newComputer.InvokeSet("displayName", computerName + "$");
                    newComputer.InvokeSet("sAMAccountName", computerName + "$");
                    newComputer.InvokeSet("userAccountControl", 0x1000);
                    newComputer.CommitChanges();
                    newComputer.InvokeSet("accountDisabled", true);
                    int val = (int)newComputer.Properties["userAccountControl"].Value;
                    if ((val & 0x0010) != 0)
                    {
                        newComputer.Properties["userAccountControl"].Value = (val ^ 0x0010);
                    }
                    newComputer.CommitChanges();
                    parentContainer.Close();
                    newComputer.Close();
                }
                catch (Exception ex)
                {
                    testSite.Log.Add(LogEntryKind.Debug, ex.Message);
                }
            }
            else 
            {
                testSite.Log.Add(LogEntryKind.Debug, computerName + " already existed in Active Directory.");
            }
        }

        /// <summary>
        /// Create a User Account
        /// </summary>
        /// <param name="userName">Specifies the user name.</param>
        /// <param name="password">Specify the password</param>
        public static void CreateUser(string userName, string password)
        {
            if (!IsUserExist(userName))
            {
                try
                {
                    DirectoryEntry parentContainer = new DirectoryEntry(string.Format(CultureInfo.InvariantCulture, "LDAP://{0}/{1}", testConfig.PDCIP, "CN=Users," + testConfig.RootDomainNC), testConfig.DomainAdminUsername, testConfig.DomainAdminPwd);
                    DirectoryEntry newUser = parentContainer.Children.Add("CN=" + userName, "user");
                    newUser.InvokeSet("displayName", userName);
                    newUser.InvokeSet("sAMAccountName", userName);
                    newUser.CommitChanges();
                    newUser.InvokeSet("accountDisabled", false);
                    newUser.Invoke("SetPassword", new object[] { password });
                    int val = (int)newUser.Properties["userAccountControl"].Value;
                    if ((val & 0x0010) != 0)
                    {
                        newUser.Properties["userAccountControl"].Value = (val ^ 0x0010);
                    }
                    newUser.CommitChanges();
                    parentContainer.Close();
                    newUser.Close();
                }
                catch (Exception ex)
                {
                    testSite.Log.Add(LogEntryKind.Debug, ex.Message);
                }
            }
            else 
            {
                testSite.Log.Add(LogEntryKind.Debug, userName + " already existed in Active Directory.");
            }
        }

        /// <summary>
        /// Create a New Security Group.
        /// </summary>
        /// <param name="userName">Specifies the group name.</param>
        public static void CreateGroup(string groupName)
        {
            if (!IsGroupExist(groupName))
            {
                try
                {
                    DirectoryEntry parentContainer = new DirectoryEntry(string.Format(CultureInfo.InvariantCulture, "LDAP://{0}/{1}", testConfig.PDCIP, "CN=Users," + testConfig.RootDomainNC), testConfig.DomainAdminUsername, testConfig.DomainAdminPwd);
                    DirectoryEntry newGroup = parentContainer.Children.Add("CN=" + groupName, "group");
                    newGroup.InvokeSet("displayName", groupName);
                    newGroup.InvokeSet("sAMAccountName", groupName);
                    newGroup.CommitChanges();
                    parentContainer.Close();
                    newGroup.Close();
                }
                catch (Exception ex)
                {
                    testSite.Log.Add(LogEntryKind.Debug, ex.Message);
                }
            }
            else
            {
                testSite.Log.Add(LogEntryKind.Debug, groupName + " already existed in Active Directory.");
            }
        }

        /// <summary>
        /// Add User to Group
        /// </summary>
        /// <param name="userName">Specifies the user name.</param>
        /// <param name="groupName">Specifies the group name.</param>
        public static void AddToGroup(string userName, string groupName)
        {
            try
            {
                DirectoryEntry userEntry = SearchObject(ObjectClass.User, userName);
                DirectoryEntry groupEntry = SearchObject(ObjectClass.Group, groupName);
                groupEntry.Properties["member"].Add(userEntry.Properties["distinguishedName"].Value);
                groupEntry.CommitChanges();
                groupEntry.Close();
                userEntry.Close();
            }
            catch (Exception ex)
            {
                testSite.Log.Add(LogEntryKind.Debug, ex.Message);
            }
        }
    }

    public enum ObjectClass
    {
        User,
        Group,
        Computer
    }
}
