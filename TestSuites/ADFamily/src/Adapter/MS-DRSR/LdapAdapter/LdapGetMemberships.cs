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

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    public partial class LdapAdapter : ManagedAdapterBase, ILdapAdapter
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1307:SpecifyStringComparison", MessageId = "System.String.CompareTo(System.String)")]
        static int CompareDsNames(DSNAME a, DSNAME b)
        {
            string na = LdapUtility.ConvertUshortArrayToString(a.StringName);
            string nb = LdapUtility.ConvertUshortArrayToString(b.StringName);
            return na.CompareTo(nb);
        }

        static GroupTypeFlags[] ParseGroupType(uint groupType)
        {
            List<GroupTypeFlags> grpTypes = new List<GroupTypeFlags>();
            if ((groupType & (uint)GroupTypeFlags.BUILTIN_LOCAL_GROUP) > 0)
                grpTypes.Add(GroupTypeFlags.BUILTIN_LOCAL_GROUP);
            if ((groupType & (uint)GroupTypeFlags.ACCOUNT_GROUP) > 0)
                grpTypes.Add(GroupTypeFlags.ACCOUNT_GROUP);
            if ((groupType & (uint)GroupTypeFlags.RESOURCE_GROUP) > 0)
                grpTypes.Add(GroupTypeFlags.RESOURCE_GROUP);
            if ((groupType & (uint)GroupTypeFlags.UNIVERSAL_GROUP) > 0)
                grpTypes.Add(GroupTypeFlags.UNIVERSAL_GROUP);
            if ((groupType & (uint)GroupTypeFlags.APP_BASIC_GROUP) > 0)
                grpTypes.Add(GroupTypeFlags.APP_BASIC_GROUP);
            if ((groupType & (uint)GroupTypeFlags.APP_QUERY_GROUP) > 0)
                grpTypes.Add(GroupTypeFlags.APP_QUERY_GROUP);
            if ((groupType & (uint)GroupTypeFlags.SECURITY_ENABLED) > 0)
                grpTypes.Add(GroupTypeFlags.SECURITY_ENABLED);

            return grpTypes.ToArray();
        }

        static bool FilterGroupOperationType(REVERSE_MEMBERSHIP_OPERATION_TYPE opType, uint groupType)
        {
            GroupTypeFlags[] grpTypes = ParseGroupType(groupType);

            // filterout built-in groups
            if (opType == REVERSE_MEMBERSHIP_OPERATION_TYPE.RevMembGetAliasMembership
                || opType == REVERSE_MEMBERSHIP_OPERATION_TYPE.RevMembGetResourceGroups)
            {
                if ((groupType & (uint)GroupTypeFlags.BUILTIN_LOCAL_GROUP) > 0)
                    return false;
            }

            switch (opType)
            {
                case REVERSE_MEMBERSHIP_OPERATION_TYPE.RevMembGetGroupsForUser:
                    {
                        // Nontransitive membership in groups that are confined to a given domain, 
                        // excluding built-in groups and domain-local groups (resource groups). 
                        if ((groupType & (uint)GroupTypeFlags.RESOURCE_GROUP) == 0)
                            return true;
                        return false;
                    }
                case REVERSE_MEMBERSHIP_OPERATION_TYPE.RevMembGetAliasMembership:
                    {
                        // Nontransitive membership in domain-local groups that are confined to a given domain.
                        if ((groupType & (uint)GroupTypeFlags.RESOURCE_GROUP) > 0)
                            return true;
                        return false;
                    }
                case REVERSE_MEMBERSHIP_OPERATION_TYPE.RevMembGetAccountGroups:
                    {
                        // Transitive membership in all account groups in a given domain, 
                        // excluding built-in groups.
                        if ((groupType & (uint)GroupTypeFlags.ACCOUNT_GROUP) > 0)
                            return true;
                        return false;
                    }
                case REVERSE_MEMBERSHIP_OPERATION_TYPE.RevMembGetResourceGroups:
                    {
                        // Transitive membership in all domain-local groups in a given domain, 
                        // excluding built-in groups.
                        if ((groupType & (uint)GroupTypeFlags.RESOURCE_GROUP) > 0)
                            return true;
                        return false;
                    }
                case REVERSE_MEMBERSHIP_OPERATION_TYPE.RevMembGetUniversalGroups:
                    {
                        // Transitive membership in all universal groups, excluding built-in groups.
                        if ((groupType & (uint)GroupTypeFlags.UNIVERSAL_GROUP) > 0)
                            return true;
                        return false;
                    }
                case REVERSE_MEMBERSHIP_OPERATION_TYPE.GroupMembersTransitive:
                    {
                        // Transitive closure of members of a group based on the information 
                        // present in the server's NC replicas, including the primary group.
                        throw new NotImplementedException();
                    }
                case REVERSE_MEMBERSHIP_OPERATION_TYPE.RevMembGlobalGroupsNonTransitive:
                    {
                        // Non-transitive membership in global groups, excluding built-in groups.
                        if ((groupType & (uint)GroupTypeFlags.ACCOUNT_GROUP) > 0)
                            return true;
                        return false;
                    }
                default:
                    return false;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.UInt32.ToString")]
        public DSNAME[] GetGroupMembersTransitive(
            DsServer dc,
            DSNAME name)
        {
            string baseDn = LdapUtility.ConvertUshortArrayToString(
                ((AddsDomain)dc.Domain).DomainNC.StringName
                );

            List<DSNAME> names = new List<DSNAME>();

            // primary groups
            uint pgId = 0;
            for (int i = 0; i < 4; ++i)
            {
                pgId <<= 8;
                pgId += name.Sid.Data[name.SidLen - i - 1];
            }

            SearchResultEntryCollection results = null;
            DirectoryControlCollection cc = new DirectoryControlCollection();
            cc.Add(new SearchOptionsControl(SearchOption.DomainScope));
            ResultCode r = ControlledSearch(
                dc,
                baseDn,
                "(primaryGroupId=" + pgId.ToString() + ")",
                System.DirectoryServices.Protocols.SearchScope.Subtree,
                null,
                cc,
                out results);

            if (r == ResultCode.Success)
            {
                foreach (SearchResultEntry e in results)
                {
                    string obj = e.DistinguishedName;
                    names.Add(LdapUtility.CreateDSNameForObject(dc, obj));
                }
            }

            string[] members = LdapUtility.GetAttributeValuesString(
                dc,
                LdapUtility.ConvertUshortArrayToString(name.StringName),
                "member");

            if (members != null)
            {
                foreach (string m in members)
                {
                    names.Add(LdapUtility.CreateDSNameForObject(dc, m));
                }
            }

            if (names.Count > 0)
            {
                List<DSNAME> tn = new List<DSNAME>();
                foreach (DSNAME n in names)
                {
                    // Recursively find all members of the member.
                    DSNAME[] ns = GetGroupMembersTransitive(dc, n);
                    if (ns != null)
                        tn.AddRange(ns);
                }
                if (tn.Count > 0)
                    names.AddRange(tn);

                // Remove duplicates
                Comparison<DSNAME> cmp = new Comparison<DSNAME>(CompareDsNames);
                names.Sort(cmp);

                int i = 0;
                while (i < names.Count - 1)
                {
                    string na = LdapUtility.ConvertUshortArrayToString(names[i].StringName);
                    string nb = LdapUtility.ConvertUshortArrayToString(names[i + 1].StringName);
                    if (na == nb)
                        names.RemoveAt(i);
                    else
                        ++i;
                }
                return names.ToArray();
            }

            return null;
        }


        /// <summary>
        /// Get group memberships for a given user.
        /// </summary>
        /// <param name="dc">The DC to talk to.</param>
        /// <param name="names">the DSName of the object whose reverse membership is being requested.</param>
        /// <param name="operationType">Type of evaluation.</param>
        /// <param name="limitingDomain">Domain filter.</param>
        /// <returns>The filtered group membership.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.Convert.ToInt32(System.Object)"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.Convert.ToInt32(System.String)")]
        public DSNAME[] GetMemberships(
            DsServer dc,
            DSNAME names,
            REVERSE_MEMBERSHIP_OPERATION_TYPE operationType,
            DSNAME? limitingDomain)
        {
            string baseDn = LdapUtility.ConvertUshortArrayToString(
                ((AddsDomain)dc.Domain).DomainNC.StringName
                );

            string name = LdapUtility.ConvertUshortArrayToString(names.StringName);
            List<DSNAME> results = new List<DSNAME>();

            // GroupMembersTransitive
            if (operationType == REVERSE_MEMBERSHIP_OPERATION_TYPE.GroupMembersTransitive)
            {
                return GetGroupMembersTransitive(dc, names);
            }

            // Get the primary group first
            DSNAME primaryGroup = LdapUtility.GetPrimaryGroup(dc, name, baseDn).Value;
            uint groupType = (uint)Convert.ToInt32(
                LdapUtility.GetAttributeValueInString(
                    dc,
                    LdapUtility.ConvertUshortArrayToString(primaryGroup.StringName),
                    "groupType")
                );

            if (FilterGroupOperationType(operationType, groupType))
            {
                // Valid primary group
                results.Add(primaryGroup);
            }


            string[] memberOfs = LdapUtility.GetAttributeValuesString(dc, name, "memberOf");
            foreach (string grpName in memberOfs)
            {
                // Get groupType and filter using the filters
                groupType = (uint)Convert.ToInt32(
                    LdapUtility.GetAttributeValueInString(dc, grpName, "groupType")
                    );

                if (!FilterGroupOperationType(operationType, groupType))
                    continue;

                results.Add(LdapUtility.CreateDSNameForObject(dc, grpName));
            }
            return results.ToArray();
        }
    }
}
