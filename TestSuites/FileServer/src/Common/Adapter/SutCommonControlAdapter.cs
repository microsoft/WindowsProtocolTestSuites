// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Novell.Directory.Ldap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;

namespace Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter
{
    /// <summary>
    /// A managed protocol adapter
    /// </summary>
    public class SutCommonControlAdapter : ManagedAdapterBase, ISutCommonControlAdapter
    {
        private TestConfigBase testConfig;
        // DC domain name
        private string domainName;
        // DC admin name
        private string adminName;
        // DC admin password
        private string adminPassword;

        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
            testConfig = new TestConfigBase(Site);
            TestTools.StackSdk.Security.KerberosLib.KerberosContext.KDCComputerName = testConfig.DCServerName;
            TestTools.StackSdk.Security.KerberosLib.KerberosContext.KDCPort = testConfig.KDCPort;
            domainName = testConfig.DomainName;
            adminName = testConfig.UserName;
            adminPassword = testConfig.UserPassword;
    }

        public string GetGroupMembers(string groupName)
        {
            int ldapPort = 389;
            string domainNetbios = domainName.Split('.')[0];
            string admin = string.Format("{0}\\{1}", domainNetbios.ToUpper(), adminName);
            string domainFqn = "DC=" + domainName.Replace(".", ",DC=");
            string groupFqn = $"CN={groupName},CN=Users,{domainFqn}";
            var groupSearchBase = domainFqn;
            var groupSearchFilter = $"(member={groupFqn})";

            LdapConnection conn = new LdapConnection();

            conn.Connect(domainName, ldapPort);
            conn.Bind(admin, adminPassword);

            var groupSearchQueue = conn.Search(groupSearchBase, LdapConnection.ScopeSub, groupSearchFilter, null, false, null, null);

            List<GroupMember> groupMembers = new List<GroupMember>();

            LdapMessage groupSearchMessage = null;
            while ((groupSearchMessage = groupSearchQueue.GetResponse()) != null)
            {
                if (groupSearchMessage is LdapSearchResult groupSearchResult)
                {
                    LdapEntry groupEntry = groupSearchResult.Entry;
                    string groupMemberName = groupEntry.GetAttribute("name").StringValue;
                    byte[] groupMemberSidBinary = groupEntry.GetAttribute("objectSid").ByteValue;
                    string groupMemberObjectClass = groupEntry.GetAttribute("objectClass").StringValue;
                    string groupMemberPrincipalSource = "ActiveDirectory";
                    _SID groupMemberSid = TypeMarshal.ToStruct<_SID>(groupMemberSidBinary);
                    GroupMember groupMember = new GroupMember();
                    groupMember.Name = groupMemberName;
                    groupMember.Sid = groupMemberSid;
                    groupMember.ObjectClass = groupMemberObjectClass;
                    groupMember.PrincipalSource = groupMemberPrincipalSource;
                    groupMembers.Add(groupMember);
                }
            }

            return JsonSerializer.Serialize(groupMembers);
        }

        public string GetGroups()
        {
            int ldapPort = 389;
            string domainNetbios = domainName.Split('.')[0];
            string admin = string.Format("{0}\\{1}", domainNetbios.ToUpper(), adminName);
            string domainFqn = "DC=" + domainName.Replace(".", ",DC=");
            var groupSearchBase = domainFqn;
            var groupSearchFilter = $"(objectClass=group)";

            LdapConnection conn = new LdapConnection();

            conn.Connect(domainName, ldapPort);
            conn.Bind(admin, adminPassword);

            var groupSearchQueue = conn.Search(groupSearchBase, LdapConnection.ScopeSub, groupSearchFilter, null, false, null, null);

            List<Group> groups = new List<Group>();

            LdapMessage groupSearchMessage = null;
            while ((groupSearchMessage = groupSearchQueue.GetResponse()) != null)
            {
                if (groupSearchMessage is LdapSearchResult groupSearchResult)
                {
                    LdapEntry groupEntry = groupSearchResult.Entry;
                    string groupName = groupEntry.GetAttribute("name").StringValue;
                    byte[] groupSidBinary = groupEntry.GetAttribute("objectSid").ByteValue;
                    _SID groupSid = TypeMarshal.ToStruct<_SID>(groupSidBinary);
                    Group group = new Group();
                    group.Name = groupName;
                    group.Sid = groupSid;
                    groups.Add(group);
                }
            }

            return JsonSerializer.Serialize(groups);
        }

