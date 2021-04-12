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

        private JsonSerializerOptions serializerOptions;

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

            serializerOptions = new JsonSerializerOptions();
            serializerOptions.Converters.Add(new _SIDConverter());
        }

        public string GetGroupMembers(string groupName)
        {
            List<LdapEntry> ldapGroupMembers = DtypUtility.GetGroupMembers(domainName, groupName, adminName, adminPassword);
            List<GroupMember> groupMembers = new List<GroupMember>();

            foreach (LdapEntry groupEntry in ldapGroupMembers)
            {
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

            return JsonSerializer.Serialize(groupMembers, serializerOptions);
        }

        public string GetGroups()
        {
            List<LdapEntry> ldapGroups = DtypUtility.GetGroups(domainName, adminName, adminPassword);
            List<Group> groups = new List<Group>();

            foreach (LdapEntry groupEntry in ldapGroups)
            {
                string groupName = groupEntry.GetAttribute("name").StringValue;
                byte[] groupSidBinary = groupEntry.GetAttribute("objectSid").ByteValue;
                _SID groupSid = TypeMarshal.ToStruct<_SID>(groupSidBinary);
                Group group = new Group();
                group.Name = groupName;
                group.Sid = groupSid;
                groups.Add(group);
            }

            return JsonSerializer.Serialize(groups, serializerOptions);
        }

        public string GetUserMemberships(string userName)
        {
            List<LdapEntry> ldapGroups = DtypUtility.GetUserMemberships(domainName, userName, adminName, adminPassword);
            List<Group> groups = new List<Group>();

            foreach (LdapEntry groupEntry in ldapGroups)
            {
                string groupName = groupEntry.GetAttribute("name").StringValue;
                byte[] groupSidBinary = groupEntry.GetAttribute("objectSid").ByteValue;
                _SID groupSid = TypeMarshal.ToStruct<_SID>(groupSidBinary);
                Group group = new Group();
                group.Name = groupName;
                group.Sid = groupSid;
                groups.Add(group);
            }

            return JsonSerializer.Serialize(groups, serializerOptions);
        }

        public string GetUsers()
        {
            List<LdapEntry> ldapUsers = DtypUtility.GetUsers(domainName, adminName, adminPassword);
            List<User> users = new List<User>();

            foreach(LdapEntry userEntry in ldapUsers)
            {
                string userName = userEntry.GetAttribute("name").StringValue;
                byte[] userSidBinary = userEntry.GetAttribute("objectSid").ByteValue;
                _SID userSid = TypeMarshal.ToStruct<_SID>(userSidBinary);
                User user = new User();
                user.Name = userName;
                user.Sid = userSid;
                users.Add(user);
            }

            return JsonSerializer.Serialize(users, serializerOptions);
        }

        public string GetUserSid(string userName)
        {
            _SID sid = DtypUtility.GetSidFromAccount(domainName, userName, adminName, adminPassword);
            return sid.GetSddlForm();
        }

        public string GetGroupSid(string groupName)
        {
            _SID sid = DtypUtility.GetSidFromGroupName(domainName, groupName, adminName, adminPassword);
            return sid.GetSddlForm();
        }
    }
}
