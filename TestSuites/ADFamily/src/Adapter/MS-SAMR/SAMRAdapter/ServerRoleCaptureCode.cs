// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts;
using Microsoft.Protocols.TestTools.StackSdk.Security.Samr;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Collections;
using ActiveDs;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Samr
{
    /// <summary>
    /// Verify SAMR Protocol
    /// </summary>
    public partial class SAMRProtocolAdapter
    {
        /// <summary>
        /// Verify GroupGeneralInformation Fields from SamrQueryInformationGroup
        /// </summary>
        /// <param name="groupName">The name of group to be verified.</param>
        /// <param name="groupInfo">The returned group info buffer from SamrQueryInformationGroup.</param>
        public void VerifyGroupGeneralInformationFields(string groupName, _SAMPR_GROUP_INFO_BUFFER? groupInfo)
        {
            uint temp = groupInfo.Value.General.Attributes & (Utilities.SE_GROUP_MANDATORY 
                | Utilities.SE_GROUP_ENABLED_BY_DEFAULT
                | Utilities.SE_GROUP_ENABLED);

            Site.Assert.AreEqual<uint>(
                (groupInfo.Value.General.Attributes & temp),
                temp,
                @"Attributes should be mapped.
                On a DC configuration:
                the 'Attributes' bit field is handled as follows, If 'Attributes' field associated with
                a group object and a user membership for a group is queried, the returned value MUST be
                a logical union of the following bits: SE_GROUP_MANDATORY, SE_GROUP_ENABLED_BY_DEFAULT,
                and SE_GROUP_ENABLED.");

            string groupPath = string.Format("LDAP://{0}/CN={1},{2}", _samrProtocolAdapter.pdcFqdn, groupName, _samrProtocolAdapter.primaryDomainUserContainerDN);
            DirectoryEntry entry = new DirectoryEntry(groupPath);

            if (entry.Properties["description"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(groupInfo.Value.General.AdminComment.Buffer)), "AdminComment should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["description"].Value.ToString(), utilityObject.convertToString(groupInfo.Value.General.AdminComment.Buffer), "AdminComment should be mapped.");
            }

            Site.Assert.AreEqual((uint)entry.Properties["member"].Count, groupInfo.Value.General.MemberCount, "MemberCount should be mapped.");

            if (entry.Properties["sAMAccountName"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(groupInfo.Value.General.Name.Buffer)), "Name should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["sAMAccountName"].Value.ToString(), utilityObject.convertToString(groupInfo.Value.General.Name.Buffer), "Name should be mapped.");
            }

            Site.Assert.Pass(
                "Upon receiving SamrQueryInformationGroup message, the information levels" +
                "GroupGeneralInformation MUST be processed by setting the appropriate" +
                "output field name to the associated database attribute.");
        }

        /// <summary>
        /// Verify VerifyGroupNameInformation Fields from SamrQueryInformationGroup
        /// </summary>
        /// <param name="groupName">The name of group to be verified.</param>
        /// <param name="groupInfo">The returned group info buffer from SamrQueryInformationGroup.</param>
        public void VerifyGroupNameInformationFields(string groupName, _SAMPR_GROUP_INFO_BUFFER? groupInfo)
        {
            string groupPath = string.Format("LDAP://{0}/CN={1},{2}", _samrProtocolAdapter.pdcFqdn, groupName, _samrProtocolAdapter.primaryDomainUserContainerDN);
            DirectoryEntry entry = new DirectoryEntry(groupPath);

            if (entry.Properties["sAMAccountName"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(groupInfo.Value.Name.Name.Buffer)), "Name should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["sAMAccountName"].Value.ToString(), utilityObject.convertToString(groupInfo.Value.Name.Name.Buffer), "Name should be mapped.");
            }

            Site.Assert.Pass(
                "Upon receiving SamrQueryInformationGroup message, the information levels" +
                "GroupNameInformation MUST be processed by setting the appropriate" +
                "output field name to the associated database attribute.");
        }

        /// <summary>
        /// Verify VerifyGroupAttributeInformationFields Fields from SamrQueryInformationGroup
        /// </summary>
        /// <param name="groupName">The name of group to be verified.</param>
        /// <param name="groupInfo">The returned group info buffer from SamrQueryInformationGroup.</param>
        public void VerifyGroupAttributeInformationFields(string groupName, _SAMPR_GROUP_INFO_BUFFER? groupInfo)
        {
            uint temp = groupInfo.Value.Attribute.Attributes & (Utilities.SE_GROUP_MANDATORY
                | Utilities.SE_GROUP_ENABLED_BY_DEFAULT
                | Utilities.SE_GROUP_ENABLED);

            Site.Assert.AreEqual<uint>(
                (groupInfo.Value.Attribute.Attributes & temp),
                temp,
                @"Attributes should be mapped.
                On a DC configuration:
                the 'Attributes' bit field is handled as follows, If 'Attributes' field associated with
                a group object and a user membership for a group is queried, the returned value MUST be
                a logical union of the following bits: SE_GROUP_MANDATORY, SE_GROUP_ENABLED_BY_DEFAULT,
                and SE_GROUP_ENABLED.");

            Site.Assert.Pass(
                "Upon receiving SamrQueryInformationGroup message, the information levels" +
                "GroupAttributeInformation MUST be processed by setting the appropriate" +
                "output field name to the associated database attribute.");
        }
        
        /// <summary>
        /// Verify VerifyGroupAdminCommentInformationFields Fields from SamrQueryInformationGroup
        /// </summary>
        /// <param name="groupName">The name of group to be verified.</param>
        /// <param name="groupInfo">The returned group info buffer from SamrQueryInformationGroup.</param>
        public void VerifyGroupAdminCommentInformationFields(string groupName, _SAMPR_GROUP_INFO_BUFFER? groupInfo)
        {
            string groupPath = string.Format("LDAP://{0}/CN={1},{2}", _samrProtocolAdapter.pdcFqdn, groupName, _samrProtocolAdapter.primaryDomainUserContainerDN);
            DirectoryEntry entry = new DirectoryEntry(groupPath);

            if (entry.Properties["description"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(groupInfo.Value.AdminComment.AdminComment.Buffer)), "AdminComment should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["description"].Value.ToString(), utilityObject.convertToString(groupInfo.Value.AdminComment.AdminComment.Buffer), "AdminComment should be mapped.");
            }

            Site.Assert.Pass(
                "Upon receiving SamrQueryInformationGroup message, the information levels" +
                "GroupAdminCommentInformation MUST be processed by setting the appropriate" +
                "output field name to the associated database attribute.");
        }

        /// <summary>
        /// Verify VerifyGroupReplicationInformationFields Fields from SamrQueryInformationGroup
        /// </summary>
        /// <param name="groupName">The name of group to be verified.</param>
        /// <param name="groupInfo">The returned group info buffer from SamrQueryInformationGroup.</param>
        public void VerifyGroupReplicationInformationFields(string groupName, _SAMPR_GROUP_INFO_BUFFER? groupInfo)
        {
            uint temp = groupInfo.Value.DoNotUse.Attributes & (Utilities.SE_GROUP_MANDATORY
                | Utilities.SE_GROUP_ENABLED_BY_DEFAULT
                | Utilities.SE_GROUP_ENABLED);

            Site.Assert.AreEqual<uint>(
                (groupInfo.Value.DoNotUse.Attributes & temp),
                temp,
                @"Attributes should be mapped.
                On a DC configuration:
                the 'Attributes' bit field is handled as follows, If 'Attributes' field associated with
                a group object and a user membership for a group is queried, the returned value MUST be
                a logical union of the following bits: SE_GROUP_MANDATORY, SE_GROUP_ENABLED_BY_DEFAULT,
                and SE_GROUP_ENABLED.");

            string groupPath = string.Format("LDAP://{0}/CN={1},{2}", _samrProtocolAdapter.pdcFqdn, groupName, _samrProtocolAdapter.primaryDomainUserContainerDN);
            DirectoryEntry entry = new DirectoryEntry(groupPath);

            if (entry.Properties["description"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(groupInfo.Value.DoNotUse.AdminComment.Buffer)), "AdminComment should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["description"].Value.ToString(), utilityObject.convertToString(groupInfo.Value.DoNotUse.AdminComment.Buffer), "AdminComment should be mapped.");
            }

            Site.Assert.AreEqual<uint>(0, groupInfo.Value.DoNotUse.MemberCount, "MemberCount should be mapped.");

            if (entry.Properties["sAMAccountName"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(groupInfo.Value.DoNotUse.Name.Buffer)), "Name should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["sAMAccountName"].Value.ToString(), utilityObject.convertToString(groupInfo.Value.DoNotUse.Name.Buffer), "Name should be mapped.");
            }

            Site.Assert.Pass(
                "Upon receiving SamrQueryInformationGroup message, the information levels" +
                "GroupReplicationInformation MUST be processed by setting the appropriate" +
                "output field name to the associated database attribute.");
        }

        /// <summary>
        /// Verify returned display group buffer from SamrQueryDisplayInformation
        /// </summary>
        /// <param name="buffer">The buffer to be verified.</param>
        public void VerifyQueryDisplayInformationForDomainDisplayGroup(_SAMPR_DOMAIN_DISPLAY_GROUP_BUFFER buffer) 
        {
            Site.Assert.IsTrue(buffer.EntriesRead > 0, "The number of entries returned should be larger than 0.");
            Dictionary<string, string> dic = new Dictionary<string, string>();
            LdapConnection con = new LdapConnection(new LdapDirectoryIdentifier(_samrProtocolAdapter.pdcFqdn),
    new NetworkCredential(_samrProtocolAdapter.DomainAdministratorName, _samrProtocolAdapter.DomainUserPassword, _samrProtocolAdapter.PrimaryDomainDnsName));
            con.SessionOptions.Sealing = false;
            con.SessionOptions.Signing = false;
            string filter = string.Format("(&(objectClass=group)(|(groupType={0})(groupType={1})))", Utilities.GROUP_TYPE_SECURITY_ACCOUNT, Utilities.GROUP_TYPE_SECURITY_UNIVERSAL);
            SearchRequest searchReq = new SearchRequest(
                    _samrProtocolAdapter.primaryDomainUserContainerDN,
                    filter,
                    System.DirectoryServices.Protocols.SearchScope.Subtree,
                    "sAMAccountName",
                    "distinguishedName");
            SearchResponse searchRep = (SearchResponse)con.SendRequest(searchReq);
            Site.Assert.AreEqual((uint)searchRep.Entries.Count, buffer.EntriesRead, "The returned group count should be right.");
            foreach (SearchResultEntry entry in searchRep.Entries)
            {
                string sAMAccountName = (string)entry.Attributes["sAMAccountName"].GetValues(Type.GetType("System.String"))[0];
                string distinguishedName = (string)entry.Attributes["distinguishedName"].GetValues(Type.GetType("System.String"))[0];
                dic.Add(sAMAccountName, distinguishedName);
            }
            con.Dispose();

            List<string> actualGroups = new List<string>();
            List<uint> indexes = new List<uint>();
            bool isUnique = true;
            bool isSorted = true;
            foreach (_SAMPR_DOMAIN_DISPLAY_GROUP group in buffer.Buffer)
            {
                string groupName = utilityObject.convertToString(group.AccountName.Buffer);
                if (actualGroups.Count > 0)
                {
                    if (string.Compare(actualGroups[actualGroups.Count - 1], groupName) >= 0)
                    {
                        isSorted = false;
                    }
                }
                actualGroups.Add(groupName);
                if (indexes.Contains(group.Index))
                {
                    isUnique = false;
                }
                else
                {
                    indexes.Add(group.Index);
                }

                uint temp = group.Attributes & (Utilities.SE_GROUP_MANDATORY
                | Utilities.SE_GROUP_ENABLED_BY_DEFAULT
                | Utilities.SE_GROUP_ENABLED);

                Site.Assert.AreEqual<uint>(
                    (group.Attributes & temp),
                    temp,
                    @"Attributes should be mapped.
                On a DC configuration:
                the 'Attributes' bit field is handled as follows, If 'Attributes' field associated with
                a group object and a user membership for a group is queried, the returned value MUST be
                a logical union of the following bits: SE_GROUP_MANDATORY, SE_GROUP_ENABLED_BY_DEFAULT,
                and SE_GROUP_ENABLED.");

                string groupPath = string.Format("LDAP://{0}/{1}", _samrProtocolAdapter.pdcFqdn, dic[groupName]);
                DirectoryEntry entry = new DirectoryEntry(groupPath);

                SecurityIdentifier sid = new SecurityIdentifier((byte[])entry.Properties["objectSid"].Value, 0);
                string[] sidArray = sid.ToString().Split('-');
                string rid = sidArray[sidArray.Length - 1];
                Site.Assert.AreEqual(rid, group.Rid.ToString(), "Rid returned is right for {0}.", groupName);
                if (entry.Properties["description"].Value == null)
                {
                    Site.Assert.IsTrue(string.IsNullOrEmpty(utilityObject.convertToString(group.AdminComment.Buffer)),
                        "AdminComment returned is right.");
                }
                else
                {
                    Site.Assert.AreEqual(entry.Properties["description"].Value.ToString(),
                        utilityObject.convertToString(group.AdminComment.Buffer),
                        "AdminComment returned is right.");
                }
            }
            Site.Assert.IsTrue(isSorted, "This method MUST return a set of database objects, sorted by their sAMAccountName attribute value");
            Site.Assert.IsTrue(isUnique, "Every element has a unique index.");  
        }

        /// <summary>
        /// Verify returned enumeration buffer from SamrEnumerateGroupsInDomain
        /// </summary>
        /// <param name="buffer">The buffer to be verified.</param>
        public void VerifyEnumerateGroupsInDomainResults(_SAMPR_ENUMERATION_BUFFER? buffer)
        {
            List<string> groups = new List<string>(utilityObject.convertToString((_SAMPR_ENUMERATION_BUFFER)buffer, buffer.Value.EntriesRead));
            List<string> rids = new List<string>();
            foreach (_SAMPR_RID_ENUMERATION rid in buffer.Value.Buffer)
            {
                rids.Add(rid.RelativeId.ToString());
            }
            LdapConnection con = new LdapConnection(new LdapDirectoryIdentifier(_samrProtocolAdapter.pdcFqdn),
                new NetworkCredential(_samrProtocolAdapter.DomainAdministratorName, _samrProtocolAdapter.DomainUserPassword, _samrProtocolAdapter.PrimaryDomainDnsName));
            con.SessionOptions.Sealing = false;
            con.SessionOptions.Signing = false;
            string filter = string.Format("(&(objectClass=group)(|(groupType={0})(groupType={1})))", Utilities.GROUP_TYPE_SECURITY_ACCOUNT, Utilities.GROUP_TYPE_SECURITY_UNIVERSAL);
            SearchRequest searchReq = new SearchRequest(
                    _samrProtocolAdapter.primaryDomainUserContainerDN,
                    filter,
                    System.DirectoryServices.Protocols.SearchScope.Subtree,
                    "sAMAccountName",
                    "objectSid");
            SearchResponse searchRep = (SearchResponse)con.SendRequest(searchReq);

            Site.Assert.AreEqual((uint)searchRep.Entries.Count, buffer.Value.EntriesRead, "Each element of Buffer.Buffer MUST represent one database object that matches the Enumerate-Filter.");
            List<string> databaseGroups = new List<string>();
            List<string> databaseRids = new List<string>();
            foreach (System.DirectoryServices.Protocols.SearchResultEntry entry in searchRep.Entries)
            {
                string name = (string)entry.Attributes["sAMAccountName"].GetValues(Type.GetType("System.String"))[0];
                databaseGroups.Add(name);

                byte[] values = (byte[])entry.Attributes["objectSid"].GetValues(Type.GetType("System.Byte[]"))[0];
                SecurityIdentifier Sid = new SecurityIdentifier(values, 0);
                string[] sidArray = Sid.ToString().Split('-');
                string rid = sidArray[sidArray.Length - 1];
                databaseRids.Add(rid);
            }

            groups.Sort();
            rids.Sort();
            databaseGroups.Sort();
            databaseRids.Sort();
            Site.Assert.IsTrue(Enumerable.SequenceEqual<string>(groups, databaseGroups),
                "Buffer.Buffer.Name is the sAMAccountName attribute value of the database object.");
            Site.Assert.IsTrue(Enumerable.SequenceEqual<string>(rids, databaseRids),
                "Buffer.Buffer.RelativeId is the RID of the objectSid attribute of the database object.");
        }

        /// <summary>
        /// Verify UserAllInformation Fields from SamrQueryInformationUser and SamrQueryInformationUser2
        /// </summary>
        /// <param name="userName">The name of user to be verified.</param>
        /// <param name="userInfo">The returned user info buffer from SamrQueryInformationUser/SamrQueryInformationUser2.</param>
        public void VerifyUserAllInformationFields(string userName, _SAMPR_USER_INFO_BUFFER? userInfo, uint grantedAccess = Utilities.USER_READ_GENERAL | Utilities.USER_READ_LOGON | Utilities.USER_READ_ACCOUNT | Utilities.USER_READ_PREFERENCES)
        {
            bool isVerifyR3032 = false;

            if ((userInfo.Value.All.WhichFields &
                (uint)(USER_ALLValue.USER_ALL_NTPASSWORDPRESENT | USER_ALLValue.USER_ALL_LMPASSWORDPRESENT
                | USER_ALLValue.USER_ALL_PRIVATEDATA | USER_ALLValue.USER_ALL_PASSWORDEXPIRED
                | USER_ALLValue.USER_ALL_SECURITYDESCRIPTOR)) == 0)
            {
                isVerifyR3032 = true;
            }

            Site.CaptureRequirementIfIsTrue(
                isVerifyR3032, 3032,
                @"The following bits in Buffer.All.WhichFields, and their corresponding field values, MUST never be returned by the server.
                WhichFields                 bits
                USER_ALL_NTPASSWORDPRESENT  0x01000000
                USER_ALL_LMPASSWORDPRESENT  0x02000000
                USER_ALL_PRIVATEDATA        0x04000000
                USER_ALL_PASSWORDEXPIRED    0x08000000
                USER_ALL_SECURITYDESCRIPTOR 0x10000000");

            Site.Log.Add(LogEntryKind.Comment, "The server MUST set the fields of Buffer.All based on the access granted in UserHandle.GrantedAccess.");
            string userPath = string.Format("LDAP://{0}/CN={1},{2}", _samrProtocolAdapter.pdcFqdn, userName, _samrProtocolAdapter.primaryDomainUserContainerDN);
            DirectoryEntry entry = new DirectoryEntry(userPath);

            if ((grantedAccess & Utilities.USER_READ_GENERAL) == Utilities.USER_READ_GENERAL)
            {
                Site.Assert.IsTrue((userInfo.Value.All.WhichFields & (uint)USER_ALLValue.USER_ALL_USERNAME) == (uint)USER_ALLValue.USER_ALL_USERNAME,
                    "USER_ALL_USERNAME MUST be set if USER_READ_GENERAL is granted.");
                Site.Assert.AreEqual(entry.Properties["sAMAccountName"].Value.ToString(), utilityObject.convertToString(userInfo.Value.All.UserName.Buffer), "UserName in field should be equal to sAMAccountName in Database.");

                Site.Assert.IsTrue((userInfo.Value.All.WhichFields & (uint)USER_ALLValue.USER_ALL_FULLNAME) == (uint)USER_ALLValue.USER_ALL_FULLNAME,
                    "USER_ALL_FULLNAME MUST be set if USER_READ_GENERAL is granted.");
                if (entry.Properties["displayName"].Value == null)
                {
                    Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(userInfo.Value.All.FullName.Buffer)), "FullName should be mapped.");
                }
                else
                {
                    Site.Assert.AreEqual(entry.Properties["displayName"].Value.ToString(), utilityObject.convertToString(userInfo.Value.All.FullName.Buffer), "FullName should be mapped.");
                }

                Site.Assert.IsTrue((userInfo.Value.All.WhichFields & (uint)USER_ALLValue.USER_ALL_USERID) == (uint)USER_ALLValue.USER_ALL_USERID,
                    "USER_ALL_USERID MUST be set if USER_READ_GENERAL is granted.");
                SecurityIdentifier sid = new SecurityIdentifier((byte[])entry.Properties["objectSid"].Value, 0);
                string[] sidArray = sid.ToString().Split('-');
                string rid = sidArray[sidArray.Length - 1];
                Site.Assert.AreEqual(rid, userInfo.Value.All.UserId.ToString(), "UserId should be mapped.");

                Site.Assert.IsTrue((userInfo.Value.All.WhichFields & (uint)USER_ALLValue.USER_ALL_PRIMARYGROUPID) == (uint)USER_ALLValue.USER_ALL_PRIMARYGROUPID,
                    "USER_ALL_PRIMARYGROUPID MUST be set if USER_READ_GENERAL is granted.");
                if (entry.Properties["primaryGroupId"].Value == null)
                {
                    Site.Assert.IsTrue(String.IsNullOrEmpty(userInfo.Value.All.PrimaryGroupId.ToString()), "PrimaryGroupId should be mapped.");
                }
                else
                {
                    Site.Assert.AreEqual(entry.Properties["primaryGroupId"].Value.ToString(), userInfo.Value.All.PrimaryGroupId.ToString(), "PrimaryGroupId should be mapped.");
                }

                Site.Assert.IsTrue((userInfo.Value.All.WhichFields & (uint)USER_ALLValue.USER_ALL_ADMINCOMMENT) == (uint)USER_ALLValue.USER_ALL_ADMINCOMMENT,
                    "USER_ALL_ADMINCOMMENT MUST be set if USER_READ_GENERAL is granted.");
                if (entry.Properties["description"].Value == null)
                {
                    Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(userInfo.Value.All.AdminComment.Buffer)), "AdminComment should be mapped.");
                }
                else
                {
                    Site.Assert.AreEqual(entry.Properties["description"].Value.ToString(), utilityObject.convertToString(userInfo.Value.All.AdminComment.Buffer), "AdminComment should be mapped.");
                }

                Site.Assert.IsTrue((userInfo.Value.All.WhichFields & (uint)USER_ALLValue.USER_ALL_USERCOMMENT) == (uint)USER_ALLValue.USER_ALL_USERCOMMENT,
                    "USER_ALL_USERCOMMENT MUST be set if USER_READ_GENERAL is granted.");
                if (entry.Properties["comment"].Value == null)
                {
                    Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(userInfo.Value.All.UserComment.Buffer)), "UserComment should be mapped.");
                }
                else
                {
                    Site.Assert.AreEqual(entry.Properties["comment"].Value.ToString(), utilityObject.convertToString(userInfo.Value.All.UserComment.Buffer), "UserComment should be mapped.");
                }
            }
            if ((grantedAccess & Utilities.USER_READ_LOGON) == Utilities.USER_READ_LOGON)
            {
                Site.Assert.IsTrue((userInfo.Value.All.WhichFields & (uint)USER_ALLValue.USER_ALL_HOMEDIRECTORY) == (uint)USER_ALLValue.USER_ALL_HOMEDIRECTORY,
                    "USER_ALL_HOMEDIRECTORY MUST be set if USER_READ_LOGON is granted.");
                if (entry.Properties["homeDirectory"].Value == null)
                {
                    Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(userInfo.Value.All.HomeDirectory.Buffer)), "HomeDirectory should be mapped.");
                }
                else
                {
                    Site.Assert.AreEqual(entry.Properties["homeDirectory"].Value.ToString(), utilityObject.convertToString(userInfo.Value.All.HomeDirectory.Buffer), "HomeDirectory should be mapped.");
                }

                Site.Assert.IsTrue((userInfo.Value.All.WhichFields & (uint)USER_ALLValue.USER_ALL_HOMEDIRECTORYDRIVE) == (uint)USER_ALLValue.USER_ALL_HOMEDIRECTORYDRIVE,
                    "USER_ALL_HOMEDIRECTORYDRIVE MUST be set if USER_READ_LOGON is granted.");
                if (entry.Properties["homeDrive"].Value == null)
                {
                    Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(userInfo.Value.All.HomeDirectoryDrive.Buffer)), "HomeDirectoryDrive should be mapped.");
                }
                else
                {
                    Site.Assert.AreEqual(entry.Properties["homeDrive"].Value.ToString(), utilityObject.convertToString(userInfo.Value.All.HomeDirectoryDrive.Buffer), "HomeDirectoryDrive should be mapped.");
                }

                Site.Assert.IsTrue((userInfo.Value.All.WhichFields & (uint)USER_ALLValue.USER_ALL_SCRIPTPATH) == (uint)USER_ALLValue.USER_ALL_SCRIPTPATH,
                    "USER_ALL_SCRIPTPATH MUST be set if USER_READ_LOGON is granted.");
                if (entry.Properties["scriptPath"].Value == null)
                {
                    Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(userInfo.Value.All.ScriptPath.Buffer)), "ScriptPath should be mapped.");
                }
                else
                {
                    Site.Assert.AreEqual(entry.Properties["scriptPath"].Value.ToString(), utilityObject.convertToString(userInfo.Value.All.ScriptPath.Buffer), "ScriptPath should be mapped.");
                }

                Site.Assert.IsTrue((userInfo.Value.All.WhichFields & (uint)USER_ALLValue.USER_ALL_PROFILEPATH) == (uint)USER_ALLValue.USER_ALL_PROFILEPATH,
                    "USER_ALL_PROFILEPATH MUST be set if USER_READ_LOGON is granted.");
                if (entry.Properties["profilePath"].Value == null)
                {
                    Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(userInfo.Value.All.ProfilePath.Buffer)), "ProfilePath should be mapped.");
                }
                else
                {
                    Site.Assert.AreEqual(entry.Properties["profilePath"].Value.ToString(), utilityObject.convertToString(userInfo.Value.All.ProfilePath.Buffer), "ProfilePath should be mapped.");
                }

                Site.Assert.IsTrue((userInfo.Value.All.WhichFields & (uint)USER_ALLValue.USER_ALL_WORKSTATIONS) == (uint)USER_ALLValue.USER_ALL_WORKSTATIONS,
                    "USER_ALL_WORKSTATIONS MUST be set if USER_READ_LOGON is granted.");
                if (entry.Properties["userWorkstations"].Value == null)
                {
                    Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(userInfo.Value.All.WorkStations.Buffer)), "WorkStations should be mapped.");
                }
                else
                {
                    Site.Assert.AreEqual(entry.Properties["userWorkstations"].Value.ToString(), utilityObject.convertToString(userInfo.Value.All.WorkStations.Buffer), "WorkStations should be mapped.");
                }

                Site.Assert.IsTrue((userInfo.Value.All.WhichFields & (uint)USER_ALLValue.USER_ALL_LASTLOGON) == (uint)USER_ALLValue.USER_ALL_LASTLOGON,
                    "USER_ALL_LASTLOGON MUST be set if USER_READ_LOGON is granted.");
                object lastLogon = entry.Properties["lastLogon"].Value;
                Int32 highPart = (Int32)lastLogon.GetType().InvokeMember("HighPart",
                    System.Reflection.BindingFlags.GetProperty, null, lastLogon, null);
                Int32 lowPart = (Int32)lastLogon.GetType().InvokeMember("LowPart",
                    System.Reflection.BindingFlags.GetProperty, null, lastLogon, null);
                Site.Assert.AreEqual(highPart, userInfo.Value.All.LastLogon.HighPart, "LastLogon HighPart should be mapped.");
                Site.Assert.AreEqual((uint)lowPart, userInfo.Value.All.LastLogon.LowPart, "LastLogon lowPart should be mapped.");

                Site.Assert.IsTrue((userInfo.Value.All.WhichFields & (uint)USER_ALLValue.USER_ALL_LASTLOGOFF) == (uint)USER_ALLValue.USER_ALL_LASTLOGOFF,
                    "USER_ALL_LASTLOGOFF MUST be set if USER_READ_LOGON is granted.");
                object lastLogoff = entry.Properties["lastLogoff"].Value;
                highPart = (Int32)lastLogoff.GetType().InvokeMember("HighPart",
                    System.Reflection.BindingFlags.GetProperty, null, lastLogoff, null);
                lowPart = (Int32)lastLogoff.GetType().InvokeMember("LowPart",
                    System.Reflection.BindingFlags.GetProperty, null, lastLogoff, null);
                Site.Assert.AreEqual(highPart, userInfo.Value.All.LastLogoff.HighPart, "LastLogoff HighPart should be mapped.");
                Site.Assert.AreEqual((uint)lowPart, userInfo.Value.All.LastLogoff.LowPart, "LastLogoff LowPart should be mapped.");

                Site.Assert.IsTrue((userInfo.Value.All.WhichFields & (uint)USER_ALLValue.USER_ALL_LOGONHOURS) == (uint)USER_ALLValue.USER_ALL_LOGONHOURS,
                    "USER_ALL_LOGONHOURS MUST be set if USER_READ_LOGON is granted.");
                if (entry.Properties["logonHours"].Value == null)
                {
                    Site.Assert.IsTrue(String.IsNullOrEmpty(userInfo.Value.All.LogonHours.ToString()), "LogonHours should be mapped.");
                }
                else
                {

                    Site.Assert.AreEqual(System.Text.Encoding.Default.GetString((byte[])entry.Properties["logonHours"].Value), System.Text.Encoding.Default.GetString(userInfo.Value.All.LogonHours.LogonHours),
                        "LogonHours should be mapped.");
                }

                Site.Assert.IsTrue((userInfo.Value.All.WhichFields & (uint)USER_ALLValue.USER_ALL_BADPASSWORDCOUNT) == (uint)USER_ALLValue.USER_ALL_BADPASSWORDCOUNT,
                    "USER_ALL_BADPASSWORDCOUNT MUST be set if USER_READ_LOGON is granted.");
                if (entry.Properties["badPwdCount"].Value == null)
                {
                    Site.Assert.IsTrue(String.IsNullOrEmpty(userInfo.Value.All.BadPasswordCount.ToString()), "BasPasswordCount should be mapped.");
                }
                else
                {
                    Site.Assert.AreEqual(entry.Properties["badPwdCount"].Value.ToString(), userInfo.Value.All.BadPasswordCount.ToString(), "BadPasswordCount should be mapped.");
                }

                Site.Assert.IsTrue((userInfo.Value.All.WhichFields & (uint)USER_ALLValue.USER_ALL_LOGONCOUNT) == (uint)USER_ALLValue.USER_ALL_LOGONCOUNT,
                    "USER_ALL_LOGONCOUNT MUST be set if USER_READ_LOGON is granted.");
                if (entry.Properties["logonCount"].Value == null)
                {
                    Site.Assert.IsTrue(String.IsNullOrEmpty(userInfo.Value.All.LogonCount.ToString()), "LogonCount should be mapped.");
                }
                else
                {
                    Site.Assert.AreEqual(entry.Properties["logonCount"].Value.ToString(), userInfo.Value.All.LogonCount.ToString(), "LogonCount should be mapped.");
                }
            }
            if ((grantedAccess & Utilities.USER_READ_ACCOUNT) == Utilities.USER_READ_ACCOUNT)
            {
                Site.Assert.IsTrue((userInfo.Value.All.WhichFields & (uint)USER_ALLValue.USER_ALL_PASSWORDLASTSET) == (uint)USER_ALLValue.USER_ALL_PASSWORDLASTSET,
                    "USER_ALL_PASSWORDLASTSET MUST be set if USER_READ_ACCOUNT is granted.");
                object pwdLastSet = entry.Properties["pwdLastSet"].Value;
                Int32 highPart = (Int32)pwdLastSet.GetType().InvokeMember("HighPart",
                    System.Reflection.BindingFlags.GetProperty, null, pwdLastSet, null);
                Int32 lowPart = (Int32)pwdLastSet.GetType().InvokeMember("LowPart",
                    System.Reflection.BindingFlags.GetProperty, null, pwdLastSet, null);
                Site.Assert.AreEqual(highPart, userInfo.Value.All.PasswordLastSet.HighPart, "PasswordLastSet HighPart should be mapped.");
                Site.Assert.AreEqual((uint)lowPart, userInfo.Value.All.PasswordLastSet.LowPart, "PasswordLastSet LowPart should be mapped.");

                Site.Assert.IsTrue((userInfo.Value.All.WhichFields & (uint)USER_ALLValue.USER_ALL_ACCOUNTEXPIRES) == (uint)USER_ALLValue.USER_ALL_ACCOUNTEXPIRES,
                    "USER_ALL_ACCOUNTEXPIRES MUST be set if USER_READ_ACCOUNT is granted.");
                object accountExpires = entry.Properties["accountExpires"].Value;
                highPart = (Int32)accountExpires.GetType().InvokeMember("HighPart",
                    System.Reflection.BindingFlags.GetProperty, null, accountExpires, null);
                lowPart = (Int32)accountExpires.GetType().InvokeMember("LowPart",
                    System.Reflection.BindingFlags.GetProperty, null, accountExpires, null);
                Site.Assert.AreEqual(highPart, userInfo.Value.All.AccountExpires.HighPart, "AccountExpires HighPart should be mapped.");
                Site.Assert.AreEqual((uint)lowPart, userInfo.Value.All.AccountExpires.LowPart, "AccountExpires LowPart should be mapped.");

                Site.Assert.IsTrue((userInfo.Value.All.WhichFields & (uint)USER_ALLValue.USER_ALL_USERACCOUNTCONTROL) == (uint)USER_ALLValue.USER_ALL_USERACCOUNTCONTROL,
                    "USER_ALL_USERACCOUNTCONTROL MUST be set if USER_READ_ACCOUNT is granted.");
                Site.Assert.IsTrue(VerifyUserAccountControl(UInt32.Parse(entry.Properties["userAccountControl"].Value.ToString()), userInfo.Value.All.UserAccountControl),
                    "UserAccountControl:{0} should be mapped to userAccountControl:{1} in the attributes.", userInfo.Value.All.UserAccountControl, entry.Properties["userAccountControl"].Value.ToString());

                Site.Assert.IsTrue((userInfo.Value.All.WhichFields & (uint)USER_ALLValue.USER_ALL_PARAMETERS) == (uint)USER_ALLValue.USER_ALL_PARAMETERS,
                    "USER_ALL_PARAMETERS MUST be set if USER_READ_ACCOUNT is granted.");
                if (entry.Properties["userParameters"].Value == null)
                {
                    Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(userInfo.Value.All.Parameters.Buffer)), "Parameters should be mapped.");
                }
                else
                {
                    Site.Assert.AreEqual(entry.Properties["userParameters"].Value.ToString(), utilityObject.convertToString(userInfo.Value.All.Parameters.Buffer), "Parameters should be mapped.");
                }
            }
            if ((grantedAccess & Utilities.USER_READ_PREFERENCES) == Utilities.USER_READ_PREFERENCES)
            {
                Site.Assert.IsTrue((userInfo.Value.All.WhichFields & (uint)USER_ALLValue.USER_ALL_COUNTRYCODE) == (uint)USER_ALLValue.USER_ALL_COUNTRYCODE,
                    "USER_ALL_COUNTRYCODE MUST be set if USER_READ_PREFERENCES is granted.");
                if (entry.Properties["countryCode"].Value == null)
                {
                    Site.Assert.IsTrue(String.IsNullOrEmpty(userInfo.Value.All.CountryCode.ToString()), "CountryCode should be mapped.");
                }
                else
                {
                    Site.Assert.AreEqual(entry.Properties["countryCode"].Value.ToString(), userInfo.Value.All.CountryCode.ToString(), "CountryCode should be mapped.");
                }

                Site.Assert.IsTrue((userInfo.Value.All.WhichFields & (uint)USER_ALLValue.USER_ALL_CODEPAGE) == (uint)USER_ALLValue.USER_ALL_CODEPAGE,
                    "USER_ALL_CODEPAGE MUST be set if USER_READ_PREFERENCES is granted.");
                if (entry.Properties["codePage"].Value == null)
                {
                    Site.Assert.IsTrue(String.IsNullOrEmpty(userInfo.Value.All.CodePage.ToString()), "CodePage should be mapped.");
                }
                else
                {
                    Site.Assert.AreEqual(entry.Properties["codePage"].Value.ToString(), userInfo.Value.All.CodePage.ToString(), "CodePage should be mapped.");
                }
            }

            Site.Assert.IsNull(userInfo.Value.All.PrivateData.Buffer, "PrivateData:Not used. Ignored on receipt at the server and client. Servers MUST set to zero on return.");
            Site.Assert.IsNull(userInfo.Value.All.SecurityDescriptor.SecurityDescriptor, "SecurityDescriptor:Not used. Ignored on receipt at the server and client. Servers MUST set to zero on return.");
        }

        /// <summary>
        /// Verify UserAccountInformation Fields from SamrQueryInformationUser and SamrQueryInformationUser2
        /// </summary>
        /// <param name="userName">The name of user to be verified.</param>
        /// <param name="userInfo">The returned user info buffer from SamrQueryInformationUser/SamrQueryInformationUser2.</param>
        public void VerifyUserAccountInformationFields(string userName, _SAMPR_USER_INFO_BUFFER? userInfo)
        {
            string userPath = string.Format("LDAP://{0}/CN={1},{2}", _samrProtocolAdapter.pdcFqdn, userName, _samrProtocolAdapter.primaryDomainUserContainerDN);
            DirectoryEntry entry = new DirectoryEntry(userPath);

            Site.Assert.AreEqual(entry.Properties["sAMAccountName"].Value.ToString(), utilityObject.convertToString(userInfo.Value.Account.UserName.Buffer), "UserName in field should be equal to sAMAccountName in Database.");

            if (entry.Properties["displayName"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(userInfo.Value.Account.FullName.Buffer)), "FullName should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["displayName"].Value.ToString(), utilityObject.convertToString(userInfo.Value.Account.FullName.Buffer), "FullName should be mapped.");
            }

            SecurityIdentifier sid = new SecurityIdentifier((byte[])entry.Properties["objectSid"].Value, 0);
            string[] sidArray = sid.ToString().Split('-');
            string rid = sidArray[sidArray.Length - 1];
            Site.Assert.AreEqual(rid, userInfo.Value.Account.UserId.ToString(), "UserId should be mapped.");

            if (entry.Properties["primaryGroupId"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(userInfo.Value.Account.PrimaryGroupId.ToString()), "PrimaryGroupId should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["primaryGroupId"].Value.ToString(), userInfo.Value.Account.PrimaryGroupId.ToString(), "PrimaryGroupId should be mapped.");
            }

            if (entry.Properties["homeDirectory"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(userInfo.Value.Account.HomeDirectory.Buffer)), "HomeDirectory should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["homeDirectory"].Value.ToString(), utilityObject.convertToString(userInfo.Value.Account.HomeDirectory.Buffer), "HomeDirectory should be mapped.");
            }

            if (entry.Properties["homeDrive"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(userInfo.Value.Account.HomeDirectoryDrive.Buffer)), "HomeDirectoryDrive should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["homeDrive"].Value.ToString(), utilityObject.convertToString(userInfo.Value.Account.HomeDirectoryDrive.Buffer), "HomeDirectoryDrive should be mapped.");
            }

            if (entry.Properties["scriptPath"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(userInfo.Value.Account.ScriptPath.Buffer)), "ScriptPath should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["scriptPath"].Value.ToString(), utilityObject.convertToString(userInfo.Value.Account.ScriptPath.Buffer), "ScriptPath should be mapped.");
            }
            if (entry.Properties["profilePath"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(userInfo.Value.Account.ProfilePath.Buffer)), "ProfilePath should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["profilePath"].Value.ToString(), utilityObject.convertToString(userInfo.Value.Account.ProfilePath.Buffer), "ProfilePath should be mapped.");
            }

            if (entry.Properties["description"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(userInfo.Value.Account.AdminComment.Buffer)), "AdminComment should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["description"].Value.ToString(), utilityObject.convertToString(userInfo.Value.Account.AdminComment.Buffer), "AdminComment should be mapped.");
            }

            if (entry.Properties["userWorkstations"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(userInfo.Value.Account.WorkStations.Buffer)), "WorkStations should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["userWorkstations"].Value.ToString(), utilityObject.convertToString(userInfo.Value.Account.WorkStations.Buffer), "WorkStations should be mapped.");
            }

            object lastLogon = entry.Properties["lastLogon"].Value;
            Int32 highPart = (Int32)lastLogon.GetType().InvokeMember("HighPart",
                System.Reflection.BindingFlags.GetProperty, null, lastLogon, null);
            Int32 lowPart = (Int32)lastLogon.GetType().InvokeMember("LowPart",
                System.Reflection.BindingFlags.GetProperty, null, lastLogon, null);
            Site.Assert.AreEqual(highPart, userInfo.Value.Account.LastLogon.HighPart, "LastLogon HighPart should be mapped.");
            Site.Assert.AreEqual((uint)lowPart, userInfo.Value.Account.LastLogon.LowPart, "LastLogon lowPart should be mapped.");

            object lastLogoff = entry.Properties["lastLogoff"].Value;
            highPart = (Int32)lastLogoff.GetType().InvokeMember("HighPart",
                System.Reflection.BindingFlags.GetProperty, null, lastLogoff, null);
            lowPart = (Int32)lastLogoff.GetType().InvokeMember("LowPart",
                System.Reflection.BindingFlags.GetProperty, null, lastLogoff, null);
            Site.Assert.AreEqual(highPart, userInfo.Value.Account.LastLogoff.HighPart, "LastLogoff HighPart should be mapped.");
            Site.Assert.AreEqual((uint)lowPart, userInfo.Value.Account.LastLogoff.LowPart, "LastLogoff LowPart should be mapped.");

            if (entry.Properties["logonHours"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(userInfo.Value.Account.LogonHours.ToString()), "LogonHours should be mapped.");
            }
            else
            {

                Site.Assert.AreEqual(System.Text.Encoding.Default.GetString((byte[])entry.Properties["logonHours"].Value), System.Text.Encoding.Default.GetString(userInfo.Value.Account.LogonHours.LogonHours),
                    "LogonHours should be mapped.");
            }

            if (entry.Properties["badPwdCount"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(userInfo.Value.Account.BadPasswordCount.ToString()), "BasPasswordCount should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["badPwdCount"].Value.ToString(), userInfo.Value.Account.BadPasswordCount.ToString(), "BadPasswordCount should be mapped.");
            }

            if (entry.Properties["logonCount"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(userInfo.Value.Account.LogonCount.ToString()), "LogonCount should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["logonCount"].Value.ToString(), userInfo.Value.Account.LogonCount.ToString(), "LogonCount should be mapped.");
            }

            object pwdLastSet = entry.Properties["pwdLastSet"].Value;
            highPart = (Int32)pwdLastSet.GetType().InvokeMember("HighPart",
                System.Reflection.BindingFlags.GetProperty, null, pwdLastSet, null);
            lowPart = (Int32)pwdLastSet.GetType().InvokeMember("LowPart",
                System.Reflection.BindingFlags.GetProperty, null, pwdLastSet, null);
            Site.Assert.AreEqual(highPart, userInfo.Value.Account.PasswordLastSet.HighPart, "PasswordLastSet HighPart should be mapped.");
            Site.Assert.AreEqual((uint)lowPart, userInfo.Value.Account.PasswordLastSet.LowPart, "PasswordLastSet LowPart should be mapped.");

            object accountExpires = entry.Properties["accountExpires"].Value;
            highPart = (Int32)accountExpires.GetType().InvokeMember("HighPart",
                System.Reflection.BindingFlags.GetProperty, null, accountExpires, null);
            lowPart = (Int32)accountExpires.GetType().InvokeMember("LowPart",
                System.Reflection.BindingFlags.GetProperty, null, accountExpires, null);
            Site.Assert.AreEqual(highPart, userInfo.Value.Account.AccountExpires.HighPart, "AccountExpires HighPart should be mapped.");
            Site.Assert.AreEqual((uint)lowPart, userInfo.Value.Account.AccountExpires.LowPart, "AccountExpires LowPart should be mapped.");

            Site.Assert.IsTrue(VerifyUserAccountControl(UInt32.Parse(entry.Properties["userAccountControl"].Value.ToString()), userInfo.Value.Account.UserAccountControl),
                "UserAccountControl:{0} should be mapped to userAccountControl:{1} in the attributes.", userInfo.Value.Account.UserAccountControl, entry.Properties["userAccountControl"].Value.ToString());
        }

        /// <summary>
        /// Verify UserGeneralInformation Fields from SamrQueryInformationUser and SamrQueryInformationUser2
        /// </summary>
        /// <param name="userName">The name of user to be verified.</param>
        /// <param name="userInfo">The returned user info buffer from SamrQueryInformationUser/SamrQueryInformationUser2.</param>
        public void VerifyUserGeneralInformationFields(string userName, _SAMPR_USER_INFO_BUFFER? userInfo)
        {
            string userPath = string.Format("LDAP://{0}/CN={1},{2}", _samrProtocolAdapter.pdcFqdn, userName, _samrProtocolAdapter.primaryDomainUserContainerDN);
            DirectoryEntry entry = new DirectoryEntry(userPath);

            Site.Assert.AreEqual(entry.Properties["sAMAccountName"].Value.ToString(), utilityObject.convertToString(userInfo.Value.General.UserName.Buffer), "UserName in field should be equal to sAMAccountName in Database.");

            if (entry.Properties["displayName"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(userInfo.Value.General.FullName.Buffer)), "FullName should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["displayName"].Value.ToString(), utilityObject.convertToString(userInfo.Value.General.FullName.Buffer), "FullName should be mapped.");
            }

            if (entry.Properties["primaryGroupId"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(userInfo.Value.General.PrimaryGroupId.ToString()), "PrimaryGroupId should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["primaryGroupId"].Value.ToString(), userInfo.Value.General.PrimaryGroupId.ToString(), "PrimaryGroupId should be mapped.");
            }

            if (entry.Properties["description"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(userInfo.Value.General.AdminComment.Buffer)), "AdminComment should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["description"].Value.ToString(), utilityObject.convertToString(userInfo.Value.General.AdminComment.Buffer), "AdminComment should be mapped.");
            }

            if (entry.Properties["comment"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(userInfo.Value.General.UserComment.Buffer)), "UserComment should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["comment"].Value.ToString(), utilityObject.convertToString(userInfo.Value.General.UserComment.Buffer), "UserComment should be mapped.");
            }
        }

        /// <summary>
        /// Verify UserPrimaryGroupInformation Fields from SamrQueryInformationUser and SamrQueryInformationUser2
        /// </summary>
        /// <param name="userName">The name of user to be verified.</param>
        /// <param name="userInfo">The returned user info buffer from SamrQueryInformationUser/SamrQueryInformationUser2.</param>
        public void VerifyUserPrimaryGroupInformationFields(string userName, _SAMPR_USER_INFO_BUFFER? userInfo)
        {
            string userPath = string.Format("LDAP://{0}/CN={1},{2}", _samrProtocolAdapter.pdcFqdn, userName, _samrProtocolAdapter.primaryDomainUserContainerDN);
            DirectoryEntry entry = new DirectoryEntry(userPath);

            if (entry.Properties["primaryGroupId"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(userInfo.Value.PrimaryGroup.PrimaryGroupId.ToString()), "PrimaryGroupId should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["primaryGroupId"].Value.ToString(), userInfo.Value.PrimaryGroup.PrimaryGroupId.ToString(), "PrimaryGroupId should be mapped.");
            }
        }

        /// <summary>
        /// Verify UserNameInformation Fields from SamrQueryInformationUser and SamrQueryInformationUser2
        /// </summary>
        /// <param name="userName">The name of user to be verified.</param>
        /// <param name="userInfo">The returned user info buffer from SamrQueryInformationUser/SamrQueryInformationUser2.</param>
        public void VerifyUserNameInformationFields(string userName, _SAMPR_USER_INFO_BUFFER? userInfo)
        {
            string userPath = string.Format("LDAP://{0}/CN={1},{2}", _samrProtocolAdapter.pdcFqdn, userName, _samrProtocolAdapter.primaryDomainUserContainerDN);
            DirectoryEntry entry = new DirectoryEntry(userPath);

            Site.Assert.AreEqual(entry.Properties["sAMAccountName"].Value.ToString(), utilityObject.convertToString(userInfo.Value.Name.UserName.Buffer), "UserName in field should be equal to sAMAccountName in Database.");

            if (entry.Properties["displayName"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(userInfo.Value.Name.FullName.Buffer)), "FullName should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["displayName"].Value.ToString(), utilityObject.convertToString(userInfo.Value.Name.FullName.Buffer), "FullName should be mapped.");
            }
        }

        /// <summary>
        /// Verify UserAccountNameInformation Fields from SamrQueryInformationUser and SamrQueryInformationUser2
        /// </summary>
        /// <param name="userName">The name of user to be verified.</param>
        /// <param name="userInfo">The returned user info buffer from SamrQueryInformationUser/SamrQueryInformationUser2.</param>
        public void VerifyUserAccountNameInformationFields(string userName, _SAMPR_USER_INFO_BUFFER? userInfo)
        {
            string userPath = string.Format("LDAP://{0}/CN={1},{2}", _samrProtocolAdapter.pdcFqdn, userName, _samrProtocolAdapter.primaryDomainUserContainerDN);
            DirectoryEntry entry = new DirectoryEntry(userPath);

            Site.Assert.AreEqual(entry.Properties["sAMAccountName"].Value.ToString(), utilityObject.convertToString(userInfo.Value.AccountName.UserName.Buffer), "UserName in field should be equal to sAMAccountName in Database.");
        }

        /// <summary>
        /// Verify UserFullNameInformation Fields from SamrQueryInformationUser and SamrQueryInformationUser2
        /// </summary>
        /// <param name="userName">The name of user to be verified.</param>
        /// <param name="userInfo">The returned user info buffer from SamrQueryInformationUser/SamrQueryInformationUser2.</param>
        public void VerifyUserFullNameInformationFields(string userName, _SAMPR_USER_INFO_BUFFER? userInfo)
        {
            string userPath = string.Format("LDAP://{0}/CN={1},{2}", _samrProtocolAdapter.pdcFqdn, userName, _samrProtocolAdapter.primaryDomainUserContainerDN);
            DirectoryEntry entry = new DirectoryEntry(userPath);

            if (entry.Properties["displayName"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(userInfo.Value.FullName.FullName.Buffer)), "FullName should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["displayName"].Value.ToString(), utilityObject.convertToString(userInfo.Value.FullName.FullName.Buffer), "FullName should be mapped.");
            }
        }

        /// <summary>
        /// Verify UserAdminCommentInformation Fields from SamrQueryInformationUser and SamrQueryInformationUser2
        /// </summary>
        /// <param name="userName">The name of user to be verified.</param>
        /// <param name="userInfo">The returned user info buffer from SamrQueryInformationUser/SamrQueryInformationUser2.</param>
        public void VerifyUserAdminCommentInformationFields(string userName, _SAMPR_USER_INFO_BUFFER? userInfo)
        {
            string userPath = string.Format("LDAP://{0}/CN={1},{2}", _samrProtocolAdapter.pdcFqdn, userName, _samrProtocolAdapter.primaryDomainUserContainerDN);
            DirectoryEntry entry = new DirectoryEntry(userPath);

            if (entry.Properties["description"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(userInfo.Value.AdminComment.AdminComment.Buffer)), "AdminComment should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["description"].Value.ToString(), utilityObject.convertToString(userInfo.Value.AdminComment.AdminComment.Buffer), "AdminComment should be mapped.");
            }
        }

        /// <summary>
        /// Verify UserPreferencesInformation Fields from SamrQueryInformationUser and SamrQueryInformationUser2
        /// </summary>
        /// <param name="userName">The name of user to be verified.</param>
        /// <param name="userInfo">The returned user info buffer from SamrQueryInformationUser/SamrQueryInformationUser2.</param>
        public void VerifyUserPreferencesInformationFields(string userName, _SAMPR_USER_INFO_BUFFER? userInfo)
        {
            string userPath = string.Format("LDAP://{0}/CN={1},{2}", _samrProtocolAdapter.pdcFqdn, userName, _samrProtocolAdapter.primaryDomainUserContainerDN);
            DirectoryEntry entry = new DirectoryEntry(userPath);

            if (entry.Properties["comment"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(userInfo.Value.Preferences.UserComment.Buffer)), "UserComment should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["comment"].Value.ToString(), utilityObject.convertToString(userInfo.Value.Preferences.UserComment.Buffer), "UserComment should be mapped.");
            }

            Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(userInfo.Value.Preferences.Reserved1.Buffer)), 
                "Reserved1: Ignored by the client and server and MUST be a zero-length string when sent and returned.");

            if (entry.Properties["countryCode"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(userInfo.Value.Preferences.CountryCode.ToString()), "CountryCode should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["countryCode"].Value.ToString(), userInfo.Value.Preferences.CountryCode.ToString(), "CountryCode should be mapped.");
            }

            if (entry.Properties["codePage"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(userInfo.Value.Preferences.CodePage.ToString()), "CodePage should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["codePage"].Value.ToString(), userInfo.Value.Preferences.CodePage.ToString(), "CodePage should be mapped.");
            }
        }

        /// <summary>
        /// Verify UserLogonInformation Fields from SamrQueryInformationUser and SamrQueryInformationUser2
        /// </summary>
        /// <param name="userName">The name of user to be verified.</param>
        /// <param name="userInfo">The returned user info buffer from SamrQueryInformationUser/SamrQueryInformationUser2.</param>
        public void VerifyUserLogonInformationFields(string userName, _SAMPR_USER_INFO_BUFFER? userInfo)
        {
            string userPath = string.Format("LDAP://{0}/CN={1},{2}", _samrProtocolAdapter.pdcFqdn, userName, _samrProtocolAdapter.primaryDomainUserContainerDN);
            DirectoryEntry entry = new DirectoryEntry(userPath);

            Site.Assert.AreEqual(entry.Properties["sAMAccountName"].Value.ToString(), utilityObject.convertToString(userInfo.Value.Logon.UserName.Buffer), "UserName in field should be equal to sAMAccountName in Database.");

            if (entry.Properties["displayName"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(userInfo.Value.Logon.FullName.Buffer)), "FullName should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["displayName"].Value.ToString(), utilityObject.convertToString(userInfo.Value.Logon.FullName.Buffer), "FullName should be mapped.");
            }

            SecurityIdentifier sid = new SecurityIdentifier((byte[])entry.Properties["objectSid"].Value, 0);
            string[] sidArray = sid.ToString().Split('-');
            string rid = sidArray[sidArray.Length - 1];
            Site.Assert.AreEqual(rid, userInfo.Value.Logon.UserId.ToString(), "UserId should be mapped.");

            if (entry.Properties["primaryGroupId"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(userInfo.Value.Logon.PrimaryGroupId.ToString()), "PrimaryGroupId should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["primaryGroupId"].Value.ToString(), userInfo.Value.Logon.PrimaryGroupId.ToString(), "PrimaryGroupId should be mapped.");
            }

            if (entry.Properties["homeDirectory"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(userInfo.Value.Logon.HomeDirectory.Buffer)), "HomeDirectory should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["homeDirectory"].Value.ToString(), utilityObject.convertToString(userInfo.Value.Logon.HomeDirectory.Buffer), "HomeDirectory should be mapped.");
            }

            if (entry.Properties["homeDrive"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(userInfo.Value.Logon.HomeDirectoryDrive.Buffer)), "HomeDirectoryDrive should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["homeDrive"].Value.ToString(), utilityObject.convertToString(userInfo.Value.Logon.HomeDirectoryDrive.Buffer), "HomeDirectoryDrive should be mapped.");
            }

            if (entry.Properties["scriptPath"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(userInfo.Value.Logon.ScriptPath.Buffer)), "ScriptPath should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["scriptPath"].Value.ToString(), utilityObject.convertToString(userInfo.Value.Logon.ScriptPath.Buffer), "ScriptPath should be mapped.");
            }
            if (entry.Properties["profilePath"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(userInfo.Value.Logon.ProfilePath.Buffer)), "ProfilePath should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["profilePath"].Value.ToString(), utilityObject.convertToString(userInfo.Value.Logon.ProfilePath.Buffer), "ProfilePath should be mapped.");
            }

            if (entry.Properties["userWorkstations"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(userInfo.Value.Logon.WorkStations.Buffer)), "WorkStations should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["userWorkstations"].Value.ToString(), utilityObject.convertToString(userInfo.Value.Logon.WorkStations.Buffer), "WorkStations should be mapped.");
            }

            object lastLogon = entry.Properties["lastLogon"].Value;
            Int32 highPart = (Int32)lastLogon.GetType().InvokeMember("HighPart",
                System.Reflection.BindingFlags.GetProperty, null, lastLogon, null);
            Int32 lowPart = (Int32)lastLogon.GetType().InvokeMember("LowPart",
                System.Reflection.BindingFlags.GetProperty, null, lastLogon, null);
            Site.Assert.AreEqual(highPart, userInfo.Value.Logon.LastLogon.HighPart, "LastLogon HighPart should be mapped.");
            Site.Assert.AreEqual((uint)lowPart, userInfo.Value.Logon.LastLogon.LowPart, "LastLogon lowPart should be mapped.");

            object lastLogoff = entry.Properties["lastLogoff"].Value;
            highPart = (Int32)lastLogoff.GetType().InvokeMember("HighPart",
                System.Reflection.BindingFlags.GetProperty, null, lastLogoff, null);
            lowPart = (Int32)lastLogoff.GetType().InvokeMember("LowPart",
                System.Reflection.BindingFlags.GetProperty, null, lastLogoff, null);
            Site.Assert.AreEqual(highPart, userInfo.Value.Logon.LastLogoff.HighPart, "LastLogoff HighPart should be mapped.");
            Site.Assert.AreEqual((uint)lowPart, userInfo.Value.Logon.LastLogoff.LowPart, "LastLogoff LowPart should be mapped.");

            object pwdLastSet = entry.Properties["pwdLastSet"].Value;
            highPart = (Int32)pwdLastSet.GetType().InvokeMember("HighPart",
                System.Reflection.BindingFlags.GetProperty, null, pwdLastSet, null);
            lowPart = (Int32)pwdLastSet.GetType().InvokeMember("LowPart",
                System.Reflection.BindingFlags.GetProperty, null, pwdLastSet, null);
            Site.Assert.AreEqual(highPart, userInfo.Value.Logon.PasswordLastSet.HighPart, "PasswordLastSet HighPart should be mapped.");
            Site.Assert.AreEqual((uint)lowPart, userInfo.Value.Logon.PasswordLastSet.LowPart, "PasswordLastSet LowPart should be mapped.");

            if (Utilities.UF_DONT_EXPIRE_PASSWORD == (UInt32.Parse(entry.Properties["userAccountControl"].Value.ToString()) & (Utilities.UF_DONT_EXPIRE_PASSWORD)) ||
                            Utilities.UF_SMARTCARD_REQUIRED == (UInt32.Parse(entry.Properties["userAccountControl"].Value.ToString()) & (Utilities.UF_SMARTCARD_REQUIRED)) ||
                            Utilities.UF_SERVER_TRUST_ACCOUNT == (UInt32.Parse(entry.Properties["userAccountControl"].Value.ToString()) & (Utilities.UF_SERVER_TRUST_ACCOUNT)) ||
                            Utilities.UF_INTERDOMAIN_TRUST_ACCOUNT == (UInt32.Parse(entry.Properties["userAccountControl"].Value.ToString()) & (Utilities.UF_INTERDOMAIN_TRUST_ACCOUNT)) ||
                            Utilities.UF_WORKSTATION_TRUST_ACCOUNT == (UInt32.Parse(entry.Properties["userAccountControl"].Value.ToString()) & (Utilities.UF_WORKSTATION_TRUST_ACCOUNT)))
            {
                Site.Assert.AreEqual(Convert.ToInt64(0x7FFFFFFFFFFFFFFF), utilityObject.GetLongEquivalent(userInfo.Value.Logon.PasswordMustChange),
                   "If the userAccountControl attribute value on the target user object contains any of the following bits: UF_DONT_EXPIRE_PASSWD, UF_SMARTCARD_REQUIRED, UF_INTERDOMAIN_TRUST_ACCOUNT, UF_WORKSTATION_TRUST_ACCOUNT, or UF_SERVER_TRUST_ACCOUNT, the PasswordMustChange value MUST be 0x7FFFFFFF FFFFFFFF.");
            }
            else if (highPart == 0 && lowPart == 0)
            {
                Site.Assert.AreEqual(0, utilityObject.GetLongEquivalent(userInfo.Value.Logon.PasswordMustChange),
                    "Else, if the pwdLastSet attribute value on the user object is 0, the PasswordMustChange value MUST be 0.");
            }

            if (entry.Properties["logonHours"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(userInfo.Value.Logon.LogonHours.ToString()), "LogonHours should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(System.Text.Encoding.Default.GetString((byte[])entry.Properties["logonHours"].Value), System.Text.Encoding.Default.GetString(userInfo.Value.Logon.LogonHours.LogonHours),
                    "LogonHours should be mapped.");
            }

            if (entry.Properties["badPwdCount"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(userInfo.Value.Logon.BadPasswordCount.ToString()), "BasPasswordCount should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["badPwdCount"].Value.ToString(), userInfo.Value.Logon.BadPasswordCount.ToString(), "BadPasswordCount should be mapped.");
            }

            if (entry.Properties["logonCount"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(userInfo.Value.Logon.LogonCount.ToString()), "LogonCount should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["logonCount"].Value.ToString(), userInfo.Value.Logon.LogonCount.ToString(), "LogonCount should be mapped.");
            }

            Site.Assert.IsTrue(VerifyUserAccountControl(UInt32.Parse(entry.Properties["userAccountControl"].Value.ToString()), userInfo.Value.Logon.UserAccountControl),
                "UserAccountControl:{0} should be mapped to userAccountControl:{1} in the attributes.", userInfo.Value.Logon.UserAccountControl, entry.Properties["userAccountControl"].Value.ToString());
        }

        /// <summary>
        /// Verify UserLogonHoursInformation Fields from SamrQueryInformationUser and SamrQueryInformationUser2
        /// </summary>
        /// <param name="userName">The name of user to be verified.</param>
        /// <param name="userInfo">The returned user info buffer from SamrQueryInformationUser/SamrQueryInformationUser2.</param>
        public void VerifyUserLogonHoursInformationFields(string userName, _SAMPR_USER_INFO_BUFFER? userInfo)
        {
            string userPath = string.Format("LDAP://{0}/CN={1},{2}", _samrProtocolAdapter.pdcFqdn, userName, _samrProtocolAdapter.primaryDomainUserContainerDN);
            DirectoryEntry entry = new DirectoryEntry(userPath);

            if (entry.Properties["logonHours"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(userInfo.Value.LogonHours.LogonHours.ToString()), "LogonHours should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(System.Text.Encoding.Default.GetString((byte[])entry.Properties["logonHours"].Value), System.Text.Encoding.Default.GetString(userInfo.Value.LogonHours.LogonHours.LogonHours),
                    "LogonHours should be mapped.");
            }
        }

        /// <summary>
        /// Verify UserHomeInformation Fields from SamrQueryInformationUser and SamrQueryInformationUser2
        /// </summary>
        /// <param name="userName">The name of user to be verified.</param>
        /// <param name="userInfo">The returned user info buffer from SamrQueryInformationUser/SamrQueryInformationUser2.</param>
        public void VerifyUserHomeInformationFields(string userName, _SAMPR_USER_INFO_BUFFER? userInfo)
        {
            string userPath = string.Format("LDAP://{0}/CN={1},{2}", _samrProtocolAdapter.pdcFqdn, userName, _samrProtocolAdapter.primaryDomainUserContainerDN);
            DirectoryEntry entry = new DirectoryEntry(userPath);

            if (entry.Properties["homeDirectory"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(userInfo.Value.Home.HomeDirectory.Buffer)), "HomeDirectory should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["homeDirectory"].Value.ToString(), utilityObject.convertToString(userInfo.Value.Home.HomeDirectory.Buffer), "HomeDirectory should be mapped.");
            }

            if (entry.Properties["homeDrive"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(userInfo.Value.Home.HomeDirectoryDrive.Buffer)), "HomeDirectoryDrive should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["homeDrive"].Value.ToString(), utilityObject.convertToString(userInfo.Value.Home.HomeDirectoryDrive.Buffer), "HomeDirectoryDrive should be mapped.");
            }
        }

        /// <summary>
        /// Verify UserScriptInformation Fields from SamrQueryInformationUser and SamrQueryInformationUser2
        /// </summary>
        /// <param name="userName">The name of user to be verified.</param>
        /// <param name="userInfo">The returned user info buffer from SamrQueryInformationUser/SamrQueryInformationUser2.</param>
        public void VerifyUserScriptInformationFields(string userName, _SAMPR_USER_INFO_BUFFER? userInfo)
        {
            string userPath = string.Format("LDAP://{0}/CN={1},{2}", _samrProtocolAdapter.pdcFqdn, userName, _samrProtocolAdapter.primaryDomainUserContainerDN);
            DirectoryEntry entry = new DirectoryEntry(userPath);

            if (entry.Properties["scriptPath"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(userInfo.Value.Script.ScriptPath.Buffer)), "ScriptPath should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["scriptPath"].Value.ToString(), utilityObject.convertToString(userInfo.Value.Script.ScriptPath.Buffer), "ScriptPath should be mapped.");
            }
        }

        /// <summary>
        /// Verify UserProfileInformation Fields from SamrQueryInformationUser and SamrQueryInformationUser2
        /// </summary>
        /// <param name="userName">The name of user to be verified.</param>
        /// <param name="userInfo">The returned user info buffer from SamrQueryInformationUser/SamrQueryInformationUser2.</param>
        public void VerifyUserProfileInformationFields(string userName, _SAMPR_USER_INFO_BUFFER? userInfo)
        {
            string userPath = string.Format("LDAP://{0}/CN={1},{2}", _samrProtocolAdapter.pdcFqdn, userName, _samrProtocolAdapter.primaryDomainUserContainerDN);
            DirectoryEntry entry = new DirectoryEntry(userPath);

            if (entry.Properties["profilePath"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(userInfo.Value.Profile.ProfilePath.Buffer)), "ProfilePath should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["profilePath"].Value.ToString(), utilityObject.convertToString(userInfo.Value.Profile.ProfilePath.Buffer), "ProfilePath should be mapped.");
            }
        }

        /// <summary>
        /// Verify UserWorkStationsInformation Fields from SamrQueryInformationUser and SamrQueryInformationUser2
        /// </summary>
        /// <param name="userName">The name of user to be verified.</param>
        /// <param name="userInfo">The returned user info buffer from SamrQueryInformationUser/SamrQueryInformationUser2.</param>
        public void VerifyUserWorkStationsInformationFields(string userName, _SAMPR_USER_INFO_BUFFER? userInfo)
        {
            string userPath = string.Format("LDAP://{0}/CN={1},{2}", _samrProtocolAdapter.pdcFqdn, userName, _samrProtocolAdapter.primaryDomainUserContainerDN);
            DirectoryEntry entry = new DirectoryEntry(userPath);

            if (entry.Properties["userWorkstations"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(userInfo.Value.WorkStations.WorkStations.Buffer)), "WorkStations should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["userWorkstations"].Value.ToString(), utilityObject.convertToString(userInfo.Value.WorkStations.WorkStations.Buffer), "WorkStations should be mapped.");
            }
        }

        /// <summary>
        /// Verify UserControlInformation Fields from SamrQueryInformationUser and SamrQueryInformationUser2
        /// </summary>
        /// <param name="userName">The name of user to be verified.</param>
        /// <param name="userInfo">The returned user info buffer from SamrQueryInformationUser/SamrQueryInformationUser2.</param>
        public void VerifyUserControlInformationFields(string userName, _SAMPR_USER_INFO_BUFFER? userInfo)
        {
            string userPath = string.Format("LDAP://{0}/CN={1},{2}", _samrProtocolAdapter.pdcFqdn, userName, _samrProtocolAdapter.primaryDomainUserContainerDN);
            DirectoryEntry entry = new DirectoryEntry(userPath);

            Site.Assert.IsTrue(VerifyUserAccountControl(UInt32.Parse(entry.Properties["userAccountControl"].Value.ToString()), userInfo.Value.Control.UserAccountControl),
                "UserAccountControl:{0} should be mapped to userAccountControl:{1} in the attributes.", userInfo.Value.All.UserAccountControl, entry.Properties["userAccountControl"].Value.ToString());
        }

        /// <summary>
        /// Verify UserExpiresInformation Fields from SamrQueryInformationUser and SamrQueryInformationUser2
        /// </summary>
        /// <param name="userName">The name of user to be verified.</param>
        /// <param name="userInfo">The returned user info buffer from SamrQueryInformationUser/SamrQueryInformationUser2.</param>
        public void VerifyUserExpiresInformationFields(string userName, _SAMPR_USER_INFO_BUFFER? userInfo)
        {
            string userPath = string.Format("LDAP://{0}/CN={1},{2}", _samrProtocolAdapter.pdcFqdn, userName, _samrProtocolAdapter.primaryDomainUserContainerDN);
            DirectoryEntry entry = new DirectoryEntry(userPath);

            object accountExpires = entry.Properties["accountExpires"].Value;
            Int32 highPart = (Int32)accountExpires.GetType().InvokeMember("HighPart",
                System.Reflection.BindingFlags.GetProperty, null, accountExpires, null);
            Int32 lowPart = (Int32)accountExpires.GetType().InvokeMember("LowPart",
                System.Reflection.BindingFlags.GetProperty, null, accountExpires, null);
            Site.Assert.AreEqual(highPart, userInfo.Value.Expires.AccountExpires.HighPart, "AccountExpires HighPart should be mapped.");
            Site.Assert.AreEqual((uint)lowPart, userInfo.Value.Expires.AccountExpires.LowPart, "AccountExpires LowPart should be mapped.");
        }

        /// <summary>
        /// Verify UserParametersInformation Fields from SamrQueryInformationUser and SamrQueryInformationUser2
        /// </summary>
        /// <param name="userName">The name of user to be verified.</param>
        /// <param name="userInfo">The returned user info buffer from SamrQueryInformationUser/SamrQueryInformationUser2.</param>
        public void VerifyUserParametersInformationFields(string userName, _SAMPR_USER_INFO_BUFFER? userInfo)
        {
            string userPath = string.Format("LDAP://{0}/CN={1},{2}", _samrProtocolAdapter.pdcFqdn, userName, _samrProtocolAdapter.primaryDomainUserContainerDN);
            DirectoryEntry entry = new DirectoryEntry(userPath);

            if (entry.Properties["userParameters"].Value == null)
            {
                Site.Assert.IsTrue(String.IsNullOrEmpty(utilityObject.convertToString(userInfo.Value.Parameters.Parameters.Buffer)), "Parameters should be mapped.");
            }
            else
            {
                Site.Assert.AreEqual(entry.Properties["userParameters"].Value.ToString(), utilityObject.convertToString(userInfo.Value.Parameters.Parameters.Buffer), "Parameters should be mapped.");
            }
        }

        /// <summary>
        /// Verify UserAccountControl
        /// </summary>
        /// <param name="expect">Expect UserAccountControl.</param>
        /// <param name="actual">Actual UserAccountControl.</param>
        /// <returns>Return true if the two are consistent.</returns>
        public bool VerifyUserAccountControl(uint expect, uint actual)
        {
            Site.Log.Add(LogEntryKind.Debug, "Expect UserAccountControl is {0} while actual UserAccountControl is {1}", expect, actual);
            if ((expect & (uint)AdtsUserAccountControl.ADS_UF_ACCOUNT_DISABLE) == 0)
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_ACCOUNT_DISABLED) != 0)
                    return false;
            }
            else
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_ACCOUNT_DISABLED) == 0)
                    return false;
            }

            if ((expect & (uint)AdtsUserAccountControl.ADS_UF_HOMEDIR_REQUIRED) == 0)
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_HOME_DIRECTORY_REQUIRED) != 0)
                    return false;
            }
            else
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_HOME_DIRECTORY_REQUIRED) == 0)
                    return false;
            }

            if ((expect & (uint)AdtsUserAccountControl.ADS_UF_NORMAL_ACCOUNT) == 0)
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_NORMAL_ACCOUNT) != 0)
                    return false;
            }
            else
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_NORMAL_ACCOUNT) == 0)
                    return false;
            }

            if ((expect & (uint)AdtsUserAccountControl.ADS_UF_PASSWD_NOTREQD) == 0)
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_PASSWORD_NOT_REQUIRED) != 0)
                    return false;
            }
            else
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_PASSWORD_NOT_REQUIRED) == 0)
                    return false;
            }

            if ((expect & (uint)AdtsUserAccountControl.ADS_UF_INTERDOMAIN_TRUST_ACCOUNT) == 0)
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_INTERDOMAIN_TRUST_ACCOUNT) != 0)
                    return false;
            }
            else
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_INTERDOMAIN_TRUST_ACCOUNT) == 0)
                    return false;
            }

            if ((expect & (uint)AdtsUserAccountControl.ADS_UF_WORKSTATION_TRUST_ACCOUNT) == 0)
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_WORKSTATION_TRUST_ACCOUNT) != 0)
                    return false;
            }
            else
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_WORKSTATION_TRUST_ACCOUNT) == 0)
                    return false;
            }

            if ((expect & (uint)AdtsUserAccountControl.ADS_UF_SERVER_TRUST_ACCOUNT) == 0)
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_SERVER_TRUST_ACCOUNT) != 0)
                    return false;
            }
            else
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_SERVER_TRUST_ACCOUNT) == 0)
                    return false;
            }

            if ((expect & (uint)AdtsUserAccountControl.ADS_UF_DONT_EXPIRE_PASSWD) == 0)
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_DONT_EXPIRE_PASSWORD) != 0)
                    return false;
            }
            else
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_DONT_EXPIRE_PASSWORD) == 0)
                    return false;
            }

            if ((expect & (uint)AdtsUserAccountControl.ADS_UF_LOCKOUT) == 0)
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_ACCOUNT_AUTO_LOCKED) != 0)
                    return false;
            }
            else
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_ACCOUNT_AUTO_LOCKED) == 0)
                    return false;
            }

            if ((expect & (uint)AdtsUserAccountControl.ADS_UF_ENCRYPTED_TEXT_PASSWORD_ALLOWED) == 0)
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_ENCRYPTED_TEXT_PASSWORD_ALLOWED) != 0)
                    return false;
            }
            else
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_ENCRYPTED_TEXT_PASSWORD_ALLOWED) == 0)
                    return false;
            }

            if ((expect & (uint)AdtsUserAccountControl.ADS_UF_SMARTCARD_REQUIRED) == 0)
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_SMARTCARD_REQUIRED) != 0)
                    return false;
            }
            else
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_SMARTCARD_REQUIRED) == 0)
                    return false;
            }

            if ((expect & (uint)AdtsUserAccountControl.ADS_UF_TRUSTED_FOR_DELEGATION) == 0)
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_TRUSTED_FOR_DELEGATION) != 0)
                    return false;
            }
            else
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_TRUSTED_FOR_DELEGATION) == 0)
                    return false;
            }

            if ((expect & (uint)AdtsUserAccountControl.ADS_UF_NOT_DELEGATED) == 0)
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_NOT_DELEGATED) != 0)
                    return false;
            }
            else
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_NOT_DELEGATED) == 0)
                    return false;
            }

            if ((expect & (uint)AdtsUserAccountControl.ADS_UF_USE_DES_KEY_ONLY) == 0)
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_USE_DES_KEY_ONLY) != 0)
                    return false;
            }
            else
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_USE_DES_KEY_ONLY) == 0)
                    return false;
            }

            if ((expect & (uint)AdtsUserAccountControl.ADS_UF_DONT_REQUIRE_PREAUTH) == 0)
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_DONT_REQUIRE_PREAUTH) != 0)
                    return false;
            }
            else
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_DONT_REQUIRE_PREAUTH) == 0)
                    return false;
            }

            if ((expect & (uint)AdtsUserAccountControl.ADS_UF_PASSWORD_EXPIRED) == 0)
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_PASSWORD_EXPIRED) != 0)
                    return false;
            }
            else
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_PASSWORD_EXPIRED) == 0)
                    return false;
            }

            if ((expect & (uint)AdtsUserAccountControl.ADS_UF_TRUSTED_TO_AUTHENTICATE_FOR_DELEGATION) == 0)
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_TRUSTED_TO_AUTHENTICATE_FOR_DELEGATION) != 0)
                    return false;
            }
            else
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_TRUSTED_TO_AUTHENTICATE_FOR_DELEGATION) == 0)
                    return false;
            }

            if ((expect & (uint)AdtsUserAccountControl.ADS_UF_NO_AUTH_DATA_REQUIRED) == 0)
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_NO_AUTH_DATA_REQUIRED) != 0)
                    return false;
            }
            else
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_NO_AUTH_DATA_REQUIRED) == 0)
                    return false;
            }

            if ((expect & (uint)AdtsUserAccountControl.ADS_UF_PARTIAL_SECRETS_ACCOUNT) == 0)
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_PARTIAL_SECRETS_ACCOUNT) != 0)
                    return false;
            }
            else
            {
                if ((actual & (uint)USER_ACCOUNT_CONTROL.USER_PARTIAL_SECRETS_ACCOUNT) == 0)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Verify whether the rid is in the range
        /// </summary>
        /// <param name="relativeId">The Rid to be verified.</param>
        /// <returns>Return true if the Rid is in the range.</returns>
        public bool VerifyRelativeID(uint relativeId)
        {
            string ridEntryPath = string.Format("LDAP://{0}/CN=RID Set,CN={1},{2}",
                _samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PDCNetbiosName, _samrProtocolAdapter.primaryDomainControllerContainerDN);
            DirectoryEntry entry = new DirectoryEntry(ridEntryPath);
            //Let Rid-Range be the range specified by the rIDPreviousAllocationPool attribute of the Rid-Set object. 
            object ridRange = entry.Properties["rIDPreviousAllocationPool"].Value;
            //The upper bound of the Rid-Range is the second 32-bit integer of the rIDPreviousAllocationPool attribute value. 
            int ridUpperBound = (Int32)ridRange.GetType().InvokeMember("HighPart",
                System.Reflection.BindingFlags.GetProperty, null, ridRange, null);
            //The lower bound of the Rid-Range is the first 32-bit integer of the rIDPreviousAllocationPool attribute value. 
            int ridLowerBound = (Int32)ridRange.GetType().InvokeMember("LowPart",
                System.Reflection.BindingFlags.GetProperty, null, ridRange, null);

            return (relativeId >= ridLowerBound) && (relativeId <= ridUpperBound);
        }

        /// <summary>
        /// Verify returned display user buffer from SamrQueryDisplayInformation
        /// </summary>
        /// <param name="buffer">The buffer to be verified.</param>
        public void VerifyQueryDisplayInformationForDomainDisplayOemUser(_SAMPR_DOMAIN_DISPLAY_OEM_USER_BUFFER buffer)
        {
            Site.Assert.IsTrue(buffer.EntriesRead > 0, "The number of entries returned should be larger than 0.");
            Dictionary<string, string> dic = new Dictionary<string, string>();
            LdapConnection con = new LdapConnection(new LdapDirectoryIdentifier(_samrProtocolAdapter.pdcFqdn),
    new NetworkCredential(_samrProtocolAdapter.DomainAdministratorName, _samrProtocolAdapter.DomainUserPassword, _samrProtocolAdapter.PrimaryDomainDnsName));
            con.SessionOptions.Sealing = false;
            con.SessionOptions.Signing = false;
            SearchRequest searchReq = new SearchRequest(
                    _samrProtocolAdapter.primaryDomainUserContainerDN,
                    "(&(objectClass=user)(userAccountControl:1.2.840.113556.1.4.803:=512))",
                    System.DirectoryServices.Protocols.SearchScope.Subtree,
                    "sAMAccountName",
                    "distinguishedName");
            SearchResponse searchRep = (SearchResponse)con.SendRequest(searchReq);
            Site.Assert.AreEqual((uint)searchRep.Entries.Count, buffer.EntriesRead, "The returned user count should be right.");
            foreach (SearchResultEntry entry in searchRep.Entries)
            {
                string sAMAccountName = (string)entry.Attributes["sAMAccountName"].GetValues(Type.GetType("System.String"))[0];
                string distinguishedName = (string)entry.Attributes["distinguishedName"].GetValues(Type.GetType("System.String"))[0];
                dic.Add(sAMAccountName, distinguishedName);
            }
            con.Dispose();

            List<string> actualUsers = new List<string>();
            List<uint> indexes = new List<uint>();
            bool isUnique = true;
            bool isSorted = true;
            foreach (_SAMPR_DOMAIN_DISPLAY_OEM_USER user in buffer.Buffer)
            {
                string userName = new string(Encoding.Default.GetChars(user.OemAccountName.Buffer), 0, user.OemAccountName.Buffer.Length); ;
                if (actualUsers.Count > 0)
                {
                    if (string.Compare(actualUsers[actualUsers.Count - 1], userName) >= 0)
                    {
                        isSorted = false;
                    }
                }
                if (indexes.Contains(user.Index))
                {
                    isUnique = false;
                }
                else
                {
                    indexes.Add(user.Index);
                }
                string userPath = string.Format("LDAP://{0}/{1}", _samrProtocolAdapter.pdcFqdn, dic[userName]);
                Site.Assert.IsTrue(DirectoryEntry.Exists(userPath), "The returned user {0} exists in the database.", userName);
            }
            Site.Assert.IsTrue(isSorted, "This method MUST return a set of database objects, sorted by their sAMAccountName attribute value");
            Site.Assert.IsTrue(isUnique, "Every element has a unique index.");     
        }

        /// <summary>
        /// Verify returned display user buffer from SamrQueryDisplayInformation
        /// </summary>
        /// <param name="buffer">The buffer to be verified.</param>
        public void VerifyQueryDisplayInformationForDomainDisplayUser(_SAMPR_DOMAIN_DISPLAY_USER_BUFFER buffer)
        {
            Site.Assert.IsTrue(buffer.EntriesRead > 0, "The number of entries returned should be larger than 0.");
            Dictionary<string,string> dic = new Dictionary<string,string>();
            LdapConnection con = new LdapConnection(new LdapDirectoryIdentifier(_samrProtocolAdapter.pdcFqdn),
    new NetworkCredential(_samrProtocolAdapter.DomainAdministratorName, _samrProtocolAdapter.DomainUserPassword, _samrProtocolAdapter.PrimaryDomainDnsName));
            con.SessionOptions.Sealing = false;
            con.SessionOptions.Signing = false;
            SearchRequest searchReq = new SearchRequest(
                    _samrProtocolAdapter.primaryDomainUserContainerDN,
                    "(&(objectClass=user)(userAccountControl:1.2.840.113556.1.4.803:=512))",
                    System.DirectoryServices.Protocols.SearchScope.Subtree,
                    "sAMAccountName",
                    "distinguishedName");
            SearchResponse searchRep = (SearchResponse)con.SendRequest(searchReq);
            Site.Assert.AreEqual((uint)searchRep.Entries.Count, buffer.EntriesRead, "The returned user count should be right.");
            foreach(SearchResultEntry entry in searchRep.Entries)
            {
                string sAMAccountName = (string)entry.Attributes["sAMAccountName"].GetValues(Type.GetType("System.String"))[0];
                string distinguishedName = (string)entry.Attributes["distinguishedName"].GetValues(Type.GetType("System.String"))[0];
                dic.Add(sAMAccountName,distinguishedName);
            }
            con.Dispose();

            List<string> actualUsers = new List<string>();
            List<uint> indexes = new List<uint>();
            bool isUnique = true;
            bool isSorted = true;
            foreach (_SAMPR_DOMAIN_DISPLAY_USER user in buffer.Buffer)
            {
                string userName = utilityObject.convertToString(user.AccountName.Buffer);
                if (actualUsers.Count > 0)
                {
                    if (string.Compare(actualUsers[actualUsers.Count - 1], userName) >= 0)
                    {
                        isSorted = false;
                    }
                }
                actualUsers.Add(userName);
                if (indexes.Contains(user.Index))
                {
                    isUnique = false;
                }
                else
                {
                    indexes.Add(user.Index);
                }
                string userPath = string.Format("LDAP://{0}/{1}", _samrProtocolAdapter.pdcFqdn, dic[userName]);
                DirectoryEntry entry = new DirectoryEntry(userPath);

                SecurityIdentifier sid = new SecurityIdentifier((byte[])entry.Properties["objectSid"].Value, 0);
                string[] sidArray = sid.ToString().Split('-');
                string rid = sidArray[sidArray.Length - 1];
                Site.Assert.AreEqual(rid, user.Rid.ToString(), "Rid returned is right for {0}.", userName);

                Site.Assert.IsTrue(_samrProtocolAdapter.VerifyUserAccountControl(
                    UInt32.Parse(entry.Properties["userAccountControl"].Value.ToString()), user.AccountControl),
                    "AccountControl returned is right.");
                if (entry.Properties["description"].Value == null)
                {
                    Site.Assert.IsTrue(string.IsNullOrEmpty(utilityObject.convertToString(user.AdminComment.Buffer)),
                        "AdminComment returned is right.");
                }
                else
                {
                    Site.Assert.AreEqual(entry.Properties["description"].Value.ToString(),
                        utilityObject.convertToString(user.AdminComment.Buffer),
                        "AdminComment returned is right.");
                }
                if (entry.Properties["displayName"].Value == null)
                {
                    Site.Assert.IsTrue(string.IsNullOrEmpty(utilityObject.convertToString(user.FullName.Buffer)),
                        "FullName returned is right.");
                }
                else
                {
                    Site.Assert.AreEqual(entry.Properties["displayName"].Value.ToString(),
                        utilityObject.convertToString(user.FullName.Buffer),
                        "FullName returned is right.");
                }            
            }
            Site.Assert.IsTrue(isSorted, "This method MUST return a set of database objects, sorted by their sAMAccountName attribute value");
            Site.Assert.IsTrue(isUnique, "Every element has a unique index.");             
        }

        /// <summary>
        /// Verify returned enumeration buffer from SamrEnumerateUsersInDomain
        /// </summary>
        /// <param name="buffer">The buffer to be verified.</param>
        /// <param name="userAccountControlFilter">The userAccountControlFilter used to enumerate users.</param>
        public void VerifyEnumerateUsersInDomainResults(_SAMPR_ENUMERATION_BUFFER? buffer, uint userAccountControlFilter)
        {
            List<string> users = new List<string>(utilityObject.convertToString((_SAMPR_ENUMERATION_BUFFER)buffer, buffer.Value.EntriesRead));
            List<string> rids = new List<string>();
            foreach (_SAMPR_RID_ENUMERATION rid in buffer.Value.Buffer)
            {
                rids.Add(rid.RelativeId.ToString());
            }
            LdapConnection con = new LdapConnection(new LdapDirectoryIdentifier(_samrProtocolAdapter.pdcFqdn),
                new NetworkCredential(_samrProtocolAdapter.DomainAdministratorName, _samrProtocolAdapter.DomainUserPassword, _samrProtocolAdapter.PrimaryDomainDnsName));
            con.SessionOptions.Sealing = false;
            con.SessionOptions.Signing = false;
            string filter = string.Format("(&(objectClass=user)(userAccountControl:1.2.840.113556.1.4.803:={0}))", userAccountControlFilter.ToString());
            SearchRequest searchReq = new SearchRequest(
                    _samrProtocolAdapter.primaryDomainUserContainerDN,
                    filter,
                    System.DirectoryServices.Protocols.SearchScope.Subtree,
                    "sAMAccountName",
                    "objectSid");
            SearchResponse searchRep = (SearchResponse)con.SendRequest(searchReq);

            Site.Assert.AreEqual((uint)searchRep.Entries.Count, buffer.Value.EntriesRead, "Each element of Buffer.Buffer MUST represent one database object that matches the Enumerate-Filter.");
            List<string> databaseUsers = new List<string>();
            List<string> databaseRids = new List<string>();
            foreach (System.DirectoryServices.Protocols.SearchResultEntry entry in searchRep.Entries)
            {
                string name = (string)entry.Attributes["sAMAccountName"].GetValues(Type.GetType("System.String"))[0];
                databaseUsers.Add(name);

                byte[] values = (byte[])entry.Attributes["objectSid"].GetValues(Type.GetType("System.Byte[]"))[0];
                SecurityIdentifier Sid = new SecurityIdentifier(values, 0);
                string[] sidArray = Sid.ToString().Split('-');
                string rid = sidArray[sidArray.Length - 1];
                databaseRids.Add(rid);
            }

            users.Sort();
            rids.Sort();
            databaseUsers.Sort();
            databaseRids.Sort();
            Site.Assert.IsTrue(Enumerable.SequenceEqual<string>(users, databaseUsers),
                "Buffer.Buffer.Name is the sAMAccountName attribute value of the database object.");
            Site.Assert.IsTrue(Enumerable.SequenceEqual<string>(rids, databaseRids),
                "Buffer.Buffer.RelativeId is the RID of the objectSid attribute of the database object.");
        }

        /// <summary>
        /// Verify the constraints in Common Processing Of Enumeration of User, Group and Alias.
        /// </summary>
        /// <param name="buff">Indicate Buffer.Buffer entries as enumeration.</param>
        /// <param name="preferedMaximumLength">Indicate PreferedMaximumLength by the number of bytes of a maximum-sized entry.</param>
       public void VerifyConstraintsInCommonProcessingOfEnumeration(_SAMPR_RID_ENUMERATION[] buff, uint preferedMaximumLength)
        {
            int bufferLength = 0;
            foreach (_SAMPR_RID_ENUMERATION buffer in buff)
            {
                bufferLength += Marshal.SizeOf(buffer);
            }

            if (bufferLength > preferedMaximumLength)
            {
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-SAMR_R2893, byte of Buffer.Buffer {0}", bufferLength);
                bool isVerifyR2893 = false;
                foreach (_SAMPR_RID_ENUMERATION buffer in buff)
                {
                    if (Marshal.SizeOf(buffer) > bufferLength - preferedMaximumLength)
                    {
                        isVerifyR2893 = true;
                    }
                }

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR2893,
                    2893,
                    @"[In Common Processing for Enumeration of Users, Groups, and Aliases,Upon receiving 
                    SamrEnumerateGroupsInDomain, SamrEnumerateAliasesInDomain,or SamrEnumerateUsersInDomain
                    message, the server MUST process the data from the message, subject to the following 
                    constraints:]If the server returns more than PreferedMaximumLength bytes, the difference
                    between PreferedMaximumLength and the actual number of bytes returned MUST be less than
                    the maximum size, in bytes, of one entry in the array Buffer.Buffer.");
            }
        }

       /// <summary>
       /// Verify the constraints when the objectClass attribute value is user or computer, or derived from either of these classes.
       /// </summary>
       /// <param name="name">sAMAccountName</param>
       public void VerifyConstraintsForUserOrComputer(string name, ACCOUNT_TYPE accountType)
       {
           DirectoryEntry entry = new DirectoryEntry(string.Format("LDAP://{0}/{1}",_samrProtocolAdapter.primaryDomainFqdn,_samrProtocolAdapter.primaryDomainDN));
           object wkObjects = entry.Properties["wellKnownObjects"].Value;
           Dictionary<string,string> wellKnownObjects = new Dictionary<string,string>();
           foreach (DNWithBinary wkObject in (IEnumerable)wkObjects)
           {
               string dnOfWellKnownObj = wkObject.DNString;
               byte[] bytes = (byte[])wkObject.BinaryValue;
               string byteToHex = BitConverter.ToString(bytes).ToLower().Replace("-", String.Empty);
               wellKnownObjects.Add(byteToHex, dnOfWellKnownObj);
           }
           entry.Close();

           LdapConnection con = new LdapConnection(new LdapDirectoryIdentifier(_samrProtocolAdapter.pdcFqdn),
               new NetworkCredential(_samrProtocolAdapter.DomainAdministratorName, _samrProtocolAdapter.DomainUserPassword, _samrProtocolAdapter.PrimaryDomainDnsName));
           string filter = string.Format("(sAMAccountName={0})", name);
           SearchRequest request = new SearchRequest(_samrProtocolAdapter.primaryDomainDN, filter, System.DirectoryServices.Protocols.SearchScope.Subtree);
           SearchResponse response = (SearchResponse)con.SendRequest(request);
           Site.Assert.AreEqual(1, response.Entries.Count, "3.1.5.4.4 On successful completion of this method, a new database object MUST be created.");
           Site.Assert.AreEqual(1, response.Entries.Count, "3.1.5.4.4 sAMAccountName database attribute MUST be updated from the values provided in the message.");
           uint userAccountControl = 0;
           DirectoryAttribute attr = response.Entries[0].Attributes["userAccountControl"];
           for(int loopCount = 0; loopCount < attr.Count; loopCount++)
           {
               userAccountControl = UInt32.Parse(attr[loopCount].ToString());
           }

           foreach (DirectoryAttribute attribute in response.Entries[0].Attributes.Values)
           {
               switch (attribute.Name)
               {
                   case "distinguishedName":
                       if (accountType == ACCOUNT_TYPE.USER_NORMAL_ACCOUNT)
                       {
                           if (wellKnownObjects.Keys.Contains("a9d1ca15768811d1aded00c04fd8d5cd"))
                           {
                               Site.Assert.IsTrue(attribute[0].ToString().Contains(wellKnownObjects["a9d1ca15768811d1aded00c04fd8d5cd"]),
                                   "3.1.5.4.4 The distinguishedName attribute MUST be updated with a value that conforms to the constraints as specified in section 3.1.5.14.1.");
                           }
                           else
                           {
                               Site.Assert.IsTrue(attribute[0].ToString().Contains(_samrProtocolAdapter.primaryDomainUserContainerDN),
                                   "3.1.5.4.4 The distinguishedName attribute MUST be updated with a value that conforms to the constraints as specified in section 3.1.5.14.1.");
                           }
                       }
                       else if(accountType == ACCOUNT_TYPE.USER_SERVER_TRUST_ACCOUNT)
                       {
                           if (wellKnownObjects.Keys.Contains("a361b2ffffd211d1aa4b00c04fd7d83a"))
                           {
                               Site.Assert.IsTrue(attribute[0].ToString().Contains(wellKnownObjects["a361b2ffffd211d1aa4b00c04fd7d83a"]),
                                   "3.1.5.4.4 The distinguishedName attribute MUST be updated with a value that conforms to the constraints as specified in section 3.1.5.14.1.");
                           }
                           else
                           {
                               Site.Assert.IsTrue(attribute[0].ToString().Contains(_samrProtocolAdapter.primaryDomainControllerContainerDN),
                                    "3.1.5.4.4 The distinguishedName attribute MUST be updated with a value that conforms to the constraints as specified in section 3.1.5.14.1.");
                           }
                       }
                       else if (accountType == ACCOUNT_TYPE.USER_WORKSTATION_TRUST_ACCOUNT)
                       {
                           if (wellKnownObjects.Keys.Contains("aa312825768811d1aded00c04fd8d5cd"))
                           {
                               Site.Assert.IsTrue(attribute[0].ToString().Contains(wellKnownObjects["aa312825768811d1aded00c04fd8d5cd"]),
                                   "3.1.5.4.4 The distinguishedName attribute MUST be updated with a value that conforms to the constraints as specified in section 3.1.5.14.1.");
                           }
                           else
                           {
                               Site.Assert.IsTrue(attribute[0].ToString().Contains(_samrProtocolAdapter.primaryDomainComputerContainerDN),
                                   "3.1.5.4.4 The distinguishedName attribute MUST be updated with a value that conforms to the constraints as specified in section 3.1.5.14.1.");
                           }
                       }
                       break;
                   case "objectClass":
                       string objectClass = null;
                       for (int loopCount = 0; loopCount < attribute.Count; loopCount++)
                       {
                           objectClass = attribute[loopCount].ToString();
                       }
                       if (accountType == ACCOUNT_TYPE.USER_NORMAL_ACCOUNT)
                       {
                           Site.Assert.IsTrue(objectClass.Contains("user"),
                        "3.1.5.4.4 The objectClass database attribute MUST be updated with a value 'user' if the AccountType parameter is neither USER_WORKSTATION_TRUST_ACCOUNT nor USER_SERVER_TRUST_ACCOUNT.");
                       }
                       else
                       {
                           Site.Assert.IsTrue(objectClass.Contains("computer"),
                        "3.1.5.4.4 If the AccountType parameter is USER_WORKSTATION_TRUST_ACCOUNT or USER_SERVER_TRUST_ACCOUNT, use computer.");
                       }
                       break;
                   case "userAccountControl":
                       for (int loopCount = 0; loopCount < attribute.Count; loopCount++)
                       {
                           userAccountControl = UInt32.Parse(attribute[loopCount].ToString());
                       }
                       if (accountType == ACCOUNT_TYPE.USER_NORMAL_ACCOUNT)
                       {
                           Site.Assert.IsTrue(userAccountControl == (Utilities.UF_NORMAL_ACCOUNT | Utilities.UF_ACCOUNTDISABLE | Utilities.UF_PASSWD_NOTREQD),
                        "3.1.5.4.4 The userAccountControl attribute MUST be updated with a value: USER_NORMAL_ACCOUNT- UF_NORMAL_ACCOUNT|UF_ACCOUNTDISABLE.");
                           Site.Assert.IsTrue(userAccountControl == (Utilities.UF_NORMAL_ACCOUNT | Utilities.UF_ACCOUNTDISABLE | Utilities.UF_PASSWD_NOTREQD),
                        "3.1.1.8.1 If the value of the userAccountControl attribute in the database contains a bit that is specified in the following table, the userAccountControl attribute MUST be updated with the corresponding bit(s) using a bitwise OR.");
                       }
                       else if (accountType == ACCOUNT_TYPE.USER_WORKSTATION_TRUST_ACCOUNT)
                       {
                           Site.Assert.IsTrue(userAccountControl == (Utilities.UF_WORKSTATION_TRUST_ACCOUNT | Utilities.UF_ACCOUNTDISABLE | Utilities.UF_PASSWD_NOTREQD),
                        "3.1.5.4.4 The userAccountControl attribute MUST be updated with a value: USER_WORKSTATION_TRUST_ACCOUNT: UF_WORKSTATION_TRUST_ACCOUNT | UF_ACCOUNTDISABLE*.");
                       }
                       else if (accountType == ACCOUNT_TYPE.USER_SERVER_TRUST_ACCOUNT)
                       {
                           Site.Assert.IsTrue(userAccountControl == (Utilities.UF_SERVER_TRUST_ACCOUNT | Utilities.UF_ACCOUNTDISABLE | Utilities.UF_PASSWD_NOTREQD),
                        "3.1.5.4.4 The userAccountControl attribute MUST be updated with a value: USER_SERVER_TRUST_ACCOUNT: UF_SERVER_TRUST_ACCOUNT | UF_ACCOUNTDISABLE.");
                       }
                       break;
                   case "sAMAccountType":
                       uint sAMAccountType = 0;
                       for (int loopCount = 0; loopCount < attribute.Count; loopCount++)
                       {
                           sAMAccountType = UInt32.Parse(attribute[loopCount].ToString());
                       }
                       if (userAccountControl == (userAccountControl & Utilities.UF_NORMAL_ACCOUNT))
                       {
                           Site.Assert.AreEqual(Utilities.SAM_USER_OBJECT, sAMAccountType, "3.1.1.8.1 If the value of the userAccountControl attribute in the database contains a bit that is specified in the following table, the sAMAccountType attribute MUST be updated with the corresponding value.");
                       }
                       else if(userAccountControl == (userAccountControl & Utilities.UF_INTERDOMAIN_TRUST_ACCOUNT))
                       {
                           Site.Assert.AreEqual(Utilities.SAM_TRUST_ACCOUNT, sAMAccountType, "3.1.1.8.1 If the value of the userAccountControl attribute in the database contains a bit that is specified in the following table, the sAMAccountType attribute MUST be updated with the corresponding value.");
                       }
                       else if (userAccountControl == (userAccountControl & Utilities.UF_WORKSTATION_TRUST_ACCOUNT) || userAccountControl == (userAccountControl & Utilities.UF_SERVER_TRUST_ACCOUNT))
                       {
                           Site.Assert.AreEqual(Utilities.SAM_MACHINE_ACCOUNT, sAMAccountType, "3.1.1.8.1 If the value of the userAccountControl attribute in the database contains a bit that is specified in the following table, the sAMAccountType attribute MUST be updated with the corresponding value.");
                       }
                       break;
                   case "primaryGroupID":
                       uint primaryGroupID = 0;
                       for (int loopCount = 0; loopCount < attribute.Count; loopCount++)
                       {
                           primaryGroupID = Convert.ToUInt32(attribute[loopCount].ToString());
                       }
                       if ((userAccountControl == (userAccountControl & Utilities.UF_NORMAL_ACCOUNT)) || (userAccountControl == (userAccountControl & Utilities.UF_INTERDOMAIN_TRUST_ACCOUNT)))
                       {
                           Site.Assert.AreEqual(Utilities.DOMAIN_GROUP_RID_USERS, primaryGroupID,
                               "3.1.1.8.1 If the value of the userAccountControl attribute in the database contains a bit or bit combination that is specified in the following table, the primaryGroupId attribute MUST be updated with the corresponding value.");
                       }
                       else if (userAccountControl == (userAccountControl & Utilities.UF_WORKSTATION_TRUST_ACCOUNT))
                       {
                           Site.Assert.AreEqual(Utilities.DOMAIN_GROUP_RID_COMPUTERS, primaryGroupID,
                        "3.1.1.8.1 If the value of the userAccountControl attribute in the database contains a bit or bit combination that is specified in the following table, the primaryGroupId attribute MUST be updated with the corresponding value.");
                       }
                       else if (userAccountControl == (userAccountControl & Utilities.UF_SERVER_TRUST_ACCOUNT))
                       {
                           Site.Assert.AreEqual(Utilities.DOMAIN_GROUP_RID_CONTROLLERS, primaryGroupID,
                        "3.1.1.8.1 If the value of the userAccountControl attribute in the database contains a bit or bit combination that is specified in the following table, the primaryGroupId attribute MUST be updated with the corresponding value.");
                       }
                       else if ((userAccountControl == (userAccountControl & Utilities.UF_WORKSTATION_TRUST_ACCOUNT)) && (userAccountControl == (userAccountControl & Utilities.UF_PARTIAL_SECRETS_ACCOUNT)))
                       {
                           Site.Assert.AreEqual(Utilities.DOMAIN_GROUP_RID_READONLY_CONTROLLERS, primaryGroupID,
                        "3.1.1.8.1 If the value of the userAccountControl attribute in the database contains a bit or bit combination that is specified in the following table, the primaryGroupId attribute MUST be updated with the corresponding value.");
                       }
                       break;
                   case "badPwdCount":
                       int badpwdTime = 0;
                       for (int loopcount = 0; loopcount < attribute.Count; loopcount++)
                       {
                           badpwdTime = Convert.ToInt32(attribute[loopcount].ToString());

                       }
                       Site.Assert.AreEqual(0, badpwdTime, "3.1.1.8.1 badPwdCount MUST be updated with 0 if no value is present in the database.");
                       break;
                   case "codePage":
                       int codePage = 0;
                       for (int loopcount = 0; loopcount < attribute.Count; loopcount++)
                       {
                           codePage = Convert.ToInt32(attribute[loopcount].ToString());
                       }
                       Site.Assert.AreEqual(0, codePage, "3.1.1.8.1 codePage MUST be updated with 0 if no value is present in the database.");
                       break;
                   case "countryCode":
                       int countryCode = 0;
                       for (int loopCount = 0; loopCount < attribute.Count; loopCount++)
                       {
                           countryCode = Convert.ToInt32(attribute[loopCount].ToString());
                       }
                       Site.Assert.AreEqual(0, countryCode, "3.1.1.8.1 countryCode MUST be updated with 0 if no value is present in the database.");
                       break;
                   case "badPasswordTime":
                       long badPasswordTime = 0;
                       for (int loopcount = 0; loopcount < attribute.Count; loopcount++)
                       {
                           badPasswordTime = Convert.ToInt64(attribute[loopcount].ToString());
                       }
                       Site.Assert.AreEqual(0, badPasswordTime, "3.1.1.8.1 badPasswordTime MUST be updated with 0 if no value is present in the database.");
                       break;
                   case "lastLogoff":
                       long lastLogoff = 0;
                       for (int loopcount = 0; loopcount < attribute.Count; loopcount++)
                       {
                           lastLogoff = Convert.ToInt64(attribute[loopcount].ToString());
                       }
                       Site.Assert.AreEqual(0, lastLogoff, "3.1.1.8.1 lastLogoff MUST be updated with 0 if no value is present in the database.");
                       break;
                   case "lastLogon":
                       long lastLogon = 0;
                       for (int loopcount = 0; loopcount < attribute.Count; loopcount++)
                       {
                           lastLogon = Convert.ToInt64(attribute[loopcount].ToString());
                       }
                       Site.Assert.AreEqual(0, lastLogon, "3.1.1.8.1 lastLogon MUST be updated with 0 if no value is present in the database.");
                       break;
                   case "pwdLastSet":
                       long pwdLastSet = 0;
                       for (int loopcount = 0; loopcount < attribute.Count; loopcount++)
                       {
                           pwdLastSet = Convert.ToInt64(attribute[loopcount].ToString());
                       }
                       Site.Assert.AreEqual(0, pwdLastSet, "3.1.1.8.1 pwdLastSet MUST be updated with 0 if no value is present in the database.");
                       break;
                   case "accountExpires":
                       long accountExpires = 0;
                       for (int loopcount = 0; loopcount < attribute.Count; loopcount++)
                       {
                           accountExpires = Convert.ToInt64(attribute[loopcount].ToString());
                       }
                       Site.Assert.AreEqual(0x7FFFFFFFFFFFFFFF, accountExpires, "3.1.1.8.1 accountExpires MUST be updated with 0x7FFFFFFFFFFFFFFF if no value is present in the database.");
                       break;
                   case "logonCount":
                       int logonCount = 0;
                       for (int loopcount = 0; loopcount < attribute.Count; loopcount++)
                       {
                           logonCount = Convert.ToInt32(attribute[loopcount].ToString());
                       }
                       Site.Assert.AreEqual(0, logonCount, "3.1.1.8.1 logonCount MUST be updated with 0 if no value is present in the database.");
                       break;
                   default:
                       break;
               }
           }
           
       }

       /// <summary>
       /// Verify the constraints when the objectClass attribute value is group, or derived from this class.
       /// </summary>
       /// <param name="name">sAMAccountName</param>
       public void VerifyConstraintsForGroup(string name)
       {
           LdapConnection con = new LdapConnection(new LdapDirectoryIdentifier(_samrProtocolAdapter.pdcFqdn),
               new NetworkCredential(_samrProtocolAdapter.DomainAdministratorName, _samrProtocolAdapter.DomainUserPassword, _samrProtocolAdapter.PrimaryDomainDnsName));
           string filter = string.Format("(sAMAccountName={0})", name);
           SearchRequest request = new SearchRequest(_samrProtocolAdapter.primaryDomainDN, filter, System.DirectoryServices.Protocols.SearchScope.Subtree);
           SearchResponse response = (SearchResponse)con.SendRequest(request);
           Site.Assert.AreEqual(1, response.Entries.Count, "3.1.5.4.1 On successful completion of this method, a new database object MUST be created (subsequent constraints specify attributes for this new object).");

           uint groupType = 0;
           foreach (DirectoryAttribute attribute in response.Entries[0].Attributes.Values)
           {
               switch (attribute.Name)
               {
                   case "sAMAccountName":
                       string sAMAccountName = null;
                       for (int loopCount = 0; loopCount < attribute.Count; loopCount++)
                       {
                           sAMAccountName = attribute[loopCount].ToString();
                       }
                       Site.Assert.AreEqual<string>(name, sAMAccountName, "3.1.5.4.1 Common Processing for Group and Alias Creation: 6. sAMAccountName database attribute MUST be updated from the values provided in the message.");
                       break;
                   case "distinguishedName":
                       Site.Log.Add(LogEntryKind.Debug, "3.1.5.4.1 Common Processing for Group and Alias Creation: 7. distinguishedName database attribute MUST be updated with a value that conforms to the constraints as specified in section 3.1.5.14.1.");
                       Site.Log.Add(LogEntryKind.Debug, "3.1.5.14.1 The constraints refer to an AccountType parameter from the referring section; if the object being created has the objectClass of a group, there is no AccountType parameter in the message. In this case, use an Account Type value of USER_NORMAL_ACCOUNT.");
                       break;
                   case "objectClass":
                       string objectClass = null;
                       for (int loopCount = 0; loopCount < attribute.Count; loopCount++)
                       {
                           objectClass = attribute[loopCount].ToString();
                       }
                       Site.Assert.IsTrue(objectClass.Equals("group", StringComparison.InvariantCultureIgnoreCase),
                           "3.1.5.4.1 The objectClass database attribute MUST be updated with a value 'group'.");
                       break;
                   case "groupType":
                       for (int loopCount = 0; loopCount < attribute.Count; loopCount++)
                       {
                           groupType = (uint)Int32.Parse(attribute[loopCount].ToString());
                       }
                       Site.Assert.IsTrue(groupType == (groupType & Utilities.GROUP_TYPE_SECURITY_ACCOUNT),
                           "3.1.5.4.2 This method MUST be processed per the specifications in section 3.1.5.4.1, using a group type of GROUP_TYPE_SECURITY_ACCOUNT and using access mask values from section 2.2.1.5.");
                       Site.Assert.IsTrue(groupType == (groupType & Utilities.GROUP_TYPE_SECURITY_ACCOUNT), 
                           "3.1.5.4.1 The groupType database attribute MUST be updated with the value Provided-Group-Type (GROUP_TYPE_SECURITY_ACCOUNT).");
                       break;
                   case "sAMAccountType":
                       uint sAMAccountType = 0;
                       for (int loopCount = 0; loopCount < attribute.Count; loopCount++)
                       {
                           sAMAccountType = UInt32.Parse(attribute[loopCount].ToString());
                       }
                       if (groupType == (groupType & Utilities.GROUP_TYPE_SECURITY_ACCOUNT))
                       {
                           Site.Assert.AreEqual(Utilities.SAM_GROUP_OBJECT, sAMAccountType, "3.1.1.8.1 If the value of the groupType attribute in the database contains GROUP_TYPE_SECURITY_ACCOUNT, the sAMAccountType attribute MUST be updated with SAM_GROUP_OBJECT.");
                       }
                       else if (groupType == (groupType & Utilities.GROUP_TYPE_ACCOUNT_GROUP))
                       {
                           Site.Assert.AreEqual(Utilities.SAM_NON_SECURITY_GROUP_OBJECT, sAMAccountType, "3.1.1.8.1 If the value of the groupType attribute in the database contains GROUP_TYPE_ACCOUNT_GROUP, the sAMAccountType attribute MUST be updated with SAM_NON_SECURITY_GROUP_OBJECT.");
                       }
                       else if (groupType == (groupType & Utilities.GROUP_TYPE_SECURITY_RESOURCE))
                       {
                           Site.Assert.AreEqual(Utilities.SAM_ALIAS_OBJECT, sAMAccountType, "3.1.1.8.1 If the value of the groupType attribute in the database contains GROUP_TYPE_SECURITY_RESOURCE, the sAMAccountType attribute MUST be updated with SAM_ALIAS_OBJECT.");
                       }
                       else if (groupType == (groupType & Utilities.GROUP_TYPE_RESOURCE_GROUP))
                       {
                           Site.Assert.AreEqual(Utilities.SAM_NON_SECURITY_ALIAS_OBJECT, sAMAccountType, "3.1.1.8.1 If the value of the groupType attribute in the database contains GROUP_TYPE_RESOURCE_GROUP, the sAMAccountType attribute MUST be updated with SAM_NON_SECURITY_ALIAS_OBJECT.");
                       }
                       else if (groupType == (groupType & Utilities.GROUP_TYPE_SECURITY_UNIVERSAL))
                       {
                           Site.Assert.AreEqual(Utilities.SAM_GROUP_OBJECT, sAMAccountType, "3.1.1.8.1 If the value of the groupType attribute in the database contains GROUP_TYPE_SECURITY_UNIVERSAL, the sAMAccountType attribute MUST be updated with SAM_GROUP_OBJECT.");
                       }
                       else if (groupType == (groupType & Utilities.GROUP_TYPE_UNIVERSAL_GROUP))
                       {
                           Site.Assert.AreEqual(Utilities.SAM_NON_SECURITY_GROUP_OBJECT, sAMAccountType, "3.1.1.8.1 If the value of the groupType attribute in the database contains GROUP_TYPE_UNIVERSAL_GROUP, the sAMAccountType attribute MUST be updated with SAM_NON_SECURITY_GROUP_OBJECT.");
                       }
                       break;
                   default:
                       break;
               }
           }

       }

       /// <summary>
       /// Verify the domain general information returned by QueryInformationDomain
       /// </summary>
       /// <param name="domainInfo"></param>
       public void VerifyDomainGeneralInformationFields(_SAMPR_DOMAIN_GENERAL_INFORMATION? generalInfo)
       {
           Int64 uas = 0;
           string fSMORoleOwner = "";
           LdapConnection LdapCon = new LdapConnection(new LdapDirectoryIdentifier(PDCNetbiosName));
           SearchRequest searchRequest = new SearchRequest();
           searchRequest.DistinguishedName = primaryDomainDN;
           searchRequest.Scope = System.DirectoryServices.Protocols.SearchScope.Base;
           SearchResponse searchResponse = (SearchResponse)LdapCon.SendRequest(searchRequest);
           foreach (SearchResultEntry entry in searchResponse.Entries)
           {
               foreach (DirectoryAttribute attribute in entry.Attributes.Values)
               {
                   switch (attribute.Name)
                   {
                       case "uASCompat":
                           for (int loopcount = 0; loopcount < attribute.Count; loopcount++)
                           {
                               uas = Convert.ToInt64(attribute[loopcount]);
                           }
                           break;
                       case "fSMORoleOwner":
                           for (int loopcount = 0; loopcount < attribute.Count; loopcount++)
                           {
                               fSMORoleOwner = attribute[loopcount].ToString();
                           }
                           break;
                       default:
                           break;
                   }
               }
           }        
           LdapCon.Dispose();
           Site.Assert.AreEqual(_DOMAIN_SERVER_ENABLE_STATE.DomainServerEnabled, generalInfo.Value.DomainServerState, "3.1.5.5.1.1 The Buffer.General.DomainServerState field MUST be set to DomainStateEnabled.");
           if (isDC)
           {
               if (fSMORoleOwner == pdcComputerObjectDN)
               {
                   Site.Assert.AreEqual(_DOMAIN_SERVER_ROLE.DomainServerRolePrimary, generalInfo.Value.DomainServerRole,
                       "3.1.5.5.1.1 If the server is a DC and the fsmoRoleOwner attribute value of the account domain object is equal to the distinguishedName attribute value of the server's computer object, the Buffer.General.DomainServerRole field MUST be set to DomainServerRolePrimary.");

               }
               else
               {
                   Site.Assert.AreEqual(_DOMAIN_SERVER_ROLE.DomainServerRoleBackup, generalInfo.Value.DomainServerRole, "3.1.5.5.1.1 Otherwise, the Buffer.General.DomainServerRole field MUST be set to DomainServerRoleBackup.");
               }
           }
           else
           {
               Site.Assert.AreEqual(_DOMAIN_SERVER_ROLE.DomainServerRolePrimary, generalInfo.Value.DomainServerRole, "3.1.5.5.1.1 If the server is not a domain controller (DC), the Buffer.General.DomainServerRole field MUST be set to DomainServerRolePrimary.");
           }
           if (uas > 0)
           {
               Site.Assert.AreEqual(1, generalInfo.Value.UasCompatibilityRequired, "3.1.5.5.1.1 Buffer.General.UasCompatibilityRequired MUST be set to 1 if the uASCompat database attribute value on the domain object is nonzero.");
           }
       }

       /// <summary>
       /// Verify the domain server role information returned by QueryInformationDomain
       /// </summary>
       /// <param name="domainInfo"></param>
       public void VerifyDomainServerRoleInformationFields(_DOMAIN_SERVER_ROLE_INFORMATION? roleInfo)
       {
           string fSMORoleOwner = "";
           LdapConnection LdapCon = new LdapConnection(new LdapDirectoryIdentifier(PDCNetbiosName));
           SearchRequest searchRequest = new SearchRequest();
           searchRequest.DistinguishedName = primaryDomainDN;
           searchRequest.Scope = System.DirectoryServices.Protocols.SearchScope.Base;
           SearchResponse searchResponse = (SearchResponse)LdapCon.SendRequest(searchRequest);
           foreach (SearchResultEntry entry in searchResponse.Entries)
           {
               foreach (DirectoryAttribute attribute in entry.Attributes.Values)
               {
                   if (attribute.Name == "fSMORoleOwner")
                   {
                       for (int loopcount = 0; loopcount < attribute.Count; loopcount++)
                       {
                           fSMORoleOwner = attribute[loopcount].ToString();
                       }
                   }
               }
           }
           LdapCon.Dispose();
           if (isDC)
           {
               if (fSMORoleOwner == pdcComputerObjectDN)
               {
                   Site.Assert.AreEqual(_DOMAIN_SERVER_ROLE.DomainServerRolePrimary, roleInfo.Value.DomainServerRole,
                       "3.1.5.5.1.2 If the server is a DC and the fsmoRoleOwner attribute value of the account domain object is equal to the distinguishedName attribute value of the server's computer object, the Buffer.Role.DomainServerRole field MUST be set to DomainServerRolePrimary.");

               }
               else
               {
                   Site.Assert.AreEqual(_DOMAIN_SERVER_ROLE.DomainServerRoleBackup, roleInfo.Value.DomainServerRole, "3.1.5.5.1.2 Otherwise, the Buffer.Role.DomainServerRole field MUST be set to DomainServerRoleBackup.");
               }
           }
           else
           {
               Site.Assert.AreEqual(_DOMAIN_SERVER_ROLE.DomainServerRolePrimary, roleInfo.Value.DomainServerRole, "3.1.5.5.1.2 If the server is not a domain controller (DC), the Buffer.Role.DomainServerRole field MUST be set to DomainServerRolePrimary.");
           }
       }

       /// <summary>
       /// Verify the domain lockout information returned by QueryInformationDomain
       /// </summary>
       /// <param name="domainInfo"></param>
       public void VerifyDomainLockoutInformationFields(_SAMPR_DOMAIN_LOCKOUT_INFORMATION? lockoutInfo)
       {
           long lockoutDuration = 1;
           long lockoutObservationWindow = 1;
           ushort lockoutThreshold = 1; 
           LdapConnection LdapCon = new LdapConnection(new LdapDirectoryIdentifier(PDCNetbiosName));
           SearchRequest searchRequest = new SearchRequest();
           searchRequest.DistinguishedName = primaryDomainDN;
           searchRequest.Scope = System.DirectoryServices.Protocols.SearchScope.Base;
           SearchResponse searchResponse = (SearchResponse)LdapCon.SendRequest(searchRequest);
           foreach (SearchResultEntry entry in searchResponse.Entries)
           {
               foreach (DirectoryAttribute attribute in entry.Attributes.Values)
               {
                   
                   switch (attribute.Name)
                   {
                       case "lockoutDuration":
                           for (int loopcount = 0; loopcount < attribute.Count; loopcount++)
                           {
                               lockoutDuration = Convert.ToInt64(attribute[loopcount]);
                           }
                           break;
                       case "lockOutObservationWindow":
                           for (int loopcount = 0; loopcount < attribute.Count; loopcount++)
                           {
                               lockoutObservationWindow = Convert.ToInt64(attribute[loopcount]);
                           }
                           break;
                       case "lockoutThreshold":
                           for (int loopcount = 0; loopcount < attribute.Count; loopcount++)
                           {
                               lockoutThreshold = Convert.ToUInt16(attribute[loopcount]);
                           }
                           break;
                       default:
                           break;
                   }
               }
           }
           LdapCon.Dispose();
           long temp = DtypUtility.ToInt64(lockoutInfo.Value.LockoutDuration);
           Site.Assert.AreEqual(lockoutDuration, temp,
               "lockoutDuration: 3.1.5.5.1 The following information levels MUST be processed by setting the appropriate output field name to the associated database attribute, as specified in section 3.1.5.14.8.");
           temp = DtypUtility.ToInt64(lockoutInfo.Value.LockoutObservationWindow);
           Site.Assert.AreEqual(lockoutObservationWindow, temp,
               "lockOutObservationWindow: 3.1.5.5.1 The following information levels MUST be processed by setting the appropriate output field name to the associated database attribute, as specified in section 3.1.5.14.8.");
           Site.Assert.AreEqual(lockoutThreshold, lockoutInfo.Value.LockoutThreshold,
               "lockoutThreshold: 3.1.5.5.1 The following information levels MUST be processed by setting the appropriate output field name to the associated database attribute, as specified in section 3.1.5.14.8.");
       }
    }
}