        public string GetUserMemberships(string userName)
        {
            int ldapPort = 389;
            string domainNetbios = domainName.Split('.')[0];
            string admin = string.Format("{0}\\{1}", domainNetbios.ToUpper(), adminName);
            string domainFqn = "DC=" + domainName.Replace(".", ",DC=");
            string userFqn = $"CN={userName},CN=Users,{domainFqn}";
            var groupSearchBase = domainFqn;
            var groupSearchFilter = $"(member={userFqn})";

            LdapConnection conn = new LdapConnection();

            conn.Connect(domainName, ldapPort);
            conn.Bind(admin, adminPassword);

            var groupSearchQueue = conn.Search(groupSearchBase, LdapConnection.ScopeSub, groupSearchFilter, null, false, null, null);

            List<Group> groups = new List<Group>();

            LdapMessage groupSearchMessage = null;
            while ((groupSearchMessage = groupSearchQueue.GetResponse()) != null)
            {
                if (groupSearchMessage is LdapSearchResult groupSearchResult)
                {
                    LdapEntry groupEntry = groupSearchResult.Entry;
                    string groupName = groupEntry.GetAttribute("name").StringValue;
                    byte[] groupSidBinary = groupEntry.GetAttribute("objectSid").ByteValue;
                    _SID groupSid = TypeMarshal.ToStruct<_SID>(groupSidBinary);
                    Group group = new Group();
                    group.Name = groupName;
                    group.Sid = groupSid;
                    groups.Add(group);
                }
            }

            return JsonSerializer.Serialize(groups);
        }

        public string GetUsers()
        {
            int ldapPort = 389;
            string domainNetbios = domainName.Split('.')[0];
            string admin = string.Format("{0}\\{1}", domainNetbios.ToUpper(), adminName);
            string domainFqn = "DC=" + domainName.Replace(".", ",DC=");
            var userSearchBase = domainFqn;
            var userSearchFilter = $"(objectClass=user)";

            LdapConnection conn = new LdapConnection();

            conn.Connect(domainName, ldapPort);
            conn.Bind(admin, adminPassword);

            var userSearchQueue = conn.Search(userSearchBase, LdapConnection.ScopeSub, userSearchFilter, null, false, null, null);

            List<User> users = new List<User>();

            LdapMessage userSearchMessage = null;
            while ((userSearchMessage = userSearchQueue.GetResponse()) != null)
            {
                if (userSearchMessage is LdapSearchResult userSearchResult)
                {
                    LdapEntry userEntry = userSearchResult.Entry;
                    string userName = userEntry.GetAttribute("name").StringValue;
                    byte[] userSidBinary = userEntry.GetAttribute("objectSid").ByteValue;
                    _SID userSid = TypeMarshal.ToStruct<_SID>(userSidBinary);
                    User user = new User();
                    user.Name = userName;
                    user.Sid = userSid;
                    users.Add(user);
                }
            }

            return JsonSerializer.Serialize(users);
        }

        public string GetUserSid(string userName)
        {
            _SID sid = DtypUtility.GetSidFromAccount(domainName, userName, adminName, adminPassword);
            return sid.GetSddlForm();
        }

        public string GetGroupSid(string groupName)
        {
            int ldapPort = 389;
            string domainNetbios = domainName.Split('.')[0];
            string admin = string.Format("{0}\\{1}", domainNetbios.ToUpper(), adminName);
            string domainFqn = "DC=" + domainName.Replace(".", ",DC=");
            string groupFqn = $"CN={groupName},CN=Users,{domainFqn}";
            var groupSearchBase = domainFqn;
            var groupSearchFilter = $"(distinguishedName={groupFqn})";

            LdapConnection conn = new LdapConnection();

            conn.Connect(domainName, ldapPort);
            conn.Bind(admin, adminPassword);

            var groupSearchQueue = conn.Search(groupSearchBase, LdapConnection.ScopeSub, groupSearchFilter, null, false, null, null);

            LdapMessage groupSearchMessage = null;
            while ((groupSearchMessage = groupSearchQueue.GetResponse()) != null)
            {
                if (groupSearchMessage is LdapSearchResult groupSearchResult)
                {
                    LdapEntry groupEntry = groupSearchResult.Entry;
                    string groupMemberName = groupEntry.GetAttribute("name").StringValue;
                    byte[] groupMemberSidBinary = groupEntry.GetAttribute("objectSid").ByteValue;
                    _SID groupSid = TypeMarshal.ToStruct<_SID>(groupMemberSidBinary);
                    return groupSid.GetSddlForm();
                }
            }

            return "sid not found.";
        }
    }
}
