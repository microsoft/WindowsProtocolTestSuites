// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.Protocols;
using System.IO;
using System.Security.Principal;
using System.Text;

using Microsoft.Modeling;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocol.TestSuites.ActiveDirectory.Adts.Schema
{
    /// <summary>
    /// Partial class for DataSchemaTestSuite
    /// This file is the source file for Validation of the TestCase30.
    /// </summary>
    public partial class DataSchemaTestSuite
    {
        #region ReadOnlyDomainController Implementation.

        /// <summary>
        /// This method validates the requirements under 
        /// ReadOnlyDomainController.
        /// </summary>
        public void ValidateReadOnlyDomainController()
        {
            SearchResponse rodcEntries = null;
            bool isRodcEntry = false;
            string distinguishedName = "CN="
                + adAdapter.RODCNetbiosName
                + ",OU=Domain Controllers,"
                + adAdapter.rootDomainDN;

            //System.DirectoryServices.Protocols.SearchScope 
            GetLDAPObject(
                distinguishedName,
                adAdapter.PDCNetbiosName, 
                "objectclass=computer", 
                System.DirectoryServices.Protocols.SearchScope.Base, 
                new string[] { "objectclass", "userAccountControl", "primaryGroupId", "servicePrincipalName", 
                    "dNSHostName", "msDS-RevealedUsers", "msDS-NeverRevealGroup", "msDS-RevealOnDemandGroup", 
                    "msDS-KrbTgtLink", "managedBy" }, 
               out rodcEntries);
            if (rodcEntries != null)
            {
                isRodcEntry = true;
                SearchResultEntry rodcEntry = rodcEntries.Entries[0];
                SearchResultAttributeCollection attributeCollection = rodcEntry.Attributes;
                string expected = String.Empty, actual = String.Empty;

                foreach (DirectoryAttribute attribute in attributeCollection.Values)
                {
                    #region Capturing MS-AD_Schema_R913

                    if (attribute.Name.ToLower().Equals("objectclass"))
                    {
                        string[] values = (string[])attribute.GetValues(expected.GetType());
                        expected = "computer";
                        actual = values[values.Length - 1];

                        DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                            expected, 
                            actual, 
                            913,
                            @"Read-Only Domain controller object's objectClass is computer.");
                        DataSchemaSite.CaptureRequirementIfIsTrue(
                            expected == actual && isRodcEntry,
                            912,
                            @"Each RODC in a domain has a read-only domain controller object in its default NC. The 
                            DC's RODC object is the DC's computer object with additional requirements as described in 
                            this section. ");
                        continue;
                    }

                    #endregion

                    #region Capturing MS-AD_Schema_R914

                    if (attribute.Name.ToLower().Equals("useraccountcontrol"))
                    {
                        int value = int.Parse(attribute[0].ToString());
                        int userAccControlFlag = ParseUserAccountControlValue("ADS_UF_PARTIAL_SECRETS_ACCOUNT|ADS_UF_WORKSTATION_TRUST_ACCOUNT");
                        bool isR914Satisfied = ((value & userAccControlFlag) == userAccControlFlag);

                        DataSchemaSite.CaptureRequirementIfIsTrue(
                            isR914Satisfied, 
                            914,
                            @"Read-Only Domain controller object's userAccountControl is
                            {ADS_UF_PARTIAL_SECRETS_ACCOUNT | ADS_UF_WORKSTATION_TRUST_ACCOUNT}");
                        continue;
                    }

                    #endregion

                    #region Capturing MS-AD_Schema_R915, MS-AD_Schema_R916

                    if (attribute.Name.ToLower().Equals("primarygroupid"))
                    {
                        int value = int.Parse(attribute[0].ToString());
                        expected = "521";
                        actual = value.ToString();

                        DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                            expected, 
                            actual, 
                            915,
                            @"Read-Only Domain controller object's primaryGroupId is 521.");

                        if (serverOS >= OSVersion.WinSvr2008)
                        {
                            string dn = "CN=Read-only Domain Controllers,CN=Users," + adAdapter.rootDomainDN;
                            DirectoryEntry rodcGroupEntry = null;
                            adAdapter.GetObjectByDN(dn, out rodcGroupEntry);
                            SecurityIdentifier sid = new SecurityIdentifier((byte[])rodcGroupEntry.Properties["objectsid"].Value, 0);
                            expected = sid.ToString().Substring(sid.ToString().LastIndexOf('-') + 1);
                            actual = value.ToString();

                            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                                expected, 
                                actual, 
                                916,
                                @"primaryGroupId attribute is populated during creation of the RODC corresponding to
                                the RODC object. The primary group of an RODC object is the domain relative well-known 
                                Read-Only Domain Controllers security group. So the primaryGroupID attribute of an
                                RODC object equals the RID of the Read-Only Domain Controllers security group, 521.");                            
                        }
                        continue;
                    }

                    #endregion

                    #region Capturing MS-AD_Schema_R917

                    if (attribute.Name.ToLower().Equals("serviceprincipalname"))
                    {
                        string[] values = (string[])attribute.GetValues(expected.GetType());
                        //For windows 2008 SP1, it's same with windows server 2008 r2
                        expected = "host/"
                            + adAdapter.RODCNetbiosName 
                            + "." 
                            + adAdapter.PrimaryDomainDnsName 
                            + "/" 
                            + adAdapter.PrimaryDomainDnsName;
                        expected = expected.ToLower();
                        for (int i = 0; i < values.Length; i++)
                        {
                            if (expected.Equals(values[i].ToLower()))
                            {
                                actual = values[i].ToLower();
                                break;
                            }
                        }
                        DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                            expected, 
                            actual, 
                            917,
                            @"Read-Only Domain Controllers servicePrincipalName is contains all of
                            the SPNs for a normal (not read-only) DC.");
                        continue;
                    }

                    #endregion

                    #region Capturing MS-AD_Schema_R918

                    if (attribute.Name.ToLower().Equals("dnshostname"))
                    {
                        expected = adAdapter.RODCNetbiosName.ToLower() + "." + adAdapter.PrimaryDomainDnsName.ToLower();
                        
                        actual = attribute[0].ToString().ToLower();

                        DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                            expected, 
                            actual, 
                            918, 
                            @"Read-Only Domain Controller object's dNSHostName equals fully qualified DNS name of the RODC.");
                        continue;
                    }

                    #endregion

                    #region Capturing MS-AD_Schema_R920

                    if (attribute.Name.ToLower().Equals("msds-revealedusers"))
                    {
                        SearchResponse userResponse = null;
                        GetLDAPObject(
                            "CN=Users,"
                            + adAdapter.rootDomainDN,
                            adAdapter.PDCNetbiosName, 
                            "objectclass=user", 
                            System.DirectoryServices.Protocols.SearchScope.OneLevel, 
                            new string[] { "distinguishedname" }, 
                            out userResponse);
                        foreach (SearchResultEntry ent in userResponse.Entries)
                        {
                            if (ent.DistinguishedName.Contains("krbtgt_"))
                            {
                                expected = ent.DistinguishedName;
                                break;
                            }
                        }
                        actual = attribute[0].ToString();

                        bool isR920Satisfied = actual.Contains(expected);

                        DataSchemaSite.CaptureRequirementIfIsTrue(
                            isR920Satisfied, 
                            920, 
                            @"Read-Only Domain Controller object's msDS-RevealedUsers attribute Contains information 
                            about the user objects whose secret attributes are cached at this RODC. This attribute is 
                            maintained by the system; see procedure UpdateRevealedList.A more usable form of this 
                            attribute is the constructed attribute msDS-RevealedList.");
                        continue;
                    }

                    #endregion

                    #region Capturing MS-AD_Schema_R922

                    if (attribute.Name.ToLower().Equals("msds-neverrevealgroup"))
                    {
                        string[] values = (string[])attribute.GetValues(expected.GetType());

                        SearchResponse groupResponse = null;
                        GetLDAPObject(
                            "CN=Users," 
                            + adAdapter.rootDomainDN,
                            adAdapter.PDCNetbiosName, 
                            "objectclass=group", 
                            System.DirectoryServices.Protocols.SearchScope.OneLevel, 
                            new string[] { "distinguishedname" }, 
                            out groupResponse);
                        foreach (SearchResultEntry ent in groupResponse.Entries)
                        {
                            if (ent.DistinguishedName.Contains("Denied RODC Password"))
                            {
                                expected = ent.DistinguishedName;
                                break;
                            }
                        }
                        bool isR922Satisfied = false;
                        for (int i = 0; i < values.Length; i++)
                        {
                            if (values[i].Equals(expected))
                            {
                                isR922Satisfied = true;
                                break;
                            }
                        }
                        DataSchemaSite.CaptureRequirementIfIsTrue(
                            isR922Satisfied, 
                            922, 
                            @"Read-Only Domain Controller object's 
                            msDS-NeverRevealGroup attribute is maintained by an administrator. It contains a set of user 
                            and security-enabled group objects. A user in this set, or reachable from this set by 
                            traversing any number of member links from a group in this set, will not change state from 
                            not being cached to being cached at this RODC. If a user is added to this attribute 
                            (directly or indirectly) while one of its secret attributes is already cached, the secret 
                            attribute remains cached until the secret attribute changes, at which time the caching 
                            stops. For the use of this attribute, see procedure RevealSecretsForUserAllowed.");
                        continue;
                    }

                    #endregion

                    #region Capturing MS-AD_Schema_R923

                    if (attribute.Name.ToLower().Equals("msds-revealondemandgroup"))
                    {
                        string[] values = (string[])attribute.GetValues(expected.GetType());

                        SearchResponse groupResponse = null;
                        GetLDAPObject(
                            "CN=Users," 
                            + adAdapter.rootDomainDN,
                            adAdapter.PDCNetbiosName, 
                            "objectclass=group", 
                            System.DirectoryServices.Protocols.SearchScope.OneLevel, 
                            new string[] { "distinguishedname" }, 
                            out groupResponse);
                        foreach (SearchResultEntry ent in groupResponse.Entries)
                        {
                            if (ent.DistinguishedName.Contains("Allowed RODC Password"))
                            {
                                expected = ent.DistinguishedName;
                                break;
                            }
                        }
                        bool isR923Satisfied = false;
                        for (int i = 0; i < values.Length; i++)
                        {
                            if (values[i].Equals(expected))
                            {
                                isR923Satisfied = true;
                                break;
                            }
                        }
                        DataSchemaSite.CaptureRequirementIfIsTrue(
                            isR923Satisfied, 
                            923, 
                            @"Read-Only Domain Controller object's 
                            msDS-RevealOnDemandGroup attribute is maintained by an administrator. It contains a set of 
                            user and security-enabled group objects. A user in this set, or reachable from this set by 
                            traversing any number of member links from a group in this set, and not excluded by 
                            membership in msDS-NeverRevealGroup can change state from not being cached to being cached 
                            at this RODC. For the use of this attribute see procedure RevealSecretsForUserAllowed.");
                        continue;
                    }

                    #endregion

                    #region Capturing MS-AD_Schema_R924

                    if (attribute.Name.ToLower().Equals("msds-krbtgtlink"))
                    {
                        string[] values = (string[])attribute.GetValues(expected.GetType());

                        SearchResponse userResponse = null;
                        GetLDAPObject(
                            "CN=Users," 
                            + adAdapter.rootDomainDN,
                            adAdapter.PDCNetbiosName, 
                            "objectclass=user", 
                            System.DirectoryServices.Protocols.SearchScope.OneLevel, 
                            new string[] { "distinguishedname" }, 
                            out userResponse);
                        foreach (SearchResultEntry ent in userResponse.Entries)
                        {
                            if (ent.DistinguishedName.Contains("krbtgt_"))
                            {
                                expected = ent.DistinguishedName;
                                break;
                            }
                        }
                        bool isR924Satisfied = false;
                        for (int i = 0; i < values.Length; i++)
                        {
                            if (values[i].Equals(expected))
                            {
                                isR924Satisfied = true;
                                break;
                            }
                        }
                        DataSchemaSite.CaptureRequirementIfIsTrue(
                            isR924Satisfied, 
                            924, 
                            @"Read-Only Domain Controllers objects 
                            msDS-KrbTgtLink attribute is populated during creation of the RODC object. It contains a 
                            reference to the RODC's secondary Kerberos ticket-granting ticket account.");
                        continue;
                    }

                    #endregion

                    #region Capturing MS-AD_Schema_R925

                    if (attribute.Name.ToLower().Equals("managedby"))
                    {
                        actual = (string)attribute[0];

                        SearchResponse userResponse = null;
                        GetLDAPObject(
                            "CN=Users," 
                            + adAdapter.rootDomainDN,
                            adAdapter.PDCNetbiosName, 
                            "(|(objectclass=user)(objectclass=group))", 
                            System.DirectoryServices.Protocols.SearchScope.OneLevel, 
                            new string[] { "distinguishedname" }, 
                            out userResponse);
                        bool isR925Satisfied = false;
                        foreach (SearchResultEntry ent in userResponse.Entries)
                        {
                            if (ent.DistinguishedName.Contains("Admin") && ent.DistinguishedName.Equals(actual))
                            {
                                isR925Satisfied = true;
                                break;
                            }
                        }
                        DataSchemaSite.CaptureRequirementIfIsTrue(
                            isR925Satisfied,
                            925, 
                            @"Read-Only Domain Controller object's 
                            managedBy attribute If the value of this attribute points to a valid security principal, 
                            that security principal will be an implicit member of the administrators group of this RODC. 
                            This applies to this RODC only.");
                        continue;
                    }

                    #endregion
                }
            }
        }

        #endregion
    }
}