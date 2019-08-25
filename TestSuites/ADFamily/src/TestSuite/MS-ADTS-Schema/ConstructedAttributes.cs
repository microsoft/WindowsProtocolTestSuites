// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using ActiveDs;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.Protocols;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;

using Microsoft.Modeling;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;

namespace Microsoft.Protocol.TestSuites.ActiveDirectory.Adts.Schema
{
    /// <summary>
    /// Partial class for DataSchemaTestSuite
    /// This file is the source file for Validation of the TestCase18 and Testcase19.
    /// </summary>
    public partial class DataSchemaTestSuite
    {
        #region ConstructedAttributes Validation.
        /// <summary>
        /// This method validates the requirements under 
        /// ConstructedAttributes Scenario.
        /// </summary>
        public void ValidateConstructedAttributes()
        {

            #region common Variables

            //According to TD, FLAG_ATTR_IS_CONSTRUCT is value 4
            int attrIsConstructFlag = 4;
            string schemaNC = "CN=Schema,CN=configuration," + adAdapter.rootDomainDN;
            bool isSystemFlagval = true;
            bool isRootDseAttribute = true;
            bool isBacklinkattribute = true;
            bool isParentGUIDEqualToObjectGUID = true;
            string[] serverValue = null;
            string[] actualValue = null;
            string distinguishedName = String.Empty;
            string sidValues = String.Empty;
            string hostOrDomainName = adAdapter.PrimaryDomainDnsName;
            string targetOu = adAdapter.rootDomainDN, rootNC, quotaEntryValue, getSidValue, tokenGroup, tempString = String.Empty;
            byte[] objectSid;
            int quotaUsed = -1, liveCount = -1, tombstoneCount = -1, wrongLocation = -1;
            ArrayList storeSid = new ArrayList();
            AttributeContext attrContext;
            List<string> sampleClasses = new List<string>();
            SecurityIdentifier sid;
            PropertyValueCollection msDSTombQuotaFactor, msDSQuotaEffective, sidValue;
            IADsSecurityDescriptor sd = null;
            IADsAccessControlList dacl = null;
            bool isSuccess = false;
           
            //Directory Entry for holding the objects required.
            DirectoryEntry dirEntry = new DirectoryEntry();

            #endregion

            #region Before searching msDS-QuotaControl Need to create an object.

            try
            {
                string serverName = adAdapter.PDCNetbiosName;

                if (serverOS >= OSVersion.WinSvr2008R2)
                {
                    serverName += "." + adAdapter.PrimaryDomainDnsName;
                }
                LdapConnection conn = new LdapConnection(new LdapDirectoryIdentifier(serverName));
                conn.Bind();
                AddRequest request = new AddRequest();
                request.DistinguishedName = "CN=schemaFour,CN=NTDS Quotas,CN=Configuration," + adAdapter.rootDomainDN;
                request.Attributes.Add(new DirectoryAttribute("objectClass", "msDS-QuotaControl"));
                request.Attributes.Add(new DirectoryAttribute("msDS-QuotaTrustee", "S-1-5-21-1046346730-3166354421-3723927699-500"));
                request.Attributes.Add(new DirectoryAttribute("msDS-QuotaAmount", "5"));
                AddResponse response = (AddResponse)conn.SendRequest(request);
            }
            catch
            {
                DataSchemaSite.Assert.IsTrue(true, "Object already exists");
            }

            #endregion

            #region PasswordSettingsContainer

            try
            {
                string serverName = adAdapter.PDCNetbiosName;

                if (serverOS >= OSVersion.WinSvr2008R2)
                {
                    serverName += "." + adAdapter.PrimaryDomainDnsName;
                }
                LdapConnection connectionPSC = new LdapConnection(new LdapDirectoryIdentifier(serverName));
                connectionPSC.Bind();
                AddRequest request = new AddRequest();
                request.DistinguishedName = "CN=testpassword,CN=Password Settings Container,CN=System," + adAdapter.rootDomainDN;
                request.Attributes.Add(new DirectoryAttribute("objectClass", "msDS-PasswordSettings"));
                request.Attributes.Add(new DirectoryAttribute("msDS-PasswordSettingsPrecedence", "1"));
                request.Attributes.Add(new DirectoryAttribute("msDS-PasswordReversibleEncryptionEnabled", "FALSE"));
                request.Attributes.Add(new DirectoryAttribute("msDS-PasswordHistoryLength", "100"));
                request.Attributes.Add(new DirectoryAttribute("msDS-PasswordComplexityEnabled", "FALSE"));
                request.Attributes.Add(new DirectoryAttribute("msDS-MinimumPasswordLength", "56"));
                request.Attributes.Add(new DirectoryAttribute("msDS-MinimumPasswordAge", "-10"));
                request.Attributes.Add(new DirectoryAttribute("msDS-MaximumPasswordAge", "-9223372036854775808"));
                request.Attributes.Add(new DirectoryAttribute("msDS-LockoutThreshold", "1"));
                request.Attributes.Add(new DirectoryAttribute("msDS-LockoutObservationWindow", "-10"));
                request.Attributes.Add(new DirectoryAttribute("msDS-LockoutDuration", "-20"));
                AddResponse response = (AddResponse)connectionPSC.SendRequest(request);

            }
            catch (Exception ex)
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, ex.Message);
            }

            #endregion

            #region make list Of RootDseAttribute Names

            List<string> RootDseAttribute = new List<string>();
            RootDseAttribute.Add("configurationNamingContext");
            RootDseAttribute.Add("currentTime");
            RootDseAttribute.Add("defaultNamingContext");
            RootDseAttribute.Add("dNSHostName");
            RootDseAttribute.Add("dsSchemaAttrCount");
            RootDseAttribute.Add("dsSchemaClassCount");
            RootDseAttribute.Add("dsSchemaPrefixCount");
            RootDseAttribute.Add("dsServiceName");
            RootDseAttribute.Add("highestCommittedUSN");
            RootDseAttribute.Add("isGlobalCatalogReady");
            RootDseAttribute.Add("isSynchronized");
            RootDseAttribute.Add("namingContexts");
            RootDseAttribute.Add("netlogon");
            RootDseAttribute.Add("pendingPropagations");
            RootDseAttribute.Add("rootDomainNamingContext");
            RootDseAttribute.Add("schemaNamingContext");
            RootDseAttribute.Add("serverName");
            RootDseAttribute.Add("subschemaSubentry");
            RootDseAttribute.Add("supportedCapabilities");
            RootDseAttribute.Add("supportedControl");
            RootDseAttribute.Add("supportedLDAPPolicies");
            RootDseAttribute.Add("supportedLDAPVersion");
            RootDseAttribute.Add("supportedSASLMechanisms");
            RootDseAttribute.Add("domainControllerFunctionality");
            RootDseAttribute.Add("domainFunctionality");
            RootDseAttribute.Add("forestFunctionality");
            RootDseAttribute.Add("msDS-ReplAllInboundNeighbors");
            RootDseAttribute.Add("msDS-ReplAllOutboundNeighbors");
            RootDseAttribute.Add("msDS-ReplConnectionFailures");
            RootDseAttribute.Add("msDS-ReplLinkFailures");
            RootDseAttribute.Add("msDS-ReplPendingOps");
            RootDseAttribute.Add("msDS-ReplQueueStatistics");
            RootDseAttribute.Add("msDS-TopQuotaUsage");
            RootDseAttribute.Add("supportedConfigurableSettings");
            RootDseAttribute.Add("supportedExtension");
            RootDseAttribute.Add("validFSMOs");
            RootDseAttribute.Add("dsaVersionString");
            RootDseAttribute.Add("msDS-PortLDAP");
            RootDseAttribute.Add("msDS-PortSSL");
            RootDseAttribute.Add("msDS-PrincipalName");
            RootDseAttribute.Add("serviceAccountInfo");
            RootDseAttribute.Add("spnRegistrationResult");
            RootDseAttribute.Add("tokenGroups");
            RootDseAttribute.Add("supportedConfigurableSettings");
            RootDseAttribute.Add("supportedExtension");
            RootDseAttribute.Add("validFSMOs");
            RootDseAttribute.Add("dsaVersionString");
            RootDseAttribute.Add("msDS-PortLDAP");
            RootDseAttribute.Add("msDS-PortSSL");
            RootDseAttribute.Add("usnAtRifm");

            #endregion

            #region msDS-TopQuotaUsage

            isRootDseAttribute = true;
            isBacklinkattribute = true;
            isSystemFlagval = true;
            DirectoryEntry msDSQuotaEntry = new DirectoryEntry();

            if (!adAdapter.GetObjectByDN("CN=NTDS Quotas,CN=Configuration," + adAdapter.rootDomainDN, out msDSQuotaEntry))
            {
                DataSchemaSite.Assert.IsTrue(false, "Object is not found");
            }
            DirectoryEntries ownerSidEntry = msDSQuotaEntry.Children;

            foreach (DirectoryEntry sidForQuota in ownerSidEntry)
            {
                sidValue = sidForQuota.Properties["msDS-QuotaTrustee"];
                byte[] sidValueForQuota = (byte[])sidValue.Value;
                sid = new SecurityIdentifier(sidValueForQuota, 0);
                sidValues = sid.Value.ToString();
                storeSid.Add(sidValues);
            }
            //msDS-TopQuotaUsage and msDS-QuotaUsed
            msDSQuotaEntry.RefreshCache(new string[] { "msDS-TopQuotaUsage" });
            PropertyValueCollection msDSTopQuotaUsage = msDSQuotaEntry.Properties["msDS-TopQuotaUsage"];

            if (msDSTopQuotaUsage.PropertyName.ToString().Equals("msDS-TopQuotaUsage"))
            {
                bool isString = true;
                foreach (object quotaUsage in msDSTopQuotaUsage)
                {
                    if (!(quotaUsage is string))
                    {
                        isString = false;
                        break;
                    }

                }

                DataSchemaSite.CaptureRequirementIfIsTrue(
                    isString,
                    339,
                    @"The value of TO!msDS-TopQuotaUsage, where TO be the object from which the msDS-TopQuotaUsage 
                    attribute is being read, R be the root object of the NC containing TO.
                    The TO!msDS-TopQuotaUsage equals a set of XML encoded strings sorted by the element quotaUsed when:
                    TO is the object: GetWellknownObject(n: R, guid: GUID_NTDS_QUOTAS_CONTAINER_W).");
            }
            DirectoryEntry TopQuotaUsageEntry;
            adAdapter.GetObjectByDN("CN=ms-DS-Top-Quota-Usage," + schemaNC, out TopQuotaUsageEntry);
            if (TopQuotaUsageEntry.Properties["linkID"].Value == null)
            {
                isBacklinkattribute = false;
            }
            if (!RootDseAttribute.Contains(TopQuotaUsageEntry.Name.ToString()))
            {
                isRootDseAttribute = false;
            }
            systemFlagVal = ParseSystemFlagsValue("FLAG_ATTR_IS_CONSTRUCTED");
            if (attrIsConstructFlag != systemFlagVal)
            {
                isSystemFlagval = false;
            }
            DataSchemaSite.Assert.IsTrue(
                isSystemFlagval
                || isBacklinkattribute
                || isRootDseAttribute,
                "Constructed attribute has been defined.");
            if (storeSid.Count > 0)
            {
                string sidForCompare = storeSid[0].ToString();
                foreach (string quotaUsage in msDSTopQuotaUsage)
                {
                    //Check if the ownerSID is assosiated with TO!objectSid
                    // if pass, R340 can be capture directly.
                    foreach (object i in sidForCompare)
                    {
                        if (quotaUsage.Contains(i.ToString()))
                        {
                            string[] splitValues = { "\r\n\t", "\r\n" };
                            string[] values = quotaUsage.Split(splitValues, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string value in values)
                            {
                                //quotaUsed
                                if (value.ToLower().StartsWith("<quotaused>"))
                                {
                                    string temp = String.Empty;
                                    temp = value.Substring(value.IndexOf(' ') + 1);
                                    temp = temp.Substring(0, temp.IndexOf(' '));
                                    quotaUsed = int.Parse(temp);
                                    storeSid.Add(quotaUsed);
                                }

                                //tombstoneCount
                                if (value.ToLower().StartsWith("<tombstonedcount>"))
                                {
                                    tempString = value.Substring(value.IndexOf(' ') + 1);
                                    tempString = tempString.Substring(0, tempString.IndexOf(' '));
                                    tombstoneCount = int.Parse(tempString);
                                    storeSid.Add(tombstoneCount);
                                }

                                //liveCount                        
                                if (value.ToLower().StartsWith("<livecount>"))
                                {
                                    tempString = String.Empty;
                                    tempString = value.Substring(value.IndexOf(' ') + 1);
                                    tempString = tempString.Substring(0, tempString.IndexOf(' '));
                                    liveCount = int.Parse(tempString);
                                    storeSid.Add(liveCount);
                                }
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }

                    DataSchemaSite.CaptureRequirement(
                        340,
                        @"In construction of msDS-TopQuotaUsage constructed attribute, Each string represents the quota information for a SID as specified in section 3.1.1.5.2.5, Quota Calculation.
                        The format of the XML encoded string is:
                        <MS_DS_TOP_QUOTA_USAGE>
                        <partitionDN> DN of the NC containing TO </partitionDN>
                        <ownerSID> SID of quota user </ownerSID>
                        <quotaUsed> rounded up value of quota used (computed) </quotaUsed>
                        <tombstoneCount> value in the TombstoneCount column </tombstoneCount>
                        <totalCount> value in the TotalCount column </totalCount>
                        </MS_DS_TOP_QUOTA_USAGE>
                        Where quotaUsed is computed as specified in msDS-QuotaUsed with cLive set to (totalCount - tombstoneCount).");
                }
            }

            #endregion

            #region msDS-QuotaEffective
            isSystemFlagval = true;
            isRootDseAttribute = true;
            isBacklinkattribute = true;
            //Let TO be the object from which the msDS-QuotaEffective attribute is being read.
            int countEffective = 0;

            do
            {

                msDSQuotaEntry.RefreshCache(new string[] { "msDS-QuotaEffective" });
                msDSQuotaEffective = msDSQuotaEntry.Properties["msDS-QuotaEffective"];

                //Let R be the root object of the NC containing TO.
                rootNC = msDSQuotaEntry.Parent.Name.ToString();
                DirectoryEntry QuotaEffectiveEntry;
                adAdapter.GetObjectByDN("CN=ms-DS-Quota-Effective," + schemaNC, out QuotaEffectiveEntry);
                if (QuotaEffectiveEntry.Properties["linkID"].Value == null)
                {
                    isBacklinkattribute = false;
                }
                if (!RootDseAttribute.Contains(QuotaEffectiveEntry.Name.ToString()))
                {
                    isRootDseAttribute = false;
                }
                systemFlagVal = ParseSystemFlagsValue("FLAG_ATTR_IS_CONSTRUCTED");
                if (attrIsConstructFlag != systemFlagVal)
                {
                    isSystemFlagval = false;
                }
                DataSchemaSite.Assert.IsTrue(
                    isSystemFlagval
                    || isBacklinkattribute
                    || isRootDseAttribute,
                    "Constructed attribute has been defined.");
                //Let SID be the sid specified by the LDAP extended control LDAP_SERVER_QUOTA_CONTROL_OID Or,
                //None specified, the requester's SID(Which user request for Quota).
                msDSQuotaEntry.RefreshCache(new string[] { "objectSid" });
                sidValue = msDSQuotaEntry.Properties["objectSid"];
                if ((sidValue.Value != null))
                {

                    //if none specified, the requester's SID.

                    if (!adAdapter.GetObjectByDN("CN=Users," + adAdapter.rootDomainDN, out dirEntry))
                    {
                        DataSchemaSite.Assert.IsTrue(false, "Object is not found");
                    }
                    DirectoryEntries sidEntries = dirEntry.Children;
                    foreach (DirectoryEntry sidEntry in sidEntries)
                    {
                        if (sidEntry.Name.Equals("CN=schema1"))
                        {
                            sidValue = sidEntry.Properties["objectSid"];
                            byte[] sidValueForQuota = (byte[])sidValue.Value;
                            SecurityIdentifier quotaSid = new SecurityIdentifier(sidValueForQuota, 0);
                            getSidValue = quotaSid.ToString();
                            //Let SID be the set of SID including SID and the set of SID returned by tokengroup
                            tokenGroup = getSidValue.Substring(getSidValue.Length - 4);
                        }
                    }

                    // [Since sidValue is not null, R334 is captured.]
                    DataSchemaSite.CaptureRequirement(
                        334,
                        @"In msDS-QuotaEffective constructed attribute,"
                        + "TO be the object from which the msDS-QuotaEffective attribute is being read, "
                        + "R be the root object of the NC containing TO,"
                        + "SID be the sid specified by the LDAP extended control LDAP_SERVER_QUOTA_CONTROL_OID or, "
                        + "if none is specified, the requestor's SID,"
                        + "and SIDs be the set of SIDs including SID and the set of SIDs returned by tokenGroups.");
                }
                if (!adAdapter.GetObjectByDN(adAdapter.rootDomainDN, out dirEntry))
                {
                    DataSchemaSite.Assert.IsTrue(false, "object is not found");
                }
                DirectoryEntries wellKnownObjects = dirEntry.Children;
                foreach (DirectoryEntry wellknownChild in wellKnownObjects)
                {
                    if (wellknownChild.Name.Equals("CN=NTDS Quotas"))
                    {
                        Object wellknownObject = dirEntry.Properties["wellknownObjects"].Value;
                        foreach (DNWithBinary wkObjects in (IEnumerable)wellknownObject)
                        {
                            string dnOfWellKnownObj = wkObjects.DNString;
                            if (dnOfWellKnownObj.Contains("CN=NTDS Quotas"))
                            {
                                byte[] bytes = (byte[])wkObjects.BinaryValue;
                                Guid guid = new Guid(bytes);
                                string byteToHex = BitConverter.ToString(bytes);
                            }
                        }
                    }
                }

                //O is a child of TO
                DirectoryEntries msDSQuotaAmount1 = msDSQuotaEntry.Children;
                foreach (DirectoryEntry quotaEntry in msDSQuotaAmount1)
                {
                    if (quotaEntry.Parent.Name.Equals("CN=NTDS Quotas"))
                    {
                        //O is a child of TO(ntds quota container)
                    }
                    quotaEntryValue = quotaEntry.Name.ToString();
                }

                string ldapSearchFilter1 = "((objectClass=msDS-QuotaControl))";
                string attributeToReturn1 = "msDS-QuotaAmount";
                LdapConnection connection1 = new LdapConnection(
                    new LdapDirectoryIdentifier(hostOrDomainName),
                    new System.Net.NetworkCredential(
                    adAdapter.DomainAdministratorName,
                    adAdapter.DomainUserPassword),
                    AuthType.Basic | AuthType.Ntlm);
                SearchRequest searchRequest1 = new SearchRequest(
                    "CN=Configuration,"
                    + adAdapter.rootDomainDN,
                    ldapSearchFilter1,
                    System.DirectoryServices.Protocols.SearchScope.Subtree,
                    attributeToReturn1);
                SearchResponse searchResponse1 = (SearchResponse)connection1.SendRequest(searchRequest1);
                foreach (SearchResultEntry entry in searchResponse1.Entries)
                {
                    if (!adAdapter.GetObjectByDN("CN=NTDS Quotas,CN=Configuration," + adAdapter.rootDomainDN, out msDSQuotaEntry))
                    {
                        DataSchemaSite.Assert.IsTrue(false, "Object is not found");
                    }
                    msDSQuotaAmount1 = msDSQuotaEntry.Children;
                    foreach (DirectoryEntry quotaEntries in msDSQuotaAmount1)
                    {
                        byte[] msDSQuotaTrustee = (byte[])quotaEntries.Properties["msDS-QuotaTrustee"].Value;
                        SecurityIdentifier trusteeGUID = new SecurityIdentifier(msDSQuotaTrustee, 0);
                        if (!adAdapter.GetObjectByDN("CN=Users," + adAdapter.rootDomainDN, out dirEntry))
                        {
                            DataSchemaSite.Assert.IsTrue(false, "Object is not found");
                        }

                        msDSQuotaAmount1 = dirEntry.Children;
                        foreach (DirectoryEntry childDomain in msDSQuotaAmount1)
                        {
                            msDSQuotaTrustee = (byte[])childDomain.Properties["objectSid"].Value;
                            SecurityIdentifier trusteeUserSID = new SecurityIdentifier(msDSQuotaTrustee, 0);
                        }
                    }
                    SearchResultAttributeCollection attributes = entry.Attributes;
                }
                ldapSearchFilter1 = "((objectClass=msDS-QuotaControl))";
                attributeToReturn1 = "msDS-QuotaTrustee";
                connection1 = new LdapConnection(
                    new LdapDirectoryIdentifier(hostOrDomainName),
                    new System.Net.NetworkCredential(
                    adAdapter.DomainAdministratorName,
                    adAdapter.DomainUserPassword),
                    AuthType.Basic | AuthType.Ntlm);
                searchRequest1 = new SearchRequest(
                    targetOu,
                    ldapSearchFilter1,
                    System.DirectoryServices.Protocols.SearchScope.Subtree,
                    attributeToReturn1);
                searchResponse1 = (SearchResponse)connection1.SendRequest(searchRequest1);
                foreach (SearchResultEntry entry in searchResponse1.Entries)
                {
                    SearchResultAttributeCollection attributes = entry.Attributes;
                }

                // [Since TO is the object from which the msDS-QuotaEffective attribute is being read, R335 is captured]
                DataSchemaSite.CaptureRequirement(
                    335,
                    @"The value of TO object msDS-QuotaEffective is the maximum of all Object O of msDS-QuotaAmount for each object O where 
                    (TO is the object:
                    GetWellknownObject(n: R, guid: GUID_NTDS_QUOTAS_CONTAINER_W))
                    and (O is a child of TO)
                    and (the client has access to O (O be the Object being considered during search)).
                    and (the client has access to O!msDS-QuotaAmount)
                    and (the client has access to O!msDS-QuotaTrustee)
                    and (there exists S in SIDS such that S is equal to O!msDS-QuotaTrustee).");

            } while (countEffective != 0);

            #endregion

            #region msDS-QuotaUsed
            isSystemFlagval = true;
            isRootDseAttribute = true;
            isBacklinkattribute = true;
            msDSTombQuotaFactor = msDSQuotaEntry.Properties["msDS-TombstoneQuotaFactor"];
            int msDSQuota, cLive, cTombstone, msDSTombQuotaFac;
            msDSTombQuotaFac = (int)msDSTombQuotaFactor.Value;
            //The liveCount value is taken from msDS-TopQuotaUsage Attribute.
            DirectoryEntry QuotaUsedEntry;

            adAdapter.GetObjectByDN("CN=ms-DS-Quota-Used," + schemaNC, out QuotaUsedEntry);
            if (QuotaUsedEntry.Properties["linkID"].Value == null)
            {
                isBacklinkattribute = false;
            }
            if (!RootDseAttribute.Contains(QuotaUsedEntry.Name.ToString()))
            {
                isRootDseAttribute = false;
            }
            systemFlagVal = ParseSystemFlagsValue("FLAG_ATTR_IS_CONSTRUCTED");
            if (attrIsConstructFlag != systemFlagVal)
            {
                isSystemFlagval = false;
            }
            DataSchemaSite.Assert.IsTrue(
                isSystemFlagval
                || isBacklinkattribute
                || isRootDseAttribute,
                "Constructed attribute has been defined.");
            //Which means non-deleted object.
            cLive = liveCount;
            //Which means deleted object.
            cTombstone = tombstoneCount;
            //The tombstoneCount is taken from msDS-TopQuotaUsage attribute.
            msDSQuota = (cLive + ((cTombstone * msDSTombQuotaFac) + 99) / 100);

            //MsDSQuota is calculated above, if test runs successfully, R337 is verified
            DataSchemaSite.CaptureRequirement(
                337,
                @"The value of TO!msDS-QuotaUsed is:
                (cLive + ((cTombstoned * TO!msDS-TombstoneQuotaFactor)+99)/100) 
                where, cLive is the number of non-tombstoned Objects associated with SID, and cTombstoned is the number 
                of Tombstoned Objects associated with SID , as detailed in section 3.1.1.5.2.5, Quota Calculation, and 
                when, (TO is the object, GetWellknownObject(n: R, guid: GUID_NTDS_QUOTAS_CONTAINER_W))
                Where TO be the object from which the msDS-QuotaUsed attribute is being read, C be the Most Specific 
                Class from TO!objectClass, R be the root object of the NC containing TO, SID be the sid specified by 
                the LDAP extended control.");

            #endregion

            #region msDS-PrincipalName

            DirectoryEntry rootEntry = new DirectoryEntry();
            if (serverOS >= OSVersion.WinSvr2008)
            {
                isSystemFlagval = true;
                isRootDseAttribute = true;
                isBacklinkattribute = true;
                //Checking the some of the constructed attributes for the object Users.
                if (!adAdapter.GetObjectByDN("CN=Users," + adAdapter.rootDomainDN, out dirEntry))
                {
                    DataSchemaSite.Assert.IsTrue(false, "CN=Users,"
                    + adAdapter.rootDomainDN + " Object is not found in server");
                }
                //For DomainName retrievals used this entry.
                rootEntry = new DirectoryEntry();
                if (!adAdapter.GetObjectByDN(adAdapter.rootDomainDN, out rootEntry))
                {
                    DataSchemaSite.Assert.IsTrue(false, adAdapter.rootDomainDN + "Object is not found in server");
                }
                DirectoryEntries childForUsers = dirEntry.Children;
                foreach (DirectoryEntry childEntry in childForUsers)
                {
                    childEntry.RefreshCache(new string[] { "msDS-PrincipalName" });
                    string msDSPrincipalName = childEntry.Properties["msDS-PrincipalName"].Value.ToString().ToLower();
                    DirectoryEntry PrincipalNameEntry;
                    adAdapter.GetObjectByDN("CN=ms-DS-Principal-Name," + schemaNC, out PrincipalNameEntry);
                    if (PrincipalNameEntry.Properties["linkID"].Value == null)
                    {
                        isBacklinkattribute = false;
                    }
                    if (!RootDseAttribute.Contains(PrincipalNameEntry.Name.ToString()))
                    {
                        isRootDseAttribute = false;
                    }
                    systemFlagVal = ParseSystemFlagsValue("FLAG_ATTR_IS_CONSTRUCTED");
                    if (attrIsConstructFlag != systemFlagVal)
                    {
                        isSystemFlagval = false;
                    }
                    DataSchemaSite.Assert.IsTrue(
                        isSystemFlagval
                        || isBacklinkattribute
                        || isRootDseAttribute,
                        "Constructed attribute has been defined.");
                    //Constraint 1:
                    //Checking domainName and sAMAccountName values
                    string domainName = rootEntry.Properties["name"].Value.ToString();
                    string sAMAccountName = childEntry.Properties["sAMAccountName"].Value.ToString();

                    if (msDSPrincipalName.Equals(domainName.ToLower() + "\\" + sAMAccountName.ToLower()))
                    {

                        //Constraint 2:
                        //ObjectSid is compare with SDDL SID string format;                  
                        objectSid = (byte[])childEntry.Properties["objectSid"].Value;
                        sid = new SecurityIdentifier(objectSid, 0);
                        dcModel.TryGetAttributeContext("objectSid", out attrContext);

                        if (attrContext.syntax.Name.Equals("StringSidSyntax"))
                        {
                            // [Since the To!objectSid is SID string format, R349 is captured.]
                            DataSchemaSite.CaptureRequirement(
                                349,
                                 @"For AD/DS, the value of TO!msDS-PrincipalName is either
                                 (1) the NetBIOS domain name, followed by a backslash ('\'), followed by TO!sAMAccountName, or 
                                (2) the value of TO!objectSid in SDDL SID string format 
                                where TO be the object from which the msDS-PrincipalName attribute is being read.");
                        }
                    }
                    break;
                }
            }

            #endregion

            #region parentGUID

            //SchemaNC : objectClass=ClassSchema were taken for example.
            //ParentGUID is check with SchemaNC objects.
            if (!adAdapter.GetObjectByDN("CN=Schema,CN=Configuration," + adAdapter.rootDomainDN, out rootEntry))
            {
                DataSchemaSite.Assert.IsTrue(false, adAdapter.rootDomainDN + "Object is not found in server");
            }
            string ldapSearchFilter = "((objectClass=classSchema))";
            string attributeToReturn = "parentGUID";
            LdapConnection connection = new LdapConnection(hostOrDomainName);
            SearchRequest searchRequest = new SearchRequest(
                targetOu,
                ldapSearchFilter,
                System.DirectoryServices.Protocols.SearchScope.Subtree,
                attributeToReturn);
            SearchResponse searchResponse = (SearchResponse)connection.SendRequest(searchRequest);
            DirectoryEntry ParentGUIDEntry;
            adAdapter.GetObjectByDN("CN=Parent-GUID," + schemaNC, out ParentGUIDEntry);

            if (ParentGUIDEntry.Properties["linkID"].Value == null)
            {
                isBacklinkattribute = false;
            }
            if (!RootDseAttribute.Contains(ParentGUIDEntry.Name.ToString()))
            {
                isRootDseAttribute = false;
            }
            systemFlagVal = ParseSystemFlagsValue("FLAG_ATTR_IS_CONSTRUCTED");
            if (attrIsConstructFlag != systemFlagVal)
            {
                isSystemFlagval = false;
            }
            DataSchemaSite.Assert.IsTrue(
                isSystemFlagval
                || isBacklinkattribute
                || isRootDseAttribute,
                "Constructed attribute has been defined.");
            foreach (SearchResultEntry entry in searchResponse.Entries)
            {
                if (entry.DistinguishedName == "CN=account,CN=Schema,CN=Configuration," + adAdapter.rootDomainDN)
                {
                    SearchResultAttributeCollection attributes = entry.Attributes;

                    foreach (DirectoryAttribute attribute in attributes.Values)
                    {
                        // Count the number of values associated with this attribute
                        for (int i = 0; i < attribute.Count; i++)
                        {
                            byte[] parentGUID = attribute[i] as byte[];
                            Guid parentGUIDForEntry = new Guid(parentGUID);
                            string parentGUIDValue = parentGUIDForEntry.ToString();
                            byte[] objectGuidForEntry = (byte[])rootEntry.Properties["objectGUID"].Value;
                            Guid objectGUID = new Guid(objectGuidForEntry);
                            string objectGUIDValue = objectGUID.ToString();
                            try
                            {
                                if (parentGUIDValue == objectGUIDValue)
                                {
                                    if (parentGUIDValue != null)
                                    {
                                        // [Since the parentGUID exists from an object that is not root object, R354 is captured.]
                                        DataSchemaSite.CaptureRequirement(
                                            354,
                                            @"parentGUID constructed attribute is not present on an object
                                            that is the root of an NC.");
                                    }
                                    // [Since the parentGUID is present in the subobject of TP, R355 is captured]
                                    DataSchemaSite.CaptureRequirement(
                                        355,
                                        @"For all other objects, let TO be the object from which the parentGUID 
                                        attribute is being read and let TP be TO!parent. TO!parentGUID is equal to 
                                        TP!objectGUID.");
                                }
                                else
                                {
                                    isParentGUIDEqualToObjectGUID = false;
                                }
                            }
                            catch (Exception)
                            {
                                DataSchemaSite.Assert.IsTrue(false, "Object is not exist");
                            }
                        }
                    }
                }
            }



            DirectoryEntry dirEntryVal = new DirectoryEntry();

            #endregion

            #region msDS-isUserCachableAtRodc

            isSystemFlagval = true;
            isRootDseAttribute = true;
            isBacklinkattribute = true;
            //Let TO is a Computer object
            DirectoryEntry rodcEntry = new DirectoryEntry();

            if (!adAdapter.GetObjectByDN("CN=Computers," + adAdapter.rootDomainDN, out rodcEntry))
            {
                DataSchemaSite.Assert.IsTrue(false, "Object is not found");
            }
            DirectoryEntries rodcSearchEntry = rodcEntry.Children;
            //If TO is a computer Object:
            //If TO!userAccountControl does not have the ADS_UF_PARTIAL_SECRETS_ACCOUNT bit set,
            //TO!msDS-IsUserCachableAtRodc has no value.
            foreach (DirectoryEntry rodcEntries in rodcSearchEntry)
            {
                PropertyValueCollection uAC = rodcEntries.Properties["userAccountControl"];
                PropertyValueCollection rodc = rodcEntries.Properties["msDS-isUserCachableAtRodc"];
                DirectoryEntry UserCachableAtRodcEntry;
                adAdapter.GetObjectByDN("CN=ms-DS-Is-User-Cachable-At-Rodc," + schemaNC, out UserCachableAtRodcEntry);
                if (UserCachableAtRodcEntry.Properties["linkID"].Value == null)
                {
                    isBacklinkattribute = false;
                }
                if (!RootDseAttribute.Contains(UserCachableAtRodcEntry.Name.ToString()))
                {
                    isRootDseAttribute = false;
                }
                systemFlagVal = ParseSystemFlagsValue("FLAG_ATTR_IS_CONSTRUCTED");
                if (attrIsConstructFlag != systemFlagVal)
                {
                    isSystemFlagval = false;
                }
                DataSchemaSite.Assert.IsTrue(
                    isSystemFlagval
                    || isBacklinkattribute
                    || isRootDseAttribute,
                    "Constructed attribute has been defined.");
                if (rodc.PropertyName.ToString().Equals("msDS-isUserCachableAtRodc"))
                {
                    // [The property msDS-isUserCachableAtRodc is present since TO is a computer object. Then R375 is captured]
                    DataSchemaSite.CaptureRequirement(
                        375,
                        @"In msDS-IsUserCachableAtRodc constructed attribute, if TO is not a nTDSDSA, computer,
                        or server object, then TO!msDS-IsUserCachableAtRodc is not present,
                        where TO be the object on which msDS-IsUserCachableAtRodc is being read.");
                }
                else
                {
                    DataSchemaSite.Assert.Fail("Failed to get msDS-isUserCachableAtRodc");
                }

                int userAccValue = (int)uAC.Value;
                int userAccControlFlag = ParseUserAccountControlValue("ADS_UF_PARTIAL_SECRETS_ACCOUNT");

                if ((userAccValue & userAccControlFlag) == 0)
                {
                    //If TO!userAccountControl does not have the ADS_UF_PARTIAL_SECRETS_ACCOUNT bit set,
                    //TO!msDS-IsUserCachableAtRodc has no value.
                    DataSchemaSite.CaptureRequirementIfAreEqual<object>(
                        null,
                        rodc.Value,
                        376,
                        @"In msDS-IsUserCachableAtRodc constructed attribute, if TO is a computer object and
                        If TO!userAccountControl does not have the ADS_UF_PARTIAL_SECRETS_ACCOUNT bit set,
                        TO!msDS-IsUserCachableAtRodc has no value.
                        where TO be the object on which msDS-IsUserCachableAtRodc is being read.");
                }
            }
            //constraint 2:-
            //If it is a server object
            //Let TC be the computer object named by TO!ServerReference
            DirectoryEntry computerEntry = new DirectoryEntry();
            DirectoryEntry serverEntry = new DirectoryEntry();

            PropertyValueCollection serverReference, computerReferenceBL;
            if (!adAdapter.GetObjectByDN("OU=Domain Controllers," + adAdapter.rootDomainDN, out computerEntry))
            {
                DataSchemaSite.Assert.IsTrue(false, "object does not exists");
            }
            if (!adAdapter.GetObjectByDN(
                "CN="
                + adAdapter.RODCNetbiosName
                + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration,"
                + adAdapter.rootDomainDN,
                out serverEntry))
            {
                DataSchemaSite.Assert.IsTrue(false, "object does not exists");
            }
            serverReference = serverEntry.Properties["serverReference"];

            DirectoryEntries computerEntries = computerEntry.Children;

            foreach (DirectoryEntry computerEntryVal in computerEntries)
            {
                computerReferenceBL = computerEntryVal.Properties["serverReferenceBL"];
                if (computerReferenceBL.Value != null)
                {
                    if (computerReferenceBL.Value.ToString().Contains("CN=" + adAdapter.RODCNetbiosName.ToUpper()))
                    {
                        PropertyValueCollection uAC = computerEntryVal.Properties["userAccountControl"];
                        int userAccControlFlag = ParseUserAccountControlValue("ADS_UF_PARTIAL_SECRETS_ACCOUNT");
                        int userAccValue = (int)uAC.Value;
                        if ((userAccValue & userAccControlFlag) == 0)
                        {
                            computerEntryVal.RefreshCache(new string[] { "msDS-msDS-IsUserCachableAtRodc" });
                            PropertyValueCollection isUserCachable = computerEntryVal.Properties["msDS-msDS-IsUserCachableAtRodc"];

                            DataSchemaSite.CaptureRequirementIfAreEqual<object>(
                                null,
                                isUserCachable.Value,
                                380,
                                @"In msDS-IsUserCachableAtRodc constructed attribute, if TO is a server object,
                                TC be the computer object named by TO!serverReference and 
                                If TC!userAccountControl does not have the ADS_UF_PARTIAL_SECRETS_ACCOUNT bit set,
                                TC!msDS-IsUserCachableAtRodc has no value.
                                where TO be the object on which msDS-IsUserCachableAtRodc is being read.");
                        }
                    }
                }
            }
            //constraint 3:-
            //If it is an nTDSDSA object             
            //Let TS be the server object that is parent of TO.
            DirectoryEntries nTDSDSAEntries = serverEntry.Children;
            DirectoryEntries computerValEntry = computerEntry.Children;
            foreach (DirectoryEntry nTDSDSA in nTDSDSAEntries)
            {
                if (nTDSDSA.Properties["objectClass"].Contains("nTDSDSA"))
                {
                    if (nTDSDSA.Parent.Name.ToLower().Equals("cn=" + adAdapter.RODCNetbiosName.ToLower()))
                    {
                        foreach (DirectoryEntry computerEntryVal in computerValEntry)
                        {
                            computerEntryVal.RefreshCache(new string[] { "serverReferenceBL" });
                            computerReferenceBL = computerEntryVal.Properties["serverReferenceBL"];
                            if (computerReferenceBL.Value != null)
                            {
                                if (computerReferenceBL.Value.ToString().ToLower().Contains(
                                    "cn=" +
                                    adAdapter.RODCNetbiosName.ToLower()))
                                {
                                    PropertyValueCollection uAC = computerEntryVal.Properties["userAccountControl"];
                                    int userAccControlFlag = ParseUserAccountControlValue("ADS_UF_PARTIAL_SECRETS_ACCOUNT");
                                    int userAccValue = (int)uAC.Value;
                                    if ((userAccValue & userAccControlFlag) == 0)
                                    {
                                        computerEntryVal.RefreshCache(new string[] { "msDS-msDS-IsUserCachableAtRodc" });
                                        PropertyValueCollection isUserCachable = computerEntryVal.Properties["msDS-msDS-IsUserCachableAtRodc"];

                                        DataSchemaSite.CaptureRequirementIfAreEqual<object>(
                                            null,
                                            isUserCachable.Value,
                                            381,
                                            @"In msDS-IsUserCachableAtRodc constructed attribute, 
                                            If TO is an nTDSDSA object, 
                                            TS be the server object that is the parent of TO and  
                                            If TS is a server object,
                                            TC be the computer object named by TS!serverReference and 
                                            If TC!userAccountControl does not have the 
                                            ADS_UF_PARTIAL_SECRETS_ACCOUNT bit set,
                                            TC!msDS-IsUserCachableAtRodc has no value.
                                            where TO be the object on which msDS-IsUserCachableAtRodc 
                                            is being read.");
                                    }
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            #region msDS-RevealedList

            //attribute exists only on the computer object of an RODC.
            if (serverOS >= OSVersion.WinSvr2008)
            {
                isSystemFlagval = true;
                isRootDseAttribute = true;
                isBacklinkattribute = true;
                if (!adAdapter.GetObjectByDN("OU=Domain Controllers," + adAdapter.rootDomainDN, out rodcEntry))
                {
                    DataSchemaSite.Assert.IsTrue(false, "Object is not found");
                }
                DirectoryEntries revealedSearchEntry = rodcEntry.Children;
                foreach (DirectoryEntry revealedEntry in revealedSearchEntry)
                {
                    revealedEntry.RefreshCache(new string[] { "serverReferenceBL" });
                    computerReferenceBL = revealedEntry.Properties["serverReferenceBL"];
                    DirectoryEntry RevealedListEntry;
                    adAdapter.GetObjectByDN("CN=ms-DS-Revealed-List," + schemaNC, out RevealedListEntry);
                    if (RevealedListEntry.Properties["linkID"].Value == null)
                    {
                        isBacklinkattribute = false;
                    }
                    if (!RootDseAttribute.Contains(RevealedListEntry.Name.ToString()))
                    {
                        isRootDseAttribute = false;
                    }
                    RevealedListEntry.RefreshCache(new string[] { "systemFlags" });
                    string systemFlagValToString = ParseSystemFlagsValue((int)RevealedListEntry.Properties["systemFlags"].Value);
                    if (!systemFlagValToString.ToLower().Contains("FLag_ATTR_IS_CONSTRUCTED".ToLower()))
                    {
                        isSystemFlagval = false;
                    }
                    DataSchemaSite.Assert.IsTrue(
                        isSystemFlagval
                        || isBacklinkattribute
                        || isRootDseAttribute,
                        "Constructed attribute has been defined.");
                    if (computerReferenceBL.Value != null)
                    {
                        if (computerReferenceBL.Value.ToString().ToLower().Contains(
                            "cn=" + adAdapter.RODCNetbiosName.ToLower()))
                        {
                            PropertyValueCollection revealedAtt = revealedEntry.Properties["msDS-RevealedList"];
                            if (revealedAtt.PropertyName.Equals("msDS-RevealedList"))
                            {
                                dcModel.TryGetAttributeContext("msDS-RevealedList", out attrContext);

                                if (attrContext.syntax.Name.Equals("ObjectDNStringSyntax"))
                                {
                                    // [Since msDS-RevealedList is DN-String, R386 is captured]
                                    DataSchemaSite.CaptureRequirement(
                                        386,
                                        @"The msDS-RevealedList attribute exists only on the computer object of an RODC.
                                        The value of msDS-RevealedList is a multi-valued DN-String.The string portion 
                                        of each value is the lDAPDisplayName of a secret attribute, and the DN portion 
                                        of each value names an object.");
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            #region msDS-KeyVersionNumber

            isSystemFlagval = true;
            isRootDseAttribute = true;
            isBacklinkattribute = true;
            //Let TO be the object from which the msDS-KeyVersionNumber attribute is being read.
            DirectoryEntry keyVersionEntry = new DirectoryEntry();
            PropertyValueCollection keyVerNo;

            if (!adAdapter.GetObjectByDN("CN=Users," + adAdapter.rootDomainDN, out keyVersionEntry))
            {
                DataSchemaSite.Assert.IsTrue(false, "Object is not found");
            }
            DirectoryEntry deHuristicEntry = new DirectoryEntry();
            if (!adAdapter.GetObjectByDN(
                "CN=Directory Service,CN=Windows NT,CN=Services,CN=Configuration," +
                adAdapter.rootDomainDN,
                out deHuristicEntry))
            {
                DataSchemaSite.Assert.IsTrue(false, "Object is not found");
            }

            DirectoryEntries keyVersionUsers = keyVersionEntry.Children;

            foreach (DirectoryEntry keyEntry in keyVersionUsers)
            {
                if (keyEntry.Name.Equals("CN=" + adAdapter.DomainAdministratorName))
                {
                    keyEntry.RefreshCache(new string[] { "msDS-KeyVersionNumber" });
                    keyVerNo = keyEntry.Properties["msDS-KeyVersionNumber"];
                    DirectoryEntry KeyVersionNumberEntry;
                    adAdapter.GetObjectByDN("CN=ms-DS-KeyVersionNumber," + schemaNC, out KeyVersionNumberEntry);
                    if (KeyVersionNumberEntry.Properties["linkID"].Value == null)
                    {
                        isBacklinkattribute = false;
                    }
                    if (!RootDseAttribute.Contains(KeyVersionNumberEntry.Name.ToString()))
                    {
                        isRootDseAttribute = false;
                    }
                    systemFlagVal = ParseSystemFlagsValue("FLAG_ATTR_IS_CONSTRUCTED");
                    if (attrIsConstructFlag != systemFlagVal)
                    {
                        isSystemFlagval = false;
                    }
                    DataSchemaSite.Assert.IsTrue(
                        isSystemFlagval
                        || isBacklinkattribute
                        || isRootDseAttribute,
                        "Constructed attribute has been defined.");
                    deHuristicEntry.RefreshCache(new string[] { "dSHeuristics" });
                    PropertyValueCollection dsHeuristic = deHuristicEntry.Properties["dSHeuristics"];
                    string dsHeu = null;

                    if (dsHeuristic.Value != null)
                        dsHeu = dsHeuristic.Value.ToString();
                    if ((dsHeu != null) && (dsHeu.StartsWith("000000000000000001")))
                    {
                        // [Since dsHeuristic is not null and msDS-KeyVersionNumber equals 1, R310 is captured]
                        DataSchemaSite.CaptureRequirementIfAreEqual<int>(
                            1,
                            (int)keyVerNo.Value,
                            310,
                            @"The value of TO!msDS-KeyVersionNumber,
                            where TO be the object from which the msDS-KeyVersionNumber attribute is being read,
                            equals 1 if the fKVNOEmuW2k dsHeuristic is true.");
                    }
                }
            }


            #endregion

            #region sDRightsEffective

            isSystemFlagval = true;
            isRootDseAttribute = true;
            isBacklinkattribute = true;
            DirectoryEntry entryForSD;

            //Get the entry
            entryForSD = new DirectoryEntry("LDAP://CN=Computers," + adAdapter.rootDomainDN);
            foreach (DirectoryEntry entryForSDRights in entryForSD.Children)
            {
                string clientCompName = adAdapter.DomainAdministratorName;
                //Get the owner
                DirectoryEntry ownerEntry = new DirectoryEntry("LDAP://CN=" + clientCompName + ",CN=Users," + adAdapter.rootDomainDN);
                //Get the attribute value
                entryForSDRights.RefreshCache(new string[] { "sdrightseffective" });
                int sDRightsEffective = int.Parse(entryForSDRights.Properties["sdrightseffective"].Value.ToString());
                DirectoryEntry SDRightsEffectiveEntry;
                adAdapter.GetObjectByDN("CN=SD-Rights-Effective," + schemaNC, out SDRightsEffectiveEntry);
                if (SDRightsEffectiveEntry.Properties["linkID"].Value == null)
                {
                    isBacklinkattribute = false;
                }
                if (!RootDseAttribute.Contains(SDRightsEffectiveEntry.Name.ToString()))
                {
                    isRootDseAttribute = false;
                }
                systemFlagVal = ParseSystemFlagsValue("FLAG_ATTR_IS_CONSTRUCTED");
                if (attrIsConstructFlag != systemFlagVal)
                {
                    isSystemFlagval = false;
                }
                DataSchemaSite.Assert.IsTrue(
                    isSystemFlagval
                    || isBacklinkattribute
                    || isRootDseAttribute,
                    "Constructed attribute has been defined.");
                //Get the dacl and process the ace to find out the owner ace
                sd = (IADsSecurityDescriptor)entryForSDRights.Properties["ntsecuritydescriptor"].Value;
                dacl = (IADsAccessControlList)sd.DiscretionaryAcl;
                isSuccess = false;
                bool isWriteOwnerset = false;
                bool isWriteDAC = false;
                bool isWriteAccessSecurity = false;
                foreach (AccessControlEntry ace in dacl)
                {
                    if (ace.Trustee.ToLower().Contains("domain admins"))
                    {
                        int accessMask = ace.AccessMask;

                        if ((accessMask & 524288) == 524288)
                        {
                            isWriteOwnerset = (sDRightsEffective & 3) == 3;

                            DataSchemaSite.CaptureRequirementIfIsTrue(
                                isWriteOwnerset,
                                282,
                                @"OSI (OWNER_SECURITY_INFORMATION)
                                and GSI (GROUP_SECURITY_INFORMATION) are both
                                set if TO!nTSecurityDescriptor grants RIGHT_WRITE_OWNER to the requester");
                        }
                        else
                        {
                            isWriteOwnerset = (sDRightsEffective & 3) == 0;
                        }

                        if ((accessMask & 262144) == 262144)
                        {
                            isWriteDAC = (sDRightsEffective & 4) == 4;

                            DataSchemaSite.CaptureRequirementIfIsTrue(
                                isWriteDAC,
                                283,
                                @"DSI (DACL_SECURITY_INFORMATION) is set if TO!nTSecurityDescriptor grants 
                                RIGHT_WRITE_DAC to the requester.");
                        }
                        else
                        {
                            isWriteDAC = (sDRightsEffective & 4) == 0;
                        }


                        if (entryForSDRights.Properties["ntsecuritydescriptor"].Value != null)
                        {
                            isWriteAccessSecurity = (sDRightsEffective & 8) == 8;

                            DataSchemaSite.CaptureRequirementIfIsTrue(isWriteAccessSecurity, 284, @"SSI 
                             (SACL_SECURITY_INFORMATION) is set if TO!nTSecurityDescriptor grants 
                             RIGHT_ACCESS_SYSTEM_SECURITY to the requester.");
                        }
                        else
                        {
                            isWriteAccessSecurity = (sDRightsEffective & 8) == 0;
                        }
                    }
                }
                if (isWriteAccessSecurity)
                {
                    break;
                }
            }

            #endregion

            #region msDs-Approx-Immed-Subordinates

            isSystemFlagval = true;
            isRootDseAttribute = true;
            isBacklinkattribute = true;
            dirEntry = new DirectoryEntry("LDAP://CN=Users," + adAdapter.rootDomainDN);
            dirEntry.RefreshCache(new string[] { "msDS-Approx-Immed-Subordinates" });
            DirectoryEntry SubordinatesEntry;

            adAdapter.GetObjectByDN("CN=ms-DS-Approx-Immed-Subordinates," + schemaNC, out SubordinatesEntry);
            if (SubordinatesEntry.Properties["linkID"].Value == null)
            {
                isBacklinkattribute = false;
            }
            if (!RootDseAttribute.Contains(SubordinatesEntry.Name.ToString()))
            {
                isRootDseAttribute = false;
            }
            systemFlagVal = ParseSystemFlagsValue("FLAG_ATTR_IS_CONSTRUCTED");
            if (attrIsConstructFlag != systemFlagVal)
            {
                isSystemFlagval = false;
            }
            DataSchemaSite.Assert.IsTrue(
                isSystemFlagval
                || isBacklinkattribute
                || isRootDseAttribute,
                "Constructed attribute has been defined.");
            sd = (IADsSecurityDescriptor)dirEntry.Properties["ntsecuritydescriptor"].Value;
            dacl = (IADsAccessControlList)sd.DiscretionaryAcl;

            AccessControlEntry tempAce = null;
            foreach (AccessControlEntry ace in dacl)
            {
                if (ace.Trustee.ToLower().Contains("domain admins"))
                {
                    int accessMask = ace.AccessMask;

                    if ((accessMask & 4) == 4)
                    {

                        int expectedValue = int.Parse(dirEntry.Properties["msDS-Approx-Immed-Subordinates"].Value.ToString());

                        int actualVal = 0;
                        foreach (DirectoryEntry child in dirEntry.Children)
                        {
                            actualVal++;
                        }

                        isSuccess = false;

                        isSuccess = (expectedValue > actualVal - 5) && (expectedValue < actualVal + 10);
                        DataSchemaSite.CaptureRequirementIfIsTrue(
                            isSuccess,
                            307,
                            @" The value of 
                            TO!msDS-Approx-Immed-Subordinates, where TO be the object from which the 
                            msDS-Approx-Immed-Subordinates attribute is being read, is the approximate
                            number of Objects contained by this object if TO!nTSecurityDescriptor grants 
                            RIGHT_DS_LIST_CONTENTS to the client.This estimate has no guarantee or 
                            requirement of accuracy.");

                        //removing the right
                        int DPbit = ace.AccessMask;

                        DPbit = (DPbit & (~4));

                        if ((DPbit & 4) != 4)
                        {
                            ace.AccessMask = DPbit;
                        }
                        tempAce = ace;
                        dacl.RemoveAce(ace);
                        dacl.AddAce(tempAce);
                        sd.DiscretionaryAcl = dacl;
                        dirEntry.Properties["ntsecuritydescriptor"].Value = sd;
                        dirEntry.CommitChanges();
                        System.Threading.Thread.Sleep(10000);
                        break;
                    }
                }
            }

            sd = (IADsSecurityDescriptor)dirEntry.Properties["ntsecuritydescriptor"].Value;
            dacl = (IADsAccessControlList)sd.DiscretionaryAcl;

            foreach (AccessControlEntry ace in dacl)
            {
                if (ace.Trustee.ToLower().Contains("domain admins"))
                {
                    int accessMask = ace.AccessMask;

                    if ((accessMask & 4) != 4)
                    {
                        int expectedValue = 0;
                        int actualVal = int.Parse(dirEntry.Properties["msDS-Approx-Immed-Subordinates"].Value.ToString());
                        isSuccess = expectedValue != actualVal;

                        DataSchemaSite.CaptureRequirementIfIsTrue(
                            isSuccess,
                            308,
                            @" The value of
                            TO!msDS-Approx-Immed-Subordinates, where TO be the object from which the 
                            msDS-Approx-Immed-Subordinates attribute is being read, is 0 
                            if TO!nTSecurityDescriptor does not grant RIGHT_DS_LIST_CONTENTS to the client.");
                    }

                    int DPbit = ace.AccessMask;

                    DPbit = (DPbit & 4);

                    if (DPbit == 0)
                    {
                        ace.AccessMask = ace.AccessMask | 4;
                    }

                    tempAce = ace;
                    dacl.RemoveAce(ace);
                    dacl.AddAce(tempAce);
                    sd.DiscretionaryAcl = dacl;
                    dirEntry.Properties["ntsecuritydescriptor"].Value = sd;
                    dirEntry.CommitChanges();
                    System.Threading.Thread.Sleep(4000);
                    break;
                }
            }

            #endregion

            #region msDS-UserPasswordExpiryTimeComputed
            List<string> listOfObjects = new List<string>();
            if (serverOS >= OSVersion.WinSvr2008)
            {
                listOfObjects = new List<string>();
                listOfObjects.Add("CN=" + adAdapter.DomainAdministratorName + ",CN=Users," + adAdapter.rootDomainDN);

                foreach (string entryDN in listOfObjects)
                {
                    isSuccess = false;
                    #region Getting the user object
                    DirectoryEntry userEntry = null;
                    adAdapter.GetObjectByDN(entryDN, out userEntry);

                    #endregion

                    #region condition1
                    isSystemFlagval = true;
                    isRootDseAttribute = true;
                    isBacklinkattribute = true;
                    PropertyValueCollection userAccountControl = userEntry.Properties["userAccountControl"];
                    int userAccControlFlag = ParseUserAccountControlValue(
                        "ADS_UF_DONT_EXPIRE_PASSWD|ADS_UF_SMARTCARD_REQUIRED|" +
                        "ADS_UF_WORKSTATION_TRUST_ACCOUNT|ADS_UF_SERVER_TRUST_ACCOUNT|ADS_UF_INTERDOMAIN_TRUST_ACCOUNT");
                    if (((int.Parse(userAccountControl.Value.ToString()) & userAccControlFlag) != 0))
                    {
                        userEntry.RefreshCache(new string[] { "msDS-UserPasswordExpiryTimeComputed" });
                        DirectoryEntry UserPasswordEntry;
                        adAdapter.GetObjectByDN("CN=ms-DS-User-Password-Expiry-Time-Computed," + schemaNC, out UserPasswordEntry);
                        if (UserPasswordEntry.Properties["linkID"].Value == null)
                        {
                            isBacklinkattribute = false;
                        }
                        if (!RootDseAttribute.Contains(UserPasswordEntry.Name.ToString()))
                        {
                            isRootDseAttribute = false;
                        }
                        systemFlagVal = ParseSystemFlagsValue("FLAG_ATTR_IS_CONSTRUCTED");
                        if (attrIsConstructFlag != systemFlagVal)
                        {
                            isSystemFlagval = false;
                        }
                        DataSchemaSite.Assert.IsTrue(
                            isSystemFlagval
                            || isBacklinkattribute
                            || isRootDseAttribute,
                            "Constructed attribute has been defined.");
                        DirectorySearcher ds = new DirectorySearcher(userEntry, "(objectClass=*)",

                            new string[] { "msDS-UserPasswordExpiryTimeComputed" },

                            System.DirectoryServices.SearchScope.Base);

                        System.DirectoryServices.SearchResult result = ds.FindOne();

                        long UserPasswordExpiryTimeComputed = (long)result.Properties["msDS-UserPasswordExpiryTimeComputed"][0];

                        //9223372036854775807 is equivalent to 7FFFFFFFFFFFFFFF
                        isSuccess = UserPasswordExpiryTimeComputed == 9223372036854775807;

                    }

                    if (!isSuccess)
                    {
                        userEntry.Properties["pwdlastset"].Value = 0;
                        userEntry.CommitChanges();
                        DirectorySearcher ds = new DirectorySearcher(userEntry, "(objectClass=*)",

                            new string[] { "msDS-UserPasswordExpiryTimeComputed" },

                            System.DirectoryServices.SearchScope.Base);

                        System.DirectoryServices.SearchResult result = ds.FindOne();

                        long UserPasswordExpiryTimeComputed = (long)result.Properties["msDS-UserPasswordExpiryTimeComputed"][0];

                        isSuccess = UserPasswordExpiryTimeComputed == 0;

                    }
                    if (!isSuccess)
                    {
                        DirectoryEntry pwdPolicy = new DirectoryEntry(
                            "LDAP://CN=testpassword,CN=Password Settings Container,CN=System,"
                            + adAdapter.rootDomainDN);
                        pwdPolicy.Properties["msDS-PSOAppliesTo"].Value = entryDN;
                        pwdPolicy.CommitChanges();

                        userEntry = new DirectoryEntry("LDAP://" + entryDN);

                        DirectorySearcher ds = new DirectorySearcher(pwdPolicy, "(objectClass=*)",

                                new string[] { "msDS-MaximumPasswordAge" },

                                System.DirectoryServices.SearchScope.Base);

                        System.DirectoryServices.SearchResult result = ds.FindOne();

                        string maxPwdAge = result.Properties["msDS-MaximumPasswordAge"][0].ToString();
                        if (maxPwdAge == "-9223372036854775808")
                        {
                            userEntry = new DirectoryEntry("LDAP://" + entryDN);
                            ds = new DirectorySearcher(userEntry, "(objectClass=*)",

                            new string[] { "msDS-UserPasswordExpiryTimeComputed" },

                            System.DirectoryServices.SearchScope.Base);

                            result = ds.FindOne();

                            long pwdExpiryTimeComputed = (long)result.Properties["msDS-UserPasswordExpiryTimeComputed"][0];
                            //check point
                            isSuccess = (pwdExpiryTimeComputed != 9223372036854775807);

                        }
                    }

                    if (!isSuccess)
                    {
                        DirectoryEntry pwdPolicy = new DirectoryEntry(
                            "LDAP://CN=testpassword,CN=Password Settings Container,CN=System,"
                            + adAdapter.rootDomainDN);
                        pwdPolicy.Properties["msDS-PSOAppliesTo"].Value = entryDN;
                        pwdPolicy.CommitChanges();
                        userEntry = new DirectoryEntry("LDAP://" + entryDN);
                        userEntry.RefreshCache(new string[] { "pwdLastSet" });
                        DirectorySearcher ds = new DirectorySearcher(userEntry, "(objectClass=*)",

                                new string[] { "pwdLastSet" },

                                System.DirectoryServices.SearchScope.Base);

                        System.DirectoryServices.SearchResult result = ds.FindOne();

                        string pwdLastSet = result.Properties["pwdLastSet"][0].ToString();

                        ds = new DirectorySearcher(pwdPolicy, "(objectClass=*)",

                                new string[] { "msDS-MaximumPasswordAge" },

                                System.DirectoryServices.SearchScope.Base);
                        result = ds.FindOne();

                        string maxPwdAge = result.Properties["msDS-MaximumPasswordAge"][0].ToString();

                        long msDSUserPwdExpiry = long.Parse(pwdLastSet) + long.Parse(maxPwdAge);


                    }
                    //capture req if issuccess is true.
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        isSuccess,
                        384,
                        @"In msDS-UserPasswordExpiryTimeComputed 
                        constructed attribute, TO be the object on which the attribute 
                        msDS-UserPasswordExpiryTimeComputed is read. If TO is a domain NC, where TO be the object on 
                        which the attribute msDS-UserPasswordExpiryTimeComputed is read and D be the root of the 
                        domain NC containing TO. The DC applies the following rules to determine the value of 
                        TO!msDS-UserPasswordExpiryTimeComputed:
                        1. If any of the ADS_UF_SMARTCARD_REQUIRED, ADS_UF_DONT_EXPIRE_PASSWD, 
                           ADS_UF_WORKSTATION_TRUST_ACCOUNT, ADS_UF_SERVER_TRUST_ACCOUNT, 
                           ADS_UF_INTERDOMAIN_TRUST_ACCOUNT bits is set in TO!userAccountControl, then 
                           TO!msDS-UserPasswordExpiryTimeComputed = 0x7FFFFFFFFFFFFFFF.
                        2. Else, if TO!pwdLastSet = null, or TO!pwdLastSet = 0, then 
                           TO!msDS-UserPasswordExpiryTimeComputed = 0.
                        3. Else, if Effective-MaximumPasswordAge = 0x8000000000000000, then 
                           TO!msDS-UserPasswordExpiryTimeComputed = 0x7FFFFFFFFFFFFFFF.
                        4. Else, TO!msDS-UserPasswordExpiryTimeComputed = TO!pwdLastSet + 
                           Effective-MaximumPasswordAge.");
                }
            }
                    #endregion


            #endregion

            #region msDS-User-Account-Control-Computed
            isDS = adAdapter.RunDSTestCases;
            if (isDS)
            {
                isSystemFlagval = true;
                isRootDseAttribute = true;
                isBacklinkattribute = true;
                listOfObjects = new List<string>();
                listOfObjects.Add("CN=" + adAdapter.DomainAdministratorName + ",CN=Users," + adAdapter.rootDomainDN);
                foreach (string entryDN in listOfObjects)
                {
                    DirectoryEntry userEntry = null;
                    adAdapter.GetObjectByDN(entryDN, out userEntry);

                    PropertyValueCollection userAccountControl = userEntry.Properties["userAccountControl"];
                    DirectoryEntry ControlComputedEntry;
                    adAdapter.GetObjectByDN("CN=ms-DS-User-Account-Control-Computed," + schemaNC, out ControlComputedEntry);
                    if (ControlComputedEntry.Properties["linkID"].Value == null)
                    {
                        isBacklinkattribute = false;
                    }
                    if (!RootDseAttribute.Contains(ControlComputedEntry.Name.ToString()))
                    {
                        isRootDseAttribute = false;
                    }
                    systemFlagVal = ParseSystemFlagsValue("FLAG_ATTR_IS_CONSTRUCTED");
                    if (attrIsConstructFlag != systemFlagVal)
                    {
                        isSystemFlagval = false;
                    }
                    DataSchemaSite.Assert.IsTrue(
                        isSystemFlagval
                        || isBacklinkattribute
                        || isRootDseAttribute,
                        "Constructed attribute has been defined.");
                    int userAccControlFlag = ParseUserAccountControlValue(
                        "ADS_UF_WORKSTATION_TRUST_ACCOUNT|ADS_UF_SERVER_TRUST_ACCOUNT|ADS_UF_INTERDOMAIN_TRUST_ACCOUNT");
                    bool isLoSet = false;
                    bool isPeSet = false;

                    if (((int.Parse(userAccountControl.Value.ToString()) & userAccControlFlag) == 0))
                    {
                        DirectoryEntry rootDomainEntry = null;
                        adAdapter.GetObjectByDN(adAdapter.rootDomainDN, out rootDomainEntry);

                        DirectorySearcher ds = new DirectorySearcher(rootDomainEntry, "(objectClass=*)",

                            new string[] { "lockoutDuration" },

                            System.DirectoryServices.SearchScope.Base);

                        System.DirectoryServices.SearchResult result = ds.FindOne();

                        long lockOutDuration = (long)result.Properties["lockoutDuration"][0];

                        ds = new DirectorySearcher(userEntry, "(objectClass=*)",

                            new string[] { "lockoutTime" },

                            System.DirectoryServices.SearchScope.Base);

                        result = ds.FindOne();

                        long lockOutTime = 0;
                        if (result.Properties.Contains("lockoutTime"))
                        {
                            lockOutTime = (long)result.Properties["lockoutTime"][0];
                        }

                        DateTime st = DateTime.Now;

                        isLoSet = (lockOutTime != 0)
                            && ((lockOutDuration < -9223372036854775808)
                            || ((lockOutDuration + st.Ticks) <= lockOutTime));
                    }
                    else
                    {
                        isLoSet = false;
                    }
                    userAccControlFlag = ParseUserAccountControlValue(
                        "ADS_UF_DONT_EXPIRE_PASSWD|ADS_UF_SMARTCARD_REQUIRED|" +
                        "ADS_UF_WORKSTATION_TRUST_ACCOUNT|ADS_UF_SERVER_TRUST_ACCOUNT|ADS_UF_INTERDOMAIN_TRUST_ACCOUNT");
                    if (((int.Parse(userAccountControl.Value.ToString()) & userAccControlFlag) == 0))
                    {
                        DirectorySearcher ds = new DirectorySearcher(userEntry, "(objectClass=*)",

                            new string[] { "pwdLastSet" },

                            System.DirectoryServices.SearchScope.Base);

                        System.DirectoryServices.SearchResult result = ds.FindOne();
                        string pwdLastSet = result.Properties["pwdLastSet"][0].ToString();

                        DirectoryEntry rootDomainEntry = null;
                        adAdapter.GetObjectByDN(adAdapter.rootDomainDN, out rootDomainEntry);

                        ds = new DirectorySearcher(rootDomainEntry, "(objectClass=*)",

                            new string[] { "maxPwdAge" },

                            System.DirectoryServices.SearchScope.Base);

                        result = ds.FindOne();

                        long maxPwdAge = (long)result.Properties["maxPwdAge"][0];
                        //9223372036854775808

                        DateTime st = DateTime.Now;
                        DateTime pLastSet = new DateTime(long.Parse(pwdLastSet));

                        isPeSet = ((pwdLastSet.Equals(null))
                            || (pwdLastSet.Equals("0"))
                            || ((maxPwdAge != -9223372036854775808)
                            && (st.Ticks - long.Parse(pwdLastSet)) > maxPwdAge));

                    }
                    else
                    {
                        isPeSet = false;
                    }
                    DirectorySearcher ds1 = new DirectorySearcher(userEntry, "(objectClass=*)",

                            new string[] { "msDS-User-Account-Control-Computed" },

                            System.DirectoryServices.SearchScope.Base);

                    System.DirectoryServices.SearchResult result1 = ds1.FindOne();
                    int uacComputed = (int)result1.Properties["msDS-User-Account-Control-Computed"][0];


                    if (isLoSet)
                    {
                        isLoSet = ((uacComputed & 16) == 16);
                    }
                    else
                    {
                        isLoSet = ((uacComputed & 16) == 0);
                    }

                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        isLoSet,
                        315,
                        @"In AD/DS, The value of TO!msDS-User-Account-Control-Computed is the bit pattern where 
                        LO (ADS_UF_LOCKOUT) is set if: 
                        (none of bits ADS_UF_WORKSTATION_TRUST_ACCOUNT, ADS_UF_SERVER_TRUST_ACCOUNT, 
                        ADS_UF_INTERDOMAIN_TRUST_ACCOUNT are set in TO!userAccountControl) and 
                        (TO!lockoutTime is nonzero and either (1) Effective-LockoutDuration (regarded as an unsigned 
                        quantity) < 0x8000000000000000, or (2) ST + Effective-LockoutDuration (regarded as a signed 
                        quantity) ≤ TO!lockoutTime ).");

                    if (isPeSet)
                    {
                        isPeSet = ((uacComputed & 8388608) == 8388608);
                    }
                    else
                    {
                        isPeSet = ((uacComputed & 8388608) == 0);
                    }
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        isPeSet,
                        316,
                        @"In AD/DS, The value of TO!msDS-User-Account-Control-Computed is the bit pattern where 
                        PE (ADS_UF_PASSWORD_EXPIRED) is set if: 
                        (none of bits ADS_UF_SMARTCARD_REQUIRED, ADS_UF_DONT_EXPIRE_PASSWD, ADS_UF_WORKSTATION_TRUST_ACCOUNT, 
                        ADS_UF_SERVER_TRUST_ACCOUNT, ADS_UF_INTERDOMAIN_TRUST_ACCOUNT are set in TO!userAccountControl) and 
                        (TO!pwdLastSet = null, or TO!pwdLastSet = 0, or (Effective-MaximumPasswordAge ≠ 0x8000000000000000 
                        and (ST - TO!pwdLastSet) > Effective-MaximumPasswordAge)).");
                }
            }

            #endregion

            #region allowedChildClassesEffective

            listOfObjects = new List<string>();
            listOfObjects.Add("CN=" + adAdapter.DomainAdministratorName + ",CN=Users," + adAdapter.rootDomainDN);
            isSystemFlagval = true;
            isRootDseAttribute = true;
            isBacklinkattribute = true;

            foreach (string entryDN in listOfObjects)
            {
                DirectoryEntry userEntry = null;
                adAdapter.GetObjectByDN(entryDN, out userEntry);

                userEntry.RefreshCache(new string[] { "allowedChildClasses", "allowedChildClassesEffective" });
                object[] childClasses = null;
                object[] actualChildClassesEffective = null;
                object[] expectedChildClassesEffective = null;

                childClasses = (object[])userEntry.Properties["allowedChildClasses"].Value;
                actualChildClassesEffective = (object[])userEntry.Properties["allowedChildClassesEffective"].Value;
                expectedChildClassesEffective = new object[actualChildClassesEffective.Length];
                DirectoryEntry ClassesEffectiveEntry;
                adAdapter.GetObjectByDN("CN=Allowed-Child-Classes-Effective," + schemaNC, out ClassesEffectiveEntry);
                if (ClassesEffectiveEntry.Properties["linkID"].Value == null)
                {
                    isBacklinkattribute = false;
                }
                if (!RootDseAttribute.Contains(ClassesEffectiveEntry.Name.ToString()))
                {
                    isRootDseAttribute = false;
                }
                systemFlagVal = ParseSystemFlagsValue("FLAG_ATTR_IS_CONSTRUCTED");
                if (attrIsConstructFlag != systemFlagVal)
                {
                    isSystemFlagval = false;
                }
                DataSchemaSite.Assert.IsTrue(
                    isSystemFlagval
                    || isBacklinkattribute
                    || isRootDseAttribute,
                    "Constructed attribute has been defined.");
                sd = (IADsSecurityDescriptor)userEntry.Properties["ntsecuritydescriptor"].Value;
                dacl = (IADsAccessControlList)sd.DiscretionaryAcl;
                bool isCreateChild = false;
                bool fAllowPrinicipals = false;
                bool spcOfO = false;
                bool isConfig = false;
                bool isSchema = false;
                foreach (IADsAccessControlEntry ace in dacl)
                {
                    if (ace.Trustee.ToLower().Contains("domain admins"))
                    {
                        if ((ace.AccessMask & 1) == 1)
                        {
                            isCreateChild = true;
                            break;
                        }
                    }
                }

                int loopVar = 0;
                foreach (string childClass in childClasses)
                {
                    spcOfO = SPCForDS(childClass);

                    isConfig = entryDN.ToLower().Contains("schema");
                    isSchema = entryDN.ToLower().Contains("configuration");

                    if (
                        isCreateChild
                        && (fAllowPrinicipals || !isConfig || !spcOfO)
                        && (fAllowPrinicipals || !isSchema || !spcOfO))
                    {
                        expectedChildClassesEffective[loopVar++] = childClass;
                    }
                }
                isSuccess = false;
                for (loopVar = 0; loopVar < expectedChildClassesEffective.Length; loopVar++)
                {
                    if (((string)actualChildClassesEffective[loopVar]).Equals(((string)expectedChildClassesEffective[loopVar])))
                    {
                        isSuccess = true;
                    }
                    else
                    {
                        isSuccess = false;
                        break;
                    }
                }

                DataSchemaSite.CaptureRequirementIfIsTrue(
                    isSuccess,
                    288,
                    @"If the DC is running as AD DS, then let 
                    fAllowPrincipals = false. Let TO be the object from which the allowedChildClassesEffective 
                    attribute is being read, contains each object class O in TO!allowedChildClasses such that:
                    -> ((TO!nTSecurityDescriptor grants RIGHT_DS_CREATE_CHILD via a simple access control entry 
                       (ACE) to the client for instantiating an object beneath TO) or (TO.nTSecurityDescriptor 
                       grants RIGHT_DS_CREATE_CHILD via an object-specific ACE to the client for instantiating an 
                       object of class O beneath TO)) 
                    -> and (fAllowPrincipals or (not TO!distinguishedName in config NC) or (not SPC(O)))
                    -> and (fAllowPrincipals or (not TO!distinguishedName in schema NC) or (not SPC(O))).");
            }

            #endregion

            #region Common ConstructedAttributes for DS and LDS

            CommonAttributesValidation(dirEntry, true);

            #endregion

            #region msDS-SiteName

            if (serverOS >= OSVersion.WinSvr2008R2)
            {
                isSystemFlagval = true;
                isRootDseAttribute = true;
                isBacklinkattribute = true;
                DirectoryEntry siteObjectEntry;
                DirectoryEntry computerObject;
                string siteObjectRDN = null;
                //Getting the directory entry for the sites container in the config NC
                if (!adAdapter.GetObjectByDN("CN=Sites,CN=Configuration," + adAdapter.rootDomainDN, out siteObjectEntry))
                {
                    DataSchemaSite.Assert.IsTrue(
                        false,
                        "CN=Sites,CN=Configuration," +
                        adAdapter.rootDomainDN +
                        " Object is not found in server");
                }
                //Getting the childrens of the sites container.
                DirectoryEntries siteObjectChildEntries = siteObjectEntry.Children;
                foreach (DirectoryEntry childs in siteObjectChildEntries)
                {
                    if (childs.Name.Equals("CN=Default-First-Site-Name"))
                    {
                        //Checking the sites objects in each child.
                        if (childs.Properties["objectCategory"].Value.ToString().ToLower().Contains("cn=site"))
                        {
                            //For each site object finding the directory entry of the servers, i.e, parent of each server.
                            DirectoryEntry serverParentEntry = childs.Children.Find("CN=Servers");
                            //Getting the directory entry of the child of the server object.
                            serverEntry = serverParentEntry.Children.Find("CN=" + adAdapter.PDCNetbiosName);
                            //Getting the nTDSDSA object from the server, i.e, child of the server.
                            DirectoryEntry nTDSDSAEntry = serverEntry.Children.Find("CN=NTDS Settings");
                            //Getting the directory entry for the computer container in the domain NC
                            if (!adAdapter.GetObjectByDN("CN=" + adAdapter.PDCNetbiosName + ",OU=Domain Controllers," + adAdapter.rootDomainDN, out computerObject))
                            {
                                DataSchemaSite.Assert.IsTrue(
                                    false,
                                    "CN="
                                    + adAdapter.PDCNetbiosName
                                    + ",OU=Domain Controllers,"
                                    + adAdapter.rootDomainDN
                                    + " Object is not found in server");
                            }
                            //Getting the constructed attribute msDS-SiteName from server object and the nTDSDSA object.
                            serverEntry.RefreshCache(new string[] { "msDS-SiteName" });
                            nTDSDSAEntry.RefreshCache(new string[] { "msDS-SiteName" });
                            DirectoryEntry SiteNameEntry;
                            adAdapter.GetObjectByDN("CN=ms-DS-SiteName," + schemaNC, out SiteNameEntry);
                            if (SiteNameEntry.Properties["linkID"].Value == null)
                            {
                                isBacklinkattribute = false;
                            }
                            if (!RootDseAttribute.Contains(SiteNameEntry.Name.ToString()))
                            {
                                isRootDseAttribute = false;
                            }
                            systemFlagVal = ParseSystemFlagsValue("FLAG_ATTR_IS_CONSTRUCTED");
                            if (attrIsConstructFlag != systemFlagVal)
                            {
                                isSystemFlagval = false;
                            }
                            DataSchemaSite.Assert.IsTrue(
                                isSystemFlagval
                                || isBacklinkattribute
                                || isRootDseAttribute,
                                "Constructed attribute has been defined.");
                            siteObjectRDN = childs.Properties["cn"].Value.ToString();
                            //Checking whether the value of the constructed attribute msDS-SiteName of nTDSDSA object and server object is equal
                            //to the value of RDN of the site object.
                            //if yes, then capture R357.
                            DataSchemaSite.Log.Add(LogEntryKind.Debug, "Verify MS-ADTS-Schema_R357");

                            DataSchemaSite.CaptureRequirementIfIsTrue(
                                (nTDSDSAEntry.Properties["msDS-SiteName"].Value.ToString() == siteObjectRDN
                                || serverEntry.Properties["msDS-SiteName"].Value.ToString() == siteObjectRDN),
                                357,
                                @"In the construction of msDS-SiteName attribute,
                            If TO is an nTDSDSA object or a server object, then TO!msDS-SiteName is equal to the 
                            value of the RDN of the site object under which TO is located,where TO be the object 
                            on which msDS-SiteName is being read.");

                            computerObject.RefreshCache(new string[] { "msDS-SiteName" });

                            //Checking whether the value of the consturcted attribute of msDS-SiteName of computer object
                            //is equal to the value of the construted attribute of msDS-SiteName of server object.
                            //if yes, then capture R358                            
                            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                                computerObject.Properties["msDS-SiteName"].Value.ToString(),
                                serverEntry.Properties["msDS-SiteName"].Value.ToString(),
                                358,
                                @"In the construction of msDS-SiteName attribute,
                            If TO is a computer object, then  TS be the server object named by 
                            TO!serverReferenceBL. TO!msDS-SiteName equals TS!msDS-SiteName, 
                            where TO be the object on which msDS-SiteName is being read.");
                        }
                    }
                }
                    //Checking that the constructed attribute msDS-SiteName is not present for the object if it is
                    //neither computer, server, nor nTDSDSA object.
                    //If yes, then capture R359.
                    dirEntry.RefreshCache(new string[] { "msDS-SiteName" });

                    DataSchemaSite.CaptureRequirementIfAreEqual<object>(
                        null,
                        dirEntry.Properties["msDS-SiteName"].Value,
                        359,
                        @"In the construction of msDS-SiteName attribute, if TO is neither a computer, server, 
                    nor nTDSDSA object, then TO!msDS-SiteName is not present, where TO be the object on which 
                    msDS-SiteName is being read.");           
            }

            #endregion

            #region msDS-isRODC

            if (serverOS >= OSVersion.WinSvr2008)
            {
                isSystemFlagval = true;
                isRootDseAttribute = true;
                isBacklinkattribute = true;
                DirectoryEntry siteObjectEntry;
                DirectoryEntry computerObject;
                //Getting the directory entry for the sites container in the config NC
                if (!adAdapter.GetObjectByDN("CN=Sites,CN=Configuration," + adAdapter.rootDomainDN, out siteObjectEntry))
                {
                    DataSchemaSite.Assert.IsTrue(
                        false,
                        "CN=Sites,CN=Configuration,"
                        + adAdapter.rootDomainDN
                        + " Object is not found in server");
                }
                //Getting the childrens of the sites container.
                DirectoryEntries siteObjectChildEntries = siteObjectEntry.Children;
                foreach (DirectoryEntry childs in siteObjectChildEntries)
                {
                    //Checking the sites objects in each child.
                    if (childs.Properties["objectCategory"].Value.ToString().ToLower().Contains("cn=site"))
                    {
                        if (childs.Name.Equals("CN=Default-First-Site-Name"))
                        {
                            //For each site object finding the directory entry of the servers, i.e, parent of each server.
                            DirectoryEntry serverParentEntry = childs.Children.Find("CN=Servers");
                            //Getting the directory entry of the child of the server object.
                            serverEntry = serverParentEntry.Children.Find("CN=" + adAdapter.PDCNetbiosName);
                            //Getting the directory entry of the child of the second server object.
                            DirectoryEntry secondaryServerEntry = serverParentEntry.Children.Find("CN=" + adAdapter.RODCNetbiosName);
                            //Getting the nTDSDSA object from the server, i.e, child of the server.
                            DirectoryEntry nTDSDSAEntry = serverEntry.Children.Find("CN=NTDS Settings");
                            //Getting the nTDSDSA object from the second server, i.e, child of the server.
                            DirectoryEntry nTDSDSAEntryOfSecondaryServer = secondaryServerEntry.Children.Find("CN=NTDS Settings");
                            DirectoryEntry schemaEntry, classObjectOfNTDSDSA;
                            if (!adAdapter.GetObjectByDN("CN=Schema,CN=Configuration," + adAdapter.rootDomainDN, out schemaEntry))
                            {
                                DataSchemaSite.Assert.IsTrue(
                                    false,
                                    "CN=Schema,CN=Configuration,"
                                    + adAdapter.rootDomainDN
                                    + " Object is not found in server");
                            }
                            if (!adAdapter.GetObjectByDN("CN=" + adAdapter.PDCNetbiosName + ",OU=Domain Controllers," + adAdapter.rootDomainDN, out computerObject))
                            {
                                DataSchemaSite.Assert.IsTrue(
                                    false,
                                    "CN="
                                    + adAdapter.PDCNetbiosName
                                    + ",OU=Domain Controllers,"
                                    + adAdapter.rootDomainDN
                                    + " Object is not found in server");
                            }
                            //Getting the directory entry for the class NTDS-DSA of the schema NC.
                            classObjectOfNTDSDSA = schemaEntry.Children.Find("CN=NTDS-DSA");
                            string nTDSDSAObjectDN = classObjectOfNTDSDSA.Properties["distinguishedName"].Value.ToString();
                            //Checking whether DN of the nTDSDSA object classSchema is equal to the objectCategory value of the
                            //nTDSDSA object.
                            if (nTDSDSAObjectDN == nTDSDSAEntry.Properties["objectCategory"].Value.ToString())
                            {
                                nTDSDSAEntry.RefreshCache(new string[] { "msDS-isRODC" });
                                DirectoryEntry RODCEntry;
                                adAdapter.GetObjectByDN("CN=ms-DS-isRODC," + schemaNC, out RODCEntry);
                                if (RODCEntry.Properties["linkID"].Value == null)
                                {
                                    isBacklinkattribute = false;
                                }
                                if (!RootDseAttribute.Contains(RODCEntry.Name.ToString()))
                                {
                                    isRootDseAttribute = false;
                                }
                                systemFlagVal = ParseSystemFlagsValue("FLAG_ATTR_IS_CONSTRUCTED");
                                if (attrIsConstructFlag != systemFlagVal)
                                {
                                    isSystemFlagval = false;
                                }
                                DataSchemaSite.Assert.IsTrue(
                                    isSystemFlagval
                                    || isBacklinkattribute
                                    || isRootDseAttribute,
                                    "Constructed attribute has been defined.");
                                //Checking whether constructed attribute msDS-isRODC value of nTDSDSA object is false or not.
                                if (nTDSDSAEntry.Properties["msDS-isRODC"].Value.ToString().ToLower() == "false")
                                {
                                    AttributeisRODCisFalse(computerObject, serverEntry, nTDSDSAEntry);
                                }
                            }
                            //Checking whether DN of the nTDSDSA object classSchema is not equal to the objectCategory value 
                            //of the nTDSDSA object.
                            if (nTDSDSAObjectDN != nTDSDSAEntry.Properties["objectCategory"].Value.ToString())
                            {
                                nTDSDSAEntry.RefreshCache(new string[] { "msDS-isRODC" });
                                //Checking whether constructed attribute msDS-isRODC value of nTDSDSA object is true or not.
                                if (nTDSDSAEntry.Properties["msDS-isRODC"].Value.ToString().ToLower() == "true")
                                {
                                    AttributeisRODCisTrue(computerObject, serverEntry, nTDSDSAEntry);
                                }
                            }
                            //Checking whether DN of the nTDSDSA object classSchema is equal to the objectCategory value of 
                            //the nTDSDSA object.                        
                            if (nTDSDSAObjectDN == nTDSDSAEntryOfSecondaryServer.Properties["objectCategory"].Value.ToString())
                            {
                                //Checking whether constructed attribute msDS-isRODC value of nTDSDSA object is false or not.
                                nTDSDSAEntryOfSecondaryServer.RefreshCache(new string[] { "msDS-isRODC" });
                                if (nTDSDSAEntryOfSecondaryServer.Properties["msDS-isRODC"].Value.ToString().ToLower() == "false")
                                {
                                    AttributeisRODCisFalse(computerObject, serverEntry, nTDSDSAEntry);
                                }
                            }
                            //Checking whether DN of the nTDSDSA object classSchema is not equal to the objectCategory value
                            //of the nTDSDSA object.                        
                            if (nTDSDSAObjectDN != nTDSDSAEntryOfSecondaryServer.Properties["objectCategory"].Value.ToString())
                            {
                                //Checking whether constructed attribute msDS-isRODC value of nTDSDSA object is true or not.
                                nTDSDSAEntryOfSecondaryServer.RefreshCache(new string[] { "msDS-isRODC" });
                                if (nTDSDSAEntryOfSecondaryServer.Properties["msDS-isRODC"].Value.ToString().ToLower() == "true")
                                {
                                    AttributeisRODCisTrue(computerObject, serverEntry, nTDSDSAEntry);
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            #region allowedChildClasses

            List<string> expectedAllowedChildClasses = new List<string>();
            List<string> objectClasses = new List<string>();
            List<string> classes = new List<string>();
            List<string> actualAllowedClasses = new List<string>();
            List<string> possSuperiors = new List<string>();
            isSystemFlagval = true;
            isRootDseAttribute = true;
            isBacklinkattribute = true;

            //Get the domainNC object from server.
            if (!adAdapter.GetObjectByDN("CN=Users," + adAdapter.rootDomainDN, out dirEntry))
            {
                DataSchemaSite.Assume.IsTrue(
                    false,
                    "CN=Users,"
                    + adAdapter.rootDomainDN
                    + " Object is not found in server");
            }
            //Get its first child.
            foreach (DirectoryEntry child in dirEntry.Children)
            {
                object[] tempObjects = (object[])child.Properties["objectClass"].Value;
                string tempClassName = tempObjects[tempObjects.Length - 1].ToString();
                if (tempClassName.ToLower().Equals("user"))
                {
                    dirEntry = child;
                    break;
                }
            }

            //Collect its objectClass values.
            foreach (String values in dirEntry.Properties["objectClass"])
            {
                objectClasses.Add(values);
            }

            //Collect its allowedChildClasses values.
            dirEntry.RefreshCache(new string[] { "allowedChildClasses" });
            DirectoryEntry ChildClassesEntry;
            adAdapter.GetObjectByDN("CN=Allowed-Child-Classes," + schemaNC, out ChildClassesEntry);
            if (ChildClassesEntry.Properties["linkID"].Value == null)
            {
                isBacklinkattribute = false;
            }
            if (!RootDseAttribute.Contains(ChildClassesEntry.Name.ToString()))
            {
                isRootDseAttribute = false;
            }
            systemFlagVal = ParseSystemFlagsValue("FLAG_ATTR_IS_CONSTRUCTED");
            if (attrIsConstructFlag != systemFlagVal)
            {
                isSystemFlagval = false;
            }
            DataSchemaSite.Assert.IsTrue(
                isSystemFlagval
                || isBacklinkattribute
                || isRootDseAttribute,
                "Constructed attribute has been defined.");
            foreach (String values in dirEntry.Properties["allowedChildClasses"])
            {
                actualAllowedClasses.Add(values.ToLower());
            }

            //Construct this attribute value from Test suite.
            if (!adAdapter.GetObjectByDN("CN=Schema,CN=Configuration," + adAdapter.rootDomainDN, out dirEntry))
            {
                DataSchemaSite.Assume.IsTrue(
                    false,
                    "CN=Schema,CN=Configuration,"
                    + adAdapter.rootDomainDN
                    + " Object is not found in server");
            }
            //Evaluating the conditions given in ADTS regarding allowedChildClasses.
            DirectoryEntries allSchemaEntries = dirEntry.Children;
            foreach (DirectoryEntry entry in allSchemaEntries)
            {
                if (entry.Properties["distinguishedName"].Value.ToString().ToLower().Contains("cn=schema"))
                {
                    PropertyValueCollection objectClassValues = entry.Properties["objectClass"];
                    if (
                        objectClassValues.Contains("classSchema")
                        && entry.Properties["systemOnly"].Value.ToString().ToLower().Contains("false")
                        && !entry.Properties["objectClassCategory"].Value.ToString().ToLower().Contains("2")
                        && !entry.Properties["objectClassCategory"].Value.ToString().ToLower().Contains("3"))
                    {
                        //Get the entry object from model.
                        ModelObject modelEntry = null;
                        dcModel.TryGetClass(entry.Properties["ldapdisplayname"].Value.ToString().ToLower(), out modelEntry);
                        if (modelEntry == null)
                            continue;
                        ConstructedAttributeHelper helper = new ConstructedAttributeHelper();
                        helper.possSuperiorList = new List<string>();

                        //Get its possSuperiors.
                        possSuperiors = helper.GetPossSuperiorsList(modelEntry, dcModel);
                        foreach (string clsval in objectClasses)
                        {
                            //If this list contains the class name, add it in the list.
                            if (possSuperiors.Contains(clsval))
                            {
                                expectedAllowedChildClasses.Add(entry.Properties["ldapDisplayName"].Value.ToString().ToLower());
                                break;
                            }
                        }
                    }
                }
                continue;
            }
            actualAllowedClasses.Sort();
            expectedAllowedChildClasses.Sort();

            //Checking passing condition.
            bool isR279Satisfied = true;
            if (isParentGUIDEqualToObjectGUID == false)
            {
                isR279Satisfied = false;
            }
            if (actualAllowedClasses.Count != expectedAllowedChildClasses.Count)
            {
                //If counts are not equal.
                isR279Satisfied = false;
            }
            else
            {
                //Check value by value.
                serverValue = actualAllowedClasses.ToArray();
                actualValue = expectedAllowedChildClasses.ToArray();
                for (int i = 0; i < actualAllowedClasses.Count; i++)
                {
                    if (!serverValue[i].Equals(actualValue[i]))
                    {
                        //If any one is not exist, make the condition false.
                        isR279Satisfied = false;
                        wrongLocation = i;
                        break;
                    }
                }
            }
            //Validating MS-AD_Schema_R279.
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isR279Satisfied,
                279,
                @"The value of TO!allowedChildClasses, where
                    TO be the object from which the allowedChildClasses attribute is being read, is the set of 
                    lDAPDisplayNames read from each Object O where(O.distinguishedName is in the schema NC)and 
                    (O!objectClass is classSchema)and (not O!systemOnly)and (not O!objectClassCategory is 2)and 
                    (not O!objectClassCategory is 3)and (there exists C in TO!objectClass such that C is in 
                    POSSSUPERIORS(O)).");

            #endregion

            #region allowedAttributes

            List<string> serverAllowedAttributes = new List<string>();
            List<string> modelAllowedAttributes = new List<string>();
            sampleClasses = new List<string>();
            DirectoryEntry reqEnt = null;
            isSystemFlagval = true;
            isRootDseAttribute = true;
            isBacklinkattribute = true;
            //Add sample classes here.
            sampleClasses.Add("CN=Account-Expires,CN=Schema,CN=Configuration," + adAdapter.rootDomainDN);

            //For each sample class,
            foreach (string sampleClass in sampleClasses)
            {
                serverAllowedAttributes = new List<string>();
                modelAllowedAttributes = new List<string>();

                //Reading the allowedAttributes for given object from server.
                if (!adAdapter.GetObjectByDN(sampleClass, out reqEnt))
                {
                    DataSchemaSite.Assume.IsTrue(false, sampleClass + " Object is not found in server");
                }
                reqEnt.RefreshCache(new string[] { "allowedAttributes" });
                foreach (string item in reqEnt.Properties["allowedAttributes"])
                {
                    serverAllowedAttributes.Add(item);
                }
                DirectoryEntry AllowedAttributesEntry;
                adAdapter.GetObjectByDN("CN=Allowed-Attributes," + schemaNC, out AllowedAttributesEntry);
                if (AllowedAttributesEntry.Properties["linkID"].Value == null)
                {
                    isBacklinkattribute = false;
                }
                if (!RootDseAttribute.Contains(AllowedAttributesEntry.Name.ToString()))
                {
                    isRootDseAttribute = false;
                }
                systemFlagVal = ParseSystemFlagsValue("FLAG_ATTR_IS_CONSTRUCTED");
                if (attrIsConstructFlag != systemFlagVal)
                {
                    isSystemFlagval = false;
                }
                DataSchemaSite.Assert.IsTrue(
                    isSystemFlagval
                    || isBacklinkattribute
                    || isRootDseAttribute,
                    "Constructed attribute has been defined.");
                serverAllowedAttributes.Sort();

                //Reading the allowedAttributes for given object from Model.
                modelAllowedAttributes = ConstructedAttributes.GetAllowedAttributes((string)reqEnt.Properties["ldapdisplayname"].Value, dcModel);
                modelAllowedAttributes.Sort();

                //Checking condition.
                bool isR290Satisfied = true;
                wrongLocation = -1;
                if (serverAllowedAttributes.Count != modelAllowedAttributes.Count)
                {
                    isR290Satisfied = false;
                }
                else
                {
                    serverValue = serverAllowedAttributes.ToArray();
                    actualValue = modelAllowedAttributes.ToArray();
                    for (int i = 0; i < serverAllowedAttributes.Count; i++)
                    {
                        if (!serverValue[i].Equals(actualValue[i]))
                        {
                            isR290Satisfied = false;
                            wrongLocation = i;
                            break;
                        }
                    }
                }
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    isR290Satisfied,
                    290,
                    @"The value of TO!allowedAttributes, where TO be the object from which the allowedAttributes 
                        attribute is being read,  is the set of lDAPDisplayNames read from each Object O where: 
                        (O.dn is in the schema NC) and (O!objectClass is attributeSchema) and 
                        (there exists C in TO!objectClass such that O is in CLASSATTS(C)).");
            }

            #endregion

            #region possibleInferiors

            List<string> serverPossInferiors = new List<string>();
            List<string> modelPossInferiors = new List<string>();
            sampleClasses = new List<string>();
            reqEnt = null;
            isSystemFlagval = true;
            isRootDseAttribute = true;
            isBacklinkattribute = true;

            //Adding Sample classes for this testing purpose.
            sampleClasses.Add("CN=User,CN=Schema,CN=Configuration," + adAdapter.rootDomainDN);
            sampleClasses.Add("CN=Computer,CN=Schema,CN=Configuration," + adAdapter.rootDomainDN);

            //For each sample class,
            foreach (string sampleClass in sampleClasses)
            {
                serverPossInferiors = new List<string>();
                modelPossInferiors = new List<string>();

                //Getting possibleInferiors of this sample class from server.
                if (!adAdapter.GetObjectByDN(sampleClass, out reqEnt))
                {
                    DataSchemaSite.Assume.IsTrue(false, sampleClass + " Object is not found in server");
                }
                reqEnt.RefreshCache(new string[] { "possibleInferiors" });
                foreach (string item in reqEnt.Properties["possibleInferiors"])
                {
                    serverPossInferiors.Add(item);
                }
                DirectoryEntry PossibleInferiorsEntry;
                adAdapter.GetObjectByDN("CN=Possible-Inferiors," + schemaNC, out PossibleInferiorsEntry);
                if (PossibleInferiorsEntry.Properties["linkID"].Value == null)
                {
                    isBacklinkattribute = false;
                }
                if (!RootDseAttribute.Contains(PossibleInferiorsEntry.Name.ToString()))
                {
                    isRootDseAttribute = false;
                }
                systemFlagVal = ParseSystemFlagsValue("FLAG_ATTR_IS_CONSTRUCTED");
                if (attrIsConstructFlag != systemFlagVal)
                {
                    isSystemFlagval = false;
                }
                DataSchemaSite.Assert.IsTrue(
                    isSystemFlagval
                    || isBacklinkattribute
                    || isRootDseAttribute,
                    "Constructed attribute has been defined.");
                serverPossInferiors.Sort();

                //Getting possibleInferiors of this sample class from Model.
                string ldapdisplayname = (string)reqEnt.Properties["ldapdisplayname"].Value;
                if (ldapdisplayname.ToLower() != "user".ToLower())
                {
                    modelPossInferiors = ConstructedAttributes.GetPossibleInferiors(ldapdisplayname, dcModel);
                }
                else
                {
                    modelPossInferiors = ConstructedAttributes.GetPossibleInferiors(ldapdisplayname, dcModel);
                    //When DirectoryEntry's property is "user", it should contain "CN=User,CN=Schema,CN=Confi"'s default object category: 
                    //CN=Person, CN=Schema, CN=Configuration. 
                    modelPossInferiors.AddRange(ConstructedAttributes.GetPossibleInferiors("person", dcModel));
                }
                modelPossInferiors.Sort();

                string reqEntPath = (string)reqEnt.Properties["distinguishedname"].Value;

                //Checking condition.
                bool isR332Satisfied = true;
                if (serverPossInferiors.Count != modelPossInferiors.Count)
                {

                    isR332Satisfied = false;
                }
                else
                {
                    string[] serverValues = serverPossInferiors.ToArray();
                    string[] modelValues = modelPossInferiors.ToArray();
                    int loopVar = 0;
                    for (loopVar = 0; loopVar < serverValues.Length; loopVar++)
                    {
                        if (!serverValues[loopVar].ToLower().Equals(modelValues[loopVar].ToLower()))
                        {
                            isR332Satisfied = false;
                            break;
                        }
                    }
                }
                //Validating the MS-AD_Schema_R332.
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    isR332Satisfied,
                    332,
                    @"The value of TO!possibleInferiors, 
                        where TO be the object from which the possibleInferiors attribute is being read. and 
                        C be the classSchema object corresponding to TO!governsID.The value of 
                        TO!possibleInferiors is the set of O!governsID for each Object O where(O is in the 
                        schema NC)and (O!objectClass is classSchema)and (not O!systemOnly)and (not 
                        O!objectClassCategory is 2)and (not O!objectClassCategory is 3)and ((C is contained in 
                        POSSSUPERIORS(O)).");
            }
            #endregion

            #region msDS-isGC
            if (serverOS >= OSVersion.WinSvr2008)
            {
                DirectoryEntry siteObjectEntry;
                DirectoryEntry computerObject;
                bool actualIsGc = false;
                bool expectedIsGc = false;
                isSystemFlagval = true;
                isRootDseAttribute = true;
                isBacklinkattribute = true;

                //Get site's container object.
                if (!adAdapter.GetObjectByDN("CN=Sites,CN=Configuration," + adAdapter.rootDomainDN, out siteObjectEntry))
                {
                    DataSchemaSite.Assume.IsTrue(
                        false,
                        "CN=Sites,CN=Configuration,"
                        + adAdapter.rootDomainDN
                        + " Object is not found in server");
                }
                DirectoryEntries siteObjectChildEntries = siteObjectEntry.Children;

                //For each child of site's container,
                foreach (DirectoryEntry child in siteObjectChildEntries)
                {
                    //If this child is site object,
                    if (child.Properties["objectCategory"].Value.ToString().ToLower().Contains("cn=site"))
                    {
                        if (child.Name.Equals("CN=Default-First-Site-Name"))
                        {
                            //Find Server's object which is of type serverContainer.
                            if (child.Children.Find("CN=Servers").Equals(null))
                                continue;
                            DirectoryEntry serverParentEntry = child.Children.Find("CN=Servers");

                            //Under Servers, find object with the name of this computer.
                            //Since each DC has one server object with this computer name.
                            if (serverParentEntry.Children.Find("CN=" + adAdapter.PDCNetbiosName) == null)
                                continue;
                            serverEntry = serverParentEntry.Children.Find("CN=" + adAdapter.PDCNetbiosName);

                            //Under this server object, one nTSDSDSA object will be there.
                            if (serverEntry.Children.Find("CN=NTDS Settings") == null)
                                continue;
                            DirectoryEntry nTDSDSAEntry = serverEntry.Children.Find("CN=NTDS Settings");

                            //Now find computer object.
                            if (
                                !adAdapter.GetObjectByDN(
                                "CN="
                                + adAdapter.PDCNetbiosName
                                + ",OU=Domain Controllers,"
                                + adAdapter.rootDomainDN,
                                out computerObject))
                            {
                                DataSchemaSite.Assume.IsTrue(
                                    false,
                                    "CN="
                                    + adAdapter.PDCNetbiosName
                                    + ",OU=Domain Controllers,"
                                    + adAdapter.rootDomainDN
                                    + " Object is not found in server");
                            }

                            //Validating MS-AD_Schema_R352.
                            expectedIsGc = false;
                            actualIsGc = false;
                            child.RefreshCache(new string[] { "msds-isgc" });
                            if (child.Properties["msds-isgc"].Value == null)
                            {
                                //The value is null. It shows that if this object is not any of nTDSDSA,
                                //computer or server, this value is not set.
                                expectedIsGc = true;
                                actualIsGc = true;
                                DataSchemaSite.CaptureRequirementIfAreEqual<bool>(
                                    expectedIsGc,
                                    actualIsGc,
                                    369,
                                    @"In msDS-isGC constructed attribute, if TO is not a nTDSDSA, computer, or server object, 
                                then TO.msDS- isGC  is not present, where TO be the object on which msDS-isGC is being read.");
                            }
                            DirectoryEntry IsGCEntry;
                            adAdapter.GetObjectByDN("CN=ms-DS-isGC," + schemaNC, out IsGCEntry);
                            if (IsGCEntry.Properties["linkID"].Value == null)
                            {
                                isBacklinkattribute = false;
                            }
                            if (!RootDseAttribute.Contains(IsGCEntry.Name.ToString()))
                            {
                                isRootDseAttribute = false;
                            }
                            systemFlagVal = ParseSystemFlagsValue("FLAG_ATTR_IS_CONSTRUCTED");
                            if (attrIsConstructFlag != systemFlagVal)
                            {
                                isSystemFlagval = false;
                            }
                            DataSchemaSite.Assert.IsTrue(
                                isSystemFlagval
                                || isBacklinkattribute
                                || isRootDseAttribute,
                                "Constructed attribute has been defined.");

                            //Validating MS-AD_Schema_R370.
                            nTDSDSAEntry.RefreshCache(new string[] { "msds-isgc" });
                            if (nTDSDSAEntry.Properties["msds-isgc"].Value != null)
                            {
                                expectedIsGc = false;
                                actualIsGc = false;
                                expectedIsGc = (bool)nTDSDSAEntry.Properties["msds-isgc"].Value;
                                actualIsGc = VerifyMsdsIsGC(nTDSDSAEntry);
                                DataSchemaSite.CaptureRequirementIfAreEqual<bool>(
                                    expectedIsGc,
                                    actualIsGc,
                                    370,
                                    "In "
                                    + "msDS-isGC constructed attribute, if TO is an nTDSDSA object then TO!msDS-isGC "
                                    + "iff TO!options has the NTDSDSA_OPT_IS_GC bit set, where TO be the object on "
                                    + "which msDS-isGC is being read.");

                            }

                            //MS-AD_Schema_R371.
                            serverEntry.RefreshCache(new string[] { "msds-isgc" });
                            if (serverEntry.Properties["msDS-isGC"].Value != null)
                            {
                                expectedIsGc = false;
                                actualIsGc = false;
                                expectedIsGc = (bool)serverEntry.Properties["msDS-isGC"].Value;
                                actualIsGc = VerifyMsdsIsGC(serverEntry);
                                DataSchemaSite.CaptureRequirementIfAreEqual<bool>(
                                    expectedIsGc,
                                    actualIsGc,
                                    371,
                                    @"In msDS-isGC constructed attribute, if TO is a server object, TN be the nTDSDSA "
                                + "object whose DN is \"CN=NTDS Settings,\" prepended to the DN of TO,Then   TN!msDS-isGC "
                                + "iff TO!options has the NTDSDSA_OPT_IS_GC bit set, where TO be the object on "
                                + "which msDS-isGC is being read .");
                            }

                            //MS-AD_Schema_R372.
                            computerObject.RefreshCache(new string[] { "msds-isgc" });
                            if (computerObject.Properties["msDS-isGC"].Value != null)
                            {
                                expectedIsGc = false;
                                actualIsGc = false;
                                expectedIsGc = (bool)computerObject.Properties["msDS-isGC"].Value;
                                actualIsGc = VerifyMsdsIsGC(computerObject);
                                DataSchemaSite.CaptureRequirementIfAreEqual<bool>(
                                    expectedIsGc,
                                    actualIsGc,
                                    372,
                                    "In msDS-isGC constructed attribute, if TO is a server object: Let TS be the server "
                                    + "object named by TO!serverReferenceBL,Then TO!msDS-isGC iff TO!options has the "
                                    + "NTDSDSA_OPT_IS_GC bit set, where TO be the object on which msDS-isGC is being read.");
                            }
                        }
                    }
                }
            }

            #endregion

            #region msDS-LocalEffectiveDeletionTime

            if (serverOS >= OSVersion.WinSvr2008R2)
            {
                DirectoryEntry DeletionTimeEntry;
                adAdapter.GetObjectByDN("CN=ms-DS-Local-Effective-Deletion-Time," + schemaNC, out DeletionTimeEntry);
                if (DeletionTimeEntry.Properties["linkID"].Value == null)
                {
                    isBacklinkattribute = false;
                }
                if (!RootDseAttribute.Contains(DeletionTimeEntry.Name.ToString()))
                {
                    isRootDseAttribute = false;
                }
                systemFlagVal = ParseSystemFlagsValue("FLAG_ATTR_IS_CONSTRUCTED");
                if (attrIsConstructFlag != systemFlagVal)
                {
                    isSystemFlagval = false;
                }
                DataSchemaSite.Assert.IsTrue(
                    isSystemFlagval
                    || isBacklinkattribute
                    || isRootDseAttribute,
                    "Constructed attribute has been defined.");

                //Verify MS-AD_Schema requirement:MS-AD_Schema_R4282
                //The msDS-LocalEffectiveDeletionTime attribute itsself is a new property for the Windows 
                //Server 2008R2 DS and LDS
                //And before calling the method,it has been determined the platform for AD DS and AD LDS.
                DataSchemaSite.CaptureRequirement(
                    "MS-ADTS-Schema",
                    4282,
                    @"The msDS-LocalEffectiveDeletionTime attribute exists on AD DS and AD LDS, 
                beginning with Windows Server� 2008 R2 operating system.");
            }

            #endregion

            #region msDS-LocalEffectiveRecycleTime
            if (serverOS >= OSVersion.WinSvr2008R2)
            {
                DirectoryEntry RecycleTimeEntry;
                adAdapter.GetObjectByDN("CN=ms-DS-Local-Effective-Recycle-Time," + schemaNC, out RecycleTimeEntry);
                if (RecycleTimeEntry.Properties["linkID"].Value == null)
                {
                    isBacklinkattribute = false;
                }
                if (!RootDseAttribute.Contains(RecycleTimeEntry.Name.ToString()))
                {
                    isRootDseAttribute = false;
                }
                systemFlagVal = ParseSystemFlagsValue("FLAG_ATTR_IS_CONSTRUCTED");
                if (attrIsConstructFlag != systemFlagVal)
                {
                    isSystemFlagval = false;
                }
                DataSchemaSite.Assert.IsTrue(
                    isSystemFlagval
                    || isBacklinkattribute
                    || isRootDseAttribute,
                    "Constructed attribute has been defined.");
                //Verify MS-AD_Schema requirement:MS-AD_Schema_R4292
                //The msDS-LocalEffectiveDeletionTime attribute itsself is a new property for the Windows Server 
                //2008R2 DS and LDS
                //And before calling the method,it has been determined the platform for AD DS and AD LDS.
                DataSchemaSite.CaptureRequirement(
                    "MS-ADTS-Schema",
                    4292,
                    @"The msDS-LocalEffectiveRecycleTime attribute exists on AD DS and AD LDS, 
                    beginning with Windows Server� 2008 R2 operating system.");
            }

            #endregion

            #region msDS-Auxiliary-Classes

            List<string> serverMsdsAuxClass = new List<string>();
            List<string> modelMsdsAuxClass = new List<string>();
            sampleClasses = new List<string>();
            reqEnt = null;
            isSystemFlagval = true;
            isRootDseAttribute = true;
            isBacklinkattribute = true;

            //Add sample classes here.
            sampleClasses.Add("CN=User,CN=Schema,CN=Configuration," + adAdapter.rootDomainDN);

            foreach (string sampleClass in sampleClasses)
            {
                serverMsdsAuxClass = new List<string>();
                modelMsdsAuxClass = new List<string>();

                //Getting msDS-Auxiliary-Classes of this sample class from server.
                if (!adAdapter.GetObjectByDN(sampleClass, out reqEnt))
                {
                    DataSchemaSite.Assume.IsTrue(false, sampleClass + " Object is not found in server");
                }
                reqEnt.RefreshCache(new string[] { "msds-auxiliary-classes" });
                foreach (string item in reqEnt.Properties["msDS-Auxiliary-Classes"])
                {
                    serverMsdsAuxClass.Add(item);
                }
                DirectoryEntry AuxiliaryClassesEntry;
                adAdapter.GetObjectByDN("CN=ms-DS-Auxiliary-Classes," + schemaNC, out AuxiliaryClassesEntry);
                if (AuxiliaryClassesEntry.Properties["linkID"].Value == null)
                {
                    isBacklinkattribute = false;
                }
                if (!RootDseAttribute.Contains(AuxiliaryClassesEntry.Name.ToString()))
                {
                    isRootDseAttribute = false;
                }
                systemFlagVal = ParseSystemFlagsValue("FLAG_ATTR_IS_CONSTRUCTED");
                if (attrIsConstructFlag != systemFlagVal)
                {
                    isSystemFlagval = false;
                }
                DataSchemaSite.Assert.IsTrue(
                    isSystemFlagval
                    || isBacklinkattribute
                    || isRootDseAttribute,
                    "Constructed attribute has been defined.");
                serverMsdsAuxClass.Sort();

                //Getting msDS-Auxiliary-Classes of this sample class from Model.
                modelMsdsAuxClass = ConstructedAttributes.GetMsdsAuxiliaryClasses((string)reqEnt.Properties["ldapdisplayname"].Value, dcModel);
                modelMsdsAuxClass.Sort();

                //Checking condition.
                bool isR324Satisfied = true;
                if (serverMsdsAuxClass.Count == modelMsdsAuxClass.Count)
                {
                    foreach (string element in serverMsdsAuxClass)
                    {
                        if (!modelMsdsAuxClass.Contains(element))
                        {
                            isR324Satisfied = false;
                            break;
                        }
                    }
                }
                else
                {
                    isR324Satisfied = false;
                }

                //MS-AD_Schema_R324.
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    isR324Satisfied,
                    324,
                    @"The value of TO!msDS-Auxiliary-Classes, 
                    where TO be the object from which the msDS-Auxiliary-Classes attribute is being read, is 
                    the set of lDAPDisplayNames from each Object O such that (O is in TO!objectClass) and 
                    (O is not in SUPCLASSES(Most Specific class of TO)).");
            }

            #endregion

            #region primaryGroupToken
            int serverPrimariyGroupToken = 0;
            int modelPrimaryGroupToken = 0;
            isSystemFlagval = true;
            isRootDseAttribute = true;
            isBacklinkattribute = true;
            sampleClasses = new List<string>();
            reqEnt = null;

            // The requirement is "If TO is not a group, no value is returned for TO!primaryGroupToken, where 
            // TO be the object from which the primaryGroupToken attribute is being read, when this attribute is 
            // read from TO."
            // So, if the sample object is not of type Group, there is no value set for primaryGroupToken 
            // attribute. When querrying the primaryGroupToken attribute from non-group object, it will be null. 
            // If it is null, we set the value of serverPrimariyGroupToken as -1. Since this attribute is 
            // constructed from the objectSid of the object, it will also be null. So we set the value of 
            // modelPrimaryGroupToken as -1. When capturing the requirement, both values should have the -1 as 
            // value. If this is the case, then for non-group object, the primaryGroupToken value will not be set.
            // Thus the requirement is passed.
            // Add sample classes here.
            sampleClasses.Add("CN=Users,CN=Builtin," + adAdapter.rootDomainDN);
            sampleClasses.Add("CN=Users," + adAdapter.rootDomainDN);

            foreach (string sampleClass in sampleClasses)
            {
                serverPrimariyGroupToken = 0;
                modelPrimaryGroupToken = 0;

                //Getting msDS-Auxiliary-Classes of this sample class from server.
                if (!adAdapter.GetObjectByDN(sampleClass, out reqEnt))
                {
                    DataSchemaSite.Assume.IsTrue(false, sampleClass + " Object is not found in server");
                }

                PropertyValueCollection objectClasValues = reqEnt.Properties["objectClass"];

                //If it is group object,
                if (objectClasValues.Contains("group"))
                {
                    //Get the Primary group token.
                    reqEnt.RefreshCache(new string[] { "primarygrouptoken", "objectsid" });
                    serverPrimariyGroupToken = (int)reqEnt.Properties["primarygrouptoken"].Value;
                    DirectoryEntry GroupTokenEntry;
                    adAdapter.GetObjectByDN("CN=Primary-Group-Token," + schemaNC, out GroupTokenEntry);
                    if (GroupTokenEntry.Properties["linkID"].Value == null)
                    {
                        isBacklinkattribute = false;
                    }
                    if (!RootDseAttribute.Contains(GroupTokenEntry.Name.ToString()))
                    {
                        isRootDseAttribute = false;
                    }
                    systemFlagVal = ParseSystemFlagsValue("FLAG_ATTR_IS_CONSTRUCTED");
                    if (attrIsConstructFlag != systemFlagVal)
                    {
                        isSystemFlagval = false;
                    }
                    DataSchemaSite.Assert.IsTrue(
                        isSystemFlagval
                        || isBacklinkattribute
                        || isRootDseAttribute,
                        "Constructed attribute has been defined.");
                    //Construct the Primary group token.
                    //Get the objectSid of this object.
                    byte[] byteArray = (byte[])reqEnt.Properties["objectSid"].Value;

                    //Convert this value into Security Identifier.
                    System.Security.Principal.SecurityIdentifier identifier = new System.Security.Principal.SecurityIdentifier(byteArray, 0);
                    string primaryGroupTokenString = identifier.Value.ToString();

                    //The last part of this value is the required primaryGroupToken value.
                    primaryGroupTokenString = primaryGroupTokenString.Substring(primaryGroupTokenString.LastIndexOf('-') + 1);

                    modelPrimaryGroupToken = int.Parse(primaryGroupTokenString);

                    //Capturing the requirement, if both values are equal, the requirement is passed.
                    //MS-AD_Schema_R301.
                    DataSchemaSite.CaptureRequirementIfAreEqual<int>(
                        serverPrimariyGroupToken,
                        modelPrimaryGroupToken,
                        301,
                        @"The value of TO!primaryGroupToken, where TO be the object 
                        from which the primaryGroupToken attribute is being read, is the RID from 
                        TO!objectSid when there exists C in TO!objectClass such that C is the group class.");
                }
                //If it is not group object,
                else
                {
                    reqEnt.RefreshCache(new string[] { "primarygrouptoken", "objectsid" });
                    //Get the Primary group token. For non-group object this value will be null.
                    if (reqEnt.Properties["primarygrouptoken"].Value != null)
                    {
                        serverPrimariyGroupToken = (int)reqEnt.Properties["primarygrouptoken"].Value;
                    }
                    else
                    {
                        //If it is null, serverPrimariyGroupToken will be set to -1.
                        serverPrimariyGroupToken = -1;
                    }
                    //Construct the Primary group token.
                    //Since serverPrimariyGroupToken attribute is constructed from objectSid of this object,
                    //it will also be null.
                    if (reqEnt.Properties["objectSid"].Value != null)
                    {
                        byte[] byteArray = (byte[])reqEnt.Properties["objectSid"].Value;
                        System.Security.Principal.SecurityIdentifier identifier = new System.Security.Principal.SecurityIdentifier(byteArray, 0);
                        string primaryGroupTokenString = identifier.Value.ToString();
                        primaryGroupTokenString = primaryGroupTokenString.Substring(primaryGroupTokenString.LastIndexOf('-') + 1);

                        modelPrimaryGroupToken = int.Parse(primaryGroupTokenString);
                    }
                    else
                    {
                        //If it is null, modelPrimaryGroupToken set to -1.
                        modelPrimaryGroupToken = -1;
                    }
                    //Capturing the requirement, both values should have the -1. if it is, the requirement is passed.
                    //MS-AD_Schema_R302.
                    DataSchemaSite.CaptureRequirementIfAreEqual<int>(
                        serverPrimariyGroupToken,
                        modelPrimaryGroupToken,
                        302,
                        @"If TO is not a group, no value is returned for 
                        TO!primaryGroupToken, where TO be the object from which the primaryGroupToken attribute
                        is being read, when this attribute is read from TO.");
                }
            }

            #endregion

            #region entryTTL

            //entryTTL = value of msDS-Entry-Time-To-Die - Current Time.
            isSystemFlagval = true;
            isRootDseAttribute = true;
            isBacklinkattribute = true;
            //Create dynamic object on server.
            distinguishedName = "CN=TestDynamicObjectUser,CN=Users," + adAdapter.rootDomainDN;

            DateTime now = DateTime.UtcNow;
            string timeFormat = now.Year.ToString();
            if (now.Month.ToString().Length < 2)
            {
                timeFormat += "0" + now.Month.ToString();
            }
            else
            {
                timeFormat += now.Month.ToString();
            }
            if (now.Day.ToString().Length < 2)
            {
                timeFormat += "0" + now.Day.ToString();
            }
            else
            {
                timeFormat += now.Day.ToString();
            }
            if (now.Hour.ToString().Length < 2)
            {
                timeFormat += "0" + now.Hour.ToString();
            }
            else
            {
                timeFormat += now.Hour.ToString();
            }
            //minimum entryTTL is 900 seconds by default
            now = now.AddMinutes(16);
            if (now.Minute.ToString().Length < 2)
            {
                timeFormat += "0" + now.Minute.ToString();
            }
            else
            {
                timeFormat += now.Minute.ToString();
            }
            if (now.Second.ToString().Length < 2)
            {
                timeFormat += "0" + now.Second.ToString();
            }
            else
            {
                timeFormat += now.Second.ToString();
            }

            List<DirectoryAttribute> atts = new List<DirectoryAttribute>();
            atts.Add(new DirectoryAttribute("sAMAccountName", new String[] { "dynamicuser_12" }));
            atts.Add(new DirectoryAttribute("objectClass", new String[] { "dynamicobject", "User" }));
            atts.Add(new DirectoryAttribute("msDS-Entry-Time-To-Die", new String[] { timeFormat + ".0Z" }));
            string ret = AdLdapClient.Instance().ConnectAndBind(
                adAdapter.PDCNetbiosName,
                adAdapter.PDCIPAddr,
                Convert.ToInt32(adAdapter.ADDSPortNum),
                adAdapter.DomainAdministratorName,
                adAdapter.DomainUserPassword,
                adAdapter.PrimaryDomainNetBiosName,
                AuthType.Basic | AuthType.Kerberos);
            if (ret.Equals("Success_STATUS_SUCCESS"))
            {
                DirectoryEntry dynamicObject = null;

                if (adAdapter.GetObjectByDN(distinguishedName, out dynamicObject))
                {
                    AdLdapClient.Instance().DeleteObject(distinguishedName,null);
                }
                ret = AdLdapClient.Instance().AddObject(distinguishedName,atts,null);
                if (!ret.Equals("Success_STATUS_SUCCESS"))
                {
                    DataSchemaSite.Assume.Fail("The specified dynamic object can't be created.");
                }
                else
                {
                    DirectoryEntry TTLEntry;
                    adAdapter.GetObjectByDN("CN=ms-DS-Entry-Time-To-Die," + schemaNC, out TTLEntry);
                    if (TTLEntry.Properties["linkID"].Value == null)
                    {
                        isBacklinkattribute = false;
                    }
                    if (!RootDseAttribute.Contains(TTLEntry.Name.ToString()))
                    {
                        isRootDseAttribute = false;
                    }
                    systemFlagVal = ParseSystemFlagsValue("FLAG_ATTR_IS_CONSTRUCTED");
                    if (attrIsConstructFlag != systemFlagVal)
                    {
                        isSystemFlagval = false;
                    }
                    DataSchemaSite.Assert.IsTrue(
                        isSystemFlagval
                        || isBacklinkattribute
                        || isRootDseAttribute,
                        "Constructed attribute has been defined.");

                    if (!adAdapter.GetObjectByDN(distinguishedName, out dynamicObject))
                    {
                        DataSchemaSite.Assume.IsTrue(false, distinguishedName + " Object is not found in server");
                    }
                    dynamicObject.RefreshCache(new string[] { "entryttl" });
                    dynamicObject.RefreshCache(new string[] { "msDS-Entry-Time-To-Die" });

                    //Get current time.
                    //DateTime currentTime = DateTime.Now;
                    //Get the current UTC time because msDS-Entry-Time-To-Die is UTC Time
                    DateTime currentTime = DateTime.UtcNow;

                    //Get the value of msDS-Entry-Time-To-Die.
                    DateTime objectTime = (DateTime)dynamicObject.Properties["msDS-Entry-Time-To-Die"].Value;

                    //Get the values
                    //Subtract the msDS-Entry-Time-To-Die value from the current time.
                    //If actualTimeRemaining < 0, it should set to 0.
                    uint actualTimeRemaining;
                    if (DateTime.Compare(currentTime, objectTime) > 0)
                    {
                        actualTimeRemaining = 0;
                    }
                    else
                    {
                        
                        actualTimeRemaining = (uint)objectTime.Subtract(currentTime).TotalSeconds;
                        if (actualTimeRemaining > 0xFFFFFFFF)
                            actualTimeRemaining = 0xFFFFFFFF;
                    }
                    
                    //Read the entryTTL value of the dynamic object which is the expected entryTTL value.
                    //entryTTL is exactly the expectedTimeRemaining.
                    int expectedTimeRemaining = int.Parse(dynamicObject.Properties["entryttl"].Value.ToString());

                    //Checking condition.
                    //The -10 is because the time different of the value read from the server and the calculation time.
                    bool isR304AndR305Satisfied = false;
                    if (actualTimeRemaining == expectedTimeRemaining
                        || actualTimeRemaining >= expectedTimeRemaining - 10)
                    {
                        isR304AndR305Satisfied = true;
                    }
                    else
                    {
                        isR304AndR305Satisfied = false;
                    }

                    //MS-AD_Schema_R304.
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        isR304AndR305Satisfied,
                        304,
                        @"The value of TO!entryTTL, where TO be the 
                    object from which the entryTTL attribute is being read, is the number of seconds in 
                    TO!msDS-Entry-Time-To-Die minus the current system time, and is constrained to the range 
                    0..0xFFFFFFFF by returning 0 if the difference is less than 0.");

                    //MS-AD_Schema_R305.
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        isR304AndR305Satisfied,
                        305,
                        @"The value of TO!entryTTL, where TO be the 
                    object from which the entryTTL attribute is being read, is the number of seconds in 
                    TO!msDS-Entry-Time-To-Die minus the current system time, and is constrained to the range 
                    0..0xFFFFFFFF by returning 0xFFFFFFFF if the difference is greater than 0xFFFFFFFF.");
                }
            }
            AdLdapClient.Instance().DeleteObject(distinguishedName,null);
            AdLdapClient.Instance().Unbind();
            #endregion

            //There are Asserts have been in every constructed attributes to verify the three criteria(isSystemFlagval 
            //|| isBacklinkattribute || isRootDseAttribute).  
            //This requirement can been cover  in the end of the test case since all assertion have passed. 
            //Verify MS-AD_Schema requirement: MS-AD_Schema_R4279.
            DataSchemaSite.CaptureRequirement(
                "MS-ADTS-Schema",
                4279,
                @"Regardless of this property[that the constructed attributes values are computed by using other 
                attributes], constructed attributes are defined to be those attributes that meet one of the 
                following three criteria:
                1. The attributeSchema object's systemFlags attribute has the ATTR_IS_CONSTRUCTED bit (section 2.2.1) 
                set to one.
                2. The attribute is a rootDSE attribute (sections 3.1.1.3.2 and 3.1.1.3.3).
                3. The attribute is a back link attribute.");
        }

        #region isRODCMethods

        ///*MS-AD_Schema_R346:  AttributeisRODCisTrue() method is called only after satisfying the condition where
        // it is calling. i.e, if the condition "If objectCategory of an object not equals the DN of the classSchema
        // object for the nTDSDSA object class, then msDS-isRODC of that object is true." is satisfied then only 
        // the method AttributeisRODCisTrue() will be called.

        // *MS-AD_Schema_R348: As specified in ADTS document section: 3.1.1.4.5.30. "The nTDSDSA object whose DN 
        // is "CN=NTDS Settings," prepended to the DN of Server Object. And this nTDSDSA object must have to 
        // satisfy the above statement(Validated for MS-AD_Schema_R346). That's why this requirement is included 
        // in the method AttributeisRODCisTrue() and placed under the if condition where nTDSDSA object whose DN 
        // is "CN=NTDS Settings," prepended to the DN of Server Object.

        // *MS-AD_Schema_R350: As specified in ADTS document setion 3.1.1.4.5.30. "The server object named by 
        // serverReferenceBL of the computer object satisfies the above two conditions which are validated for 
        // the requirements(MS-AD_Schema_R346, MS-AD_Schema_R348). So this requirements is included in the method 
        // AttributeisRODCisTrue() and placed under the if condition where "nTDSDSA object whose DN is 
        // "CN=NTDS Settings," prepended to the DN of Server Object." and "The server object named by 
        // serverReferenceBL of the computer object". */

        // This method is called when the value of the consturcted attribute msDS-isRODC is true.
        private static void AttributeisRODCisTrue(DirectoryEntry computerObject, DirectoryEntry serverEntry, DirectoryEntry nTDSDSAEntry)
        {
            //MS-AD_Schema_R363
            //This requirement is captured when the objectCategory of nTDSDSA object is not equals to the DN of 
            //the classSchema
            //object for the nTDSDSA object class.
            DataSchemaSite.CaptureRequirement(
                363,
                @"While constructing msDS-isRODC attribute, if TO is an nTDSDSA object and if TO!objectCategory does not 
                equal the DN of the classSchema object for the nTDSDSA object class then TO!msDS-isRODC is true, where 
                TO be the object on which msDS-isRODC is being read.");
            if (nTDSDSAEntry.Properties["distinguishedName"].Value.ToString() == "CN=NTDS Settings," +
                serverEntry.Properties["distinguishedName"].Value.ToString())
            {
                //MS-AD_Schema_R365
                //This requirement is captured when the DN of nTDSDSA object is 'CN=NTDS Settings,' prepended to the DN 
                //of server object.                
                DataSchemaSite.CaptureRequirement(
                365,
                @"While constructing msDS-isRODC attribute, if TO is a server object: Let TN be the nTDSDSA object "
                + "whose DN is \"CN=NTDS Settings,\" prepended to the DN of TO and if TN!objectCategory does not "
                + "equal the DN of the classSchema object for the nTDSDSA object class, then TN!msDS-isRODC is "
                + "true, where TO be the object on which msDS-isRODC is being read.");

                //MS-AD_Schema_R367
                //This requirement is captured when the server object is named by serverReferenceBL of computer object.
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    computerObject.Properties["serverReferenceBL"].Value.ToString(),
                    serverEntry.Properties["distinguishedName"].Value.ToString(),
                    367,
                    "While constructing msDS-isRODC " +
                    "attribute, if TO is a computer object: Let TS be the server object named by " +
                    "TO!serverReferenceBL and if TS!objectCategory does not equals the DN of the classSchema" +
                    " object for the nTDSDSA object class, then TS!msDS-isRODC is true, where TO be the object" +
                    " on which msDS-isRODC is being read.");
            }
        }

        // MS-AD_Schema_R345:  AttributeisRODCisFalse() method is called only after satisfying the condition where
        // it is calling. i.e, if the condition "If objectCategory of an object equals the DN of the classSchema
        // object for the nTDSDSA object class, then msDS-isRODC of that object is false." is satisfied then only 
        // the method AttributeisRODCisFalse() will be called.

        // MS-AD_Schema_R347: As specified in ADTS document section: 3.1.1.4.5.30. "The nTDSDSA object whose DN 
        // is "CN=NTDS Settings," prepended to the DN of Server Object. And this nTDSDSA object must have to 
        // satisfy the above statement(Validated for MS-AD_Schema_R345). That's why this requirement is included 
        // in the method AttributeisRODCisFalse() and placed under the if condition where nTDSDSA object whose DN 
        // is "CN=NTDS Settings," prepended to the DN of Server Object.

        // MS-AD_Schema_R349: As specified in ADTS document setion 3.1.1.4.5.30. "The server object named by 
        // serverReferenceBL of the computer object satisfies the above two conditions which are validated for 
        // the requirements(MS-AD_Schema_R345, MS-AD_Schema_R347). So this requirements is included in the method 
        // AttributeisRODCisFalse() and placed under the if condition where "nTDSDSA object whose DN is 
        // "CN=NTDS Settings," prepended to the DN of Server Object." and "The server object named by 
        // serverReferenceBL of the computer object".

        // This method is called when the value of the consturcted attribute msDS-isRODC is false.
        private static void AttributeisRODCisFalse(DirectoryEntry computerObject, DirectoryEntry serverEntry, DirectoryEntry nTDSDSAEntry)
        {
            //MS-AD_Schema_R345
            //This requirement is captured when the objectCategory of nTDSDSA object is not equals to the DN of the classSchema
            //Object for the nTDSDSA object class.            
            DataSchemaSite.CaptureRequirement(
                362,
                @"While constructing msDS-isRODC "
                + "attribute, if TO is an nTDSDSA object and if TO!objectCategory equals the DN of the classSchema"
                + " object for the nTDSDSA object class then TO!msDS-isRODC is false, where TO be the object on "
                + "which msDS-isRODC is being read .");

            //MS-AD_Schema_R364
            //This requirement is captured when the DN of nTDSDSA object is 'CN=NTDS Settings,' prepended to the 
            //DN of server object.                
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "CN=NTDS Settings," + serverEntry.Properties["distinguishedName"].Value.ToString(),
                nTDSDSAEntry.Properties["distinguishedName"].Value.ToString(),
                364,
                "While constructing msDS-isRODC attribute, if TO is a server object: Let TN be the nTDSDSA object "
                + "whose DN is \"CN=NTDS Settings,\" prepended to the DN of TO and if TN!objectCategory equals "
                + "the DN of the classSchema object for the nTDSDSA object class, then TN!msDS-isRODC is "
                + "false, where TO be the object on which msDS-isRODC is being read.");

            //MS-AD_Schema_R366
            //This requirement is captured when the server object is named by serverReferenceBL of computer object.
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                computerObject.Properties["serverReferenceBL"].Value.ToString(),
                serverEntry.Properties["distinguishedName"].Value.ToString(),
                366,
                "While constructing msDS-isRODC "
                + "attribute, if TO is a computer object: Let TS be the server object named by "
                + "TO!serverReferenceBL and if TS!objectCategory equals the DN of the classSchema "
                + "object for the nTDSDSA object class,then TS!msDS-isRODC is false. "
                + "where TO be the object on which msDS-isRODC is being read.");
        }

        #endregion

        #endregion

        #region LDSConstructedAttributes Validation.

        // <summary>
        /// This method validates the requirements under 
        /// LDSConstructedAttributes Scenario.
        /// </summary>
        public void ValidateLDSConstructedAttributes()
        {
            //Directory Entry for holding the objects required.
            DirectoryEntry dirEntry = new DirectoryEntry();

            //Common Variable used.
            int wrongLocation = -1;
            string[] serverValue = null;
            string[] actualValue = null;
            List<string> sampleClasses = new List<string>();
            string distinguishedName = String.Empty;

            //Checking the some of the constructed attributes for the object Users.
            if (!adAdapter.GetLdsObjectByDN("CN=Account-Expires,CN=Schema,CN=Configuration," + adAdapter.LDSRootObjectName, out dirEntry))
            {
                DataSchemaSite.Assert.IsTrue(
                    false,
                    "CN=Account-Expires,CN=Schema,CN=Configuration,"
                    + adAdapter.LDSRootObjectName
                    + " Object is not found in server");
            }

            CommonAttributesValidation(dirEntry, false);

            #region allowedChildClasses

            List<string> expectedAllowedChildClasses = new List<string>();
            List<string> objectClasses = new List<string>();
            List<string> classes = new List<string>();
            List<string> actualAllowedClasses = new List<string>();
            List<string> possSuperiors = new List<string>();

            //Get the configurationNC object from server.
            if (!adAdapter.GetLdsObjectByDN("CN=Configuration," + adAdapter.LDSRootObjectName, out dirEntry))
            {
                DataSchemaSite.Assume.IsTrue(
                    false,
                    "CN=Configuration,"
                    + adAdapter.LDSRootObjectName
                    + " Object is not found in server");
            }
            //Get its first child.
            foreach (DirectoryEntry child in dirEntry.Children)
            {
                dirEntry = child;
                break;
            }

            //Collect its objectClass values.
            foreach (String values in dirEntry.Properties["objectClass"])
            {
                objectClasses.Add(values);
            }

            //Collect its allowedChildClasses values.
            dirEntry.RefreshCache(new string[] { "allowedChildClasses" });
            foreach (String values in dirEntry.Properties["allowedChildClasses"])
            {
                actualAllowedClasses.Add(values.ToLower());
            }

            //Construct this attribute value from Test suite.
            if (!adAdapter.GetLdsObjectByDN("CN=Schema,CN=Configuration," + adAdapter.LDSRootObjectName, out dirEntry))
            {
                DataSchemaSite.Assume.IsTrue(
                    false,
                    "CN=Schema,CN=Configuration,"
                    + adAdapter.LDSRootObjectName
                    + " Object is not found in server");
            }

            //Evaluating the conditions given in ADTS regarding allowedChildClasses.
            DirectoryEntries allSchemaEntries = dirEntry.Children;
            foreach (DirectoryEntry entry in allSchemaEntries)
            {
                if (entry.Properties["distinguishedName"].Value.ToString().ToLower().Contains("cn=schema"))
                {
                    PropertyValueCollection objectClassValues = entry.Properties["objectClass"];
                    if (
                        objectClassValues.Contains("classSchema")
                        && entry.Properties["systemOnly"].Value.ToString().ToLower().Contains("false")
                        && !entry.Properties["objectClassCategory"].Value.ToString().ToLower().Contains("2")
                        && !entry.Properties["objectClassCategory"].Value.ToString().ToLower().Contains("3"))
                    {
                        //Get the entry object from model.
                        ModelObject modelEntry = null;
                        adamModel.TryGetClass(entry.Properties["ldapdisplayname"].Value.ToString().ToLower(), out modelEntry);
                        if (modelEntry == null)
                            continue;
                        ConstructedAttributeHelper helper = new ConstructedAttributeHelper();
                        helper.possSuperiorList = new List<string>();

                        //Get its possSuperiors.
                        possSuperiors = helper.GetPossSuperiorsList(modelEntry, adamModel);
                        foreach (string clsval in objectClasses)
                        {
                            //If this list contains the class name, add it in the list.
                            if (possSuperiors.Contains(clsval))
                            {
                                expectedAllowedChildClasses.Add(entry.Properties["ldapDisplayName"].Value.ToString().ToLower());
                                break;
                            }
                        }
                    }
                }
                continue;
            }
            actualAllowedClasses.Sort();
            expectedAllowedChildClasses.Sort();

            //Checking passing condition.
            bool isR2795Satisfied = true;
            if (actualAllowedClasses.Count != expectedAllowedChildClasses.Count)
            {
                //If counts are not equal.
                isR2795Satisfied = false;
            }
            else
            {
                //Check value by value.
                serverValue = actualAllowedClasses.ToArray();
                actualValue = expectedAllowedChildClasses.ToArray();
                for (int i = 0; i < actualAllowedClasses.Count; i++)
                {
                    if (!serverValue[i].Equals(actualValue[i]))
                    {
                        //If any one is not exist, make the condition false.
                        isR2795Satisfied = false;
                        wrongLocation = i;
                        break;
                    }
                }
            }
            //Validating MS-AD_Schema_R279.
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isR2795Satisfied,
                279,
                @"The value of TO!allowedChildClasses, where
                    TO be the object from which the allowedChildClasses attribute is being read, is the set of 
                    lDAPDisplayNames read from each Object O where(O.distinguishedName is in the schema NC)and 
                    (O!objectClass is classSchema)and (not O!systemOnly)and (not O!objectClassCategory is 2)and 
                    (not O!objectClassCategory is 3)and (there exists C in TO!objectClass such that C is in 
                    POSSSUPERIORS(O)).");

            #endregion

            #region allowedAttributes

            List<string> serverAllowedAttributes = new List<string>();
            List<string> modelAllowedAttributes = new List<string>();
            sampleClasses = new List<string>();
            DirectoryEntry reqEnt = null;

            //Add sample classes here.
            sampleClasses.Add("CN=Account-Expires,CN=Schema,CN=Configuration," + adAdapter.LDSRootObjectName);

            //For each sample class,
            foreach (string sampleClass in sampleClasses)
            {
                serverAllowedAttributes = new List<string>();
                modelAllowedAttributes = new List<string>();

                //Reading the allowedAttributes for given object from server.
                if (!adAdapter.GetLdsObjectByDN(sampleClass, out reqEnt))
                {
                    DataSchemaSite.Assume.IsTrue(false, sampleClass + " Object is not found in server");
                }
                reqEnt.RefreshCache(new string[] { "allowedAttributes" });
                foreach (string item in reqEnt.Properties["allowedAttributes"])
                {
                    serverAllowedAttributes.Add(item);
                }
                serverAllowedAttributes.Sort();

                //Reading the allowedAttributes for given object from Model.
                modelAllowedAttributes = ConstructedAttributes.GetAllowedAttributes((string)reqEnt.Properties["ldapdisplayname"].Value, adamModel);
                modelAllowedAttributes.Sort();

                //Checking condition.
                bool isR290Satisfied = true;
                wrongLocation = -1;
                if (serverAllowedAttributes.Count != modelAllowedAttributes.Count)
                {
                    isR290Satisfied = false;
                }
                else
                {
                    serverValue = serverAllowedAttributes.ToArray();
                    actualValue = modelAllowedAttributes.ToArray();
                    for (int i = 0; i < serverAllowedAttributes.Count; i++)
                    {
                        if (!serverValue[i].Equals(actualValue[i]))
                        {
                            isR290Satisfied = false;
                            wrongLocation = i;
                            break;
                        }
                    }
                }
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    isR290Satisfied,
                    290,
                    @"The value of TO!allowedAttributes, where TO be the object from which the allowedAttributes 
                        attribute is being read,  is the set of lDAPDisplayNames read from each Object O where: 
                        (O.dn is in the schema NC) and (O!objectClass is attributeSchema) and 
                        (there exists C in TO!objectClass such that O is in CLASSATTS(C)).");
            }

            #endregion

            #region possibleInferiors

            List<string> serverPossInferiors = new List<string>();
            List<string> modelPossInferiors = new List<string>();
            sampleClasses = new List<string>();
            reqEnt = null;

            //Adding Sample classes for this testing purpose.
            sampleClasses.Add("CN=DMD,CN=Schema,CN=Configuration," + adAdapter.LDSRootObjectName);
            sampleClasses.Add("CN=Configuration,CN=Schema,CN=Configuration," + adAdapter.LDSRootObjectName);

            //For each sample class,
            foreach (string sampleClass in sampleClasses)
            {
                serverPossInferiors = new List<string>();
                modelPossInferiors = new List<string>();

                //Getting possibleInferiors of this sample class from server.
                if (!adAdapter.GetLdsObjectByDN(sampleClass, out reqEnt))
                {
                    DataSchemaSite.Assume.IsTrue(false, sampleClass + " Object is not found in server");
                }
                reqEnt.RefreshCache(new string[] { "possibleInferiors" });
                foreach (string item in reqEnt.Properties["possibleInferiors"])
                {
                    serverPossInferiors.Add(item);
                }
                serverPossInferiors.Sort();

                //Getting possibleInferiors of this sample class from Model.
                modelPossInferiors = ConstructedAttributes.GetPossibleInferiors((string)reqEnt.Properties["ldapdisplayname"].Value, adamModel);
                modelPossInferiors.Sort();

                string reqEntPath = (string)reqEnt.Properties["distinguishedname"].Value;

                //Checking condition.
                bool isR332Satisfied = true;
                if (serverPossInferiors.Count != modelPossInferiors.Count)
                {
                    isR332Satisfied = false;
                }
                else
                {
                    string[] serverValues = serverPossInferiors.ToArray();
                    string[] modelValues = modelPossInferiors.ToArray();
                    int loopVar = 0;
                    for (loopVar = 0; loopVar < serverValues.Length; loopVar++)
                    {
                        if (!serverValues[loopVar].ToLower().Equals(modelValues[loopVar].ToLower()))
                        {
                            isR332Satisfied = false;
                            break;
                        }
                    }
                }
                //MS-AD_Schema_R332.
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    isR332Satisfied,
                    332,
                    @"The value of TO!possibleInferiors, 
                        where TO be the object from which the possibleInferiors attribute is being read. and 
                        C be the classSchema object corresponding to TO!governsID.The value of 
                        TO!possibleInferiors is the set of O!governsID for each Object O where(O is in the 
                        schema NC)and (O!objectClass is classSchema)and (not O!systemOnly)and (not 
                        O!objectClassCategory is 2)and (not O!objectClassCategory is 3)and ((C is contained in 
                        POSSSUPERIORS(O)).");
            }

            #endregion

            #region msDS-Auxiliary-Classes

            List<string> serverMsdsAuxClass = new List<string>();
            List<string> modelMsdsAuxClass = new List<string>();
            sampleClasses = new List<string>();
            reqEnt = null;

            //Add sample classes here.
            sampleClasses.Add("CN=User,CN=Schema,CN=Configuration," + adAdapter.LDSRootObjectName);

            foreach (string sampleClass in sampleClasses)
            {
                serverMsdsAuxClass = new List<string>();
                modelMsdsAuxClass = new List<string>();

                //Getting msDS-Auxiliary-Classes of this sample class from server.
                if (!adAdapter.GetLdsObjectByDN(sampleClass, out reqEnt))
                {
                    DataSchemaSite.Assume.IsTrue(false, sampleClass + " Object is not found in server");
                }
                reqEnt.RefreshCache(new string[] { "msds-auxiliary-classes" });
                foreach (string item in reqEnt.Properties["msDS-Auxiliary-Classes"])
                {
                    serverMsdsAuxClass.Add(item);
                }
                serverMsdsAuxClass.Sort();

                //Getting msDS-Auxiliary-Classes of this sample class from Model.
                modelMsdsAuxClass = ConstructedAttributes.GetMsdsAuxiliaryClasses((string)reqEnt.Properties["ldapdisplayname"].Value, adamModel);
                modelMsdsAuxClass.Sort();

                //Checking condition.
                bool isR324Satisfied = true;
                if (serverMsdsAuxClass.Count == modelMsdsAuxClass.Count)
                {
                    foreach (string element in serverMsdsAuxClass)
                    {
                        if (!modelMsdsAuxClass.Contains(element))
                        {
                            isR324Satisfied = false;
                            break;
                        }
                    }
                }
                else
                {
                    isR324Satisfied = false;
                }

                //MS-AD_Schema_R324.
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    isR324Satisfied,
                    324,
                    @"The value of TO!msDS-Auxiliary-Classes, 
                    where TO be the object from which the msDS-Auxiliary-Classes attribute is being read, is 
                    the set of lDAPDisplayNames from each Object O such that (O is in TO!objectClass) and 
                    (O is not in SUPCLASSES(Most Specific class of TO)).");
            }

            #endregion

            #region primaryGroupToken

            int serverPrimariyGroupToken = 0;
            int modelPrimaryGroupToken = 0;
            sampleClasses = new List<string>();
            reqEnt = null;

            //Add sample classes here.
            sampleClasses.Add("CN=Users,CN=Roles,CN=Configuration," + adAdapter.LDSRootObjectName);
            sampleClasses.Add("CN=Roles,CN=Configuration," + adAdapter.LDSRootObjectName);

            foreach (string sampleClass in sampleClasses)
            {
                serverPrimariyGroupToken = 0;
                modelPrimaryGroupToken = 0;

                //Getting msDS-Auxiliary-Classes of this sample class from server.
                if (!adAdapter.GetLdsObjectByDN(sampleClass, out reqEnt))
                {
                    DataSchemaSite.Assume.IsTrue(false, sampleClass + " Object is not found in server");
                }

                PropertyValueCollection objectClasValues = reqEnt.Properties["objectClass"];

                //If it is group object,
                if (objectClasValues.Contains("group"))
                {
                    //Get the Primary group token.
                    reqEnt.RefreshCache(new string[] { "primarygrouptoken", "objectsid" });
                    serverPrimariyGroupToken = (int)reqEnt.Properties["primarygrouptoken"].Value;

                    //Construct the Primary group token.
                    byte[] byteArray = (byte[])reqEnt.Properties["objectSid"].Value;
                    System.Security.Principal.SecurityIdentifier identifier = new System.Security.Principal.SecurityIdentifier(byteArray, 0);
                    string primaryGroupTokenString = identifier.Value.ToString();
                    primaryGroupTokenString = primaryGroupTokenString.Substring(primaryGroupTokenString.LastIndexOf('-') + 1);

                    modelPrimaryGroupToken = int.Parse(primaryGroupTokenString);

                    //MS-AD_Schema_R301.
                    DataSchemaSite.CaptureRequirementIfAreEqual<int>(
                        serverPrimariyGroupToken,
                        modelPrimaryGroupToken,
                        301,
                        @"The value of TO!primaryGroupToken, where TO be the object 
                        from which the primaryGroupToken attribute is being read, is the RID from 
                        TO!objectSid when there exists C in TO!objectClass such that C is the group class.");
                }
                //If it is not group object,
                else
                {
                    reqEnt.RefreshCache(new string[] { "primarygrouptoken", "objectsid" });
                    //Get the Primary group token.
                    if (reqEnt.Properties["primarygrouptoken"].Value != null)
                    {
                        serverPrimariyGroupToken = (int)reqEnt.Properties["primarygrouptoken"].Value;
                    }
                    else
                    {
                        serverPrimariyGroupToken = -1;
                    }
                    //Construct the Primary group token.
                    if (reqEnt.Properties["objectSid"].Value != null)
                    {
                        byte[] byteArray = (byte[])reqEnt.Properties["objectSid"].Value;
                        System.Security.Principal.SecurityIdentifier identifier = new System.Security.Principal.SecurityIdentifier(byteArray, 0);
                        string primaryGroupTokenString = identifier.Value.ToString();
                        primaryGroupTokenString = primaryGroupTokenString.Substring(primaryGroupTokenString.LastIndexOf('-') + 1);

                        modelPrimaryGroupToken = int.Parse(primaryGroupTokenString);
                    }
                    else
                    {
                        modelPrimaryGroupToken = -1;
                    }
                    //MS-AD_Schema_R302.
                    DataSchemaSite.CaptureRequirementIfAreEqual<int>(
                        serverPrimariyGroupToken,
                        modelPrimaryGroupToken,
                        302,
                        @"If TO is not a group, no value is returned for 
                        TO!primaryGroupToken, where TO be the object from which the primaryGroupToken attribute
                        is being read, when this attribute is read from TO.");
                }
            }

            #endregion

            #region entryTTL

            //Create dynamic object on server.
            distinguishedName = "CN=TestDynamicContainer,CN=Roles," +
                adAdapter.LDSApplicationNC;
            DateTime now = DateTime.UtcNow;
            string timeFormat = now.Year.ToString();
            if (now.Month.ToString().Length < 2)
            {
                timeFormat += "0" + now.Month.ToString();
            }
            else
            {
                timeFormat += now.Month.ToString();
            }
            if (now.Day.ToString().Length < 2)
            {
                timeFormat += "0" + now.Day.ToString();
            }
            else
            {
                timeFormat += now.Day.ToString();
            }
            if (now.Hour.ToString().Length < 2)
            {
                timeFormat += "0" + now.Hour.ToString();
            }
            else
            {
                timeFormat += now.Hour.ToString();
            }
            //minimum entryTTL is 900 seconds by default
            now = now.AddMinutes(16);
            if (now.Minute.ToString().Length < 2)
            {
                timeFormat += "0" + now.Minute.ToString();
            }
            else
            {
                timeFormat += now.Minute.ToString();
            }
            if (now.Second.ToString().Length < 2)
            {
                timeFormat += "0" + now.Second.ToString();
            }
            else
            {
                timeFormat += now.Second.ToString();
            }
            List<DirectoryAttribute> atts = new List<DirectoryAttribute>();
            atts.Add(new DirectoryAttribute("objectClass", new String[] { "dynamicobject", "Container" }));
            atts.Add(new DirectoryAttribute("msDS-Entry-Time-To-Die", new String[] { timeFormat + ".0Z" }));
            string ret = AdLdapClient.Instance().ConnectAndBind(
                adAdapter.PDCNetbiosName,
                adAdapter.PDCIPAddr,
                Convert.ToInt32(adAdapter.ADLDSPortNum),
                adAdapter.ClientUserName,
                adAdapter.ClientUserPassword,
                adAdapter.PrimaryDomainNetBiosName,
                AuthType.Basic | AuthType.Ntlm);
            if (ret.Equals("Success_STATUS_SUCCESS"))
            {
                DirectoryEntry dynamicObject = null;
                if (adAdapter.GetLdsObjectByDN(distinguishedName, out dynamicObject))
                {
                    AdLdapClient.Instance().DeleteObject(distinguishedName,null);
                }
                Thread.Sleep(1000);

                ret = AdLdapClient.Instance().AddObject(distinguishedName, atts, null);
                if (ret.Equals("Success_STATUS_SUCCESS"))
                {
                    if (!adAdapter.GetLdsObjectByDN(distinguishedName, out dynamicObject))
                    {
                        DataSchemaSite.Assume.IsTrue(false, distinguishedName + " Object is not found in server");
                    }
                    dynamicObject.RefreshCache(new string[] { "entryttl" });
                    dynamicObject.RefreshCache(new string[] { "msDS-Entry-Time-To-Die" });

                    DateTime currentTime = DateTime.UtcNow;
                    DateTime objectTime = (DateTime)dynamicObject.Properties["msDS-Entry-Time-To-Die"].Value;

                    //Get the values.
                    uint actualTimeRemaining;
                    if (DateTime.Compare(currentTime, objectTime) > 0)
                    {
                        actualTimeRemaining = 0;
                    }
                    else
                    {
                        actualTimeRemaining = (uint)objectTime.Subtract(currentTime).TotalSeconds;
                        if (actualTimeRemaining > 0xFFFFFFFF)
                            actualTimeRemaining = 0xFFFFFFFF;
                    }
                    int expectedTimeRemaining = int.Parse(dynamicObject.Properties["entryttl"].Value.ToString());

                    //Checking condition.
                    bool isR304AndR305Satisfied = false;
                    if (actualTimeRemaining == expectedTimeRemaining
                        || actualTimeRemaining >= expectedTimeRemaining - 10)
                    {
                        isR304AndR305Satisfied = true;
                    }
                    else
                    {
                        isR304AndR305Satisfied = false;
                    }

                    //MS-AD_Schema_R304.
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        isR304AndR305Satisfied,
                        304,
                        @"The value of TO!entryTTL, where TO be the 
                    object from which the entryTTL attribute is being read, is the number of seconds in 
                    TO!msDS-Entry-Time-To-Die minus the current system time, and is constrained to the range 
                    0..0xFFFFFFFF by returning 0 if the difference is less than 0.");

                    //MS-AD_Schema_R305.
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        isR304AndR305Satisfied,
                        305,
                        @"The value of TO!entryTTL, where TO be the 
                    object from which the entryTTL attribute is being read, is the number of seconds in 
                    TO!msDS-Entry-Time-To-Die minus the current system time, and is constrained to the range 
                    0..0xFFFFFFFF by returning 0xFFFFFFFF if the difference is greater than 0xFFFFFFFF.");
                    AdLdapClient.Instance().DeleteObject(distinguishedName, null);
                }
                else
                {
                    DataSchemaSite.Assume.Fail("The specified dynamic object may already exist.");
                }
            }

            #endregion

            #region parentGUID For AD/LDS

            //Implementation should be presented in LDS constructed attributes.
            DirectoryEntry rootEntry = null;
            bool isParentGUIDEqualToObjectGUID = true;
            if (!adAdapter.GetLdsObjectByDN("CN=Configuration," + adAdapter.LDSRootObjectName, out rootEntry))
            {
                DataSchemaSite.Assert.IsTrue(false, adAdapter.rootDomainDN + "Object is not found in server");
            }
            string hostOrDomainName = adAdapter.adamServerPort;
            string targetOu = "CN=Configuration," + adAdapter.LDSRootObjectName;
            string ldapSearchFilter = "((objectClass=container))";
            string attributeToReturn = "parentguid";
            LdapConnection connection = new LdapConnection(hostOrDomainName);
            SearchRequest searchRequest = new SearchRequest(
                targetOu,
                ldapSearchFilter,
                System.DirectoryServices.Protocols.SearchScope.OneLevel,
                attributeToReturn);
            SearchResponse searchResponse = null;
            try
            {
                searchResponse = (SearchResponse)connection.SendRequest(searchRequest);
            }
            catch (Exception ex)
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, ex.Message);
            }
            foreach (SearchResultEntry entry in searchResponse.Entries)
            {
                if (entry.DistinguishedName == "CN=DirectoryUpdates," + targetOu)
                {
                    SearchResultAttributeCollection attributes = entry.Attributes;
                    foreach (DirectoryAttribute attribute in attributes.Values)
                    {
                        // Count the number of values associated with this attribute
                        for (int i = 0; i < attribute.Count; i++)
                        {
                            byte[] x = attribute[i] as byte[];
                            Guid guid = new Guid(x);
                            string parentGUID = guid.ToString();
                            byte[] objectGUID = (byte[])rootEntry.Properties["objectGUID"].Value;
                            Guid id = new Guid(objectGUID);
                            string objectGUIDValue = id.ToString();

                            if (parentGUID != objectGUIDValue)
                            {
                                isParentGUIDEqualToObjectGUID = !isParentGUIDEqualToObjectGUID;
                            }
                        }
                    }
                }
            }

            #endregion

            #region msDS-UserPasswordExpired

            List<string> listOfObjects = new List<string>();
            listOfObjects.Add(
                "CN=UserObject1,CN=Query-Policies,CN=Directory Service,CN=Windows NT,CN=Services,CN=Configuration,"
                + adAdapter.LDSRootObjectName);

            foreach (string entryDN in listOfObjects)
            {
                bool expected = false;
                bool actual = false;

                DirectoryEntry dsEntry = null;
                DirectoryEntry userEntry = null;
                if (!adAdapter.GetLdsObjectByDN(
                    "CN=Directory Service,CN=Windows NT,CN=Services,CN=Configuration,"
                    + adAdapter.LDSRootObjectName,
                    out dsEntry))
                {
                    AdLdapClient.Instance().AddObject(entryDN, atts, null);
                }

                PropertyValueCollection value = dsEntry.Properties["msDS-Other-Settings"];
                object[] valueArray = (object[])value.Value;
                string element = String.Empty;
                foreach (string tempElement in valueArray)
                {
                    if (tempElement.Contains("ADAMDisablePasswordPolicies"))
                    {
                        element = tempElement;
                        break;
                    }
                }
                if (!element.Equals("ADAMDisablePasswordPolicies=1"))
                {
                    expected = true;
                }
                else
                {
                    expected = false;
                }

                if (!adAdapter.GetLdsObjectByDN(entryDN, out userEntry))
                {
                    DirectoryEntry parentEntry = null;
                    string tt = entryDN.Substring(entryDN.IndexOf(',') + 1);
                    adAdapter.GetLdsObjectByDN(entryDN.Substring(entryDN.IndexOf(',') + 1), out parentEntry);
                    try
                    {
                        string tempUserName = entryDN.Substring(entryDN.IndexOf('=') + 1);
                        tempUserName = tempUserName.Substring(0, tempUserName.IndexOf(','));


                        DirectoryEntry newUser = parentEntry.Children.Add("CN=" + tempUserName, "user");

                        // Checking whether the temporary User (from config file) is already exists or not. 
                        if (!adAdapter.GetLdsObjectByDN(entryDN, out userEntry))
                        {

                            newUser.CommitChanges();
                            newUser.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        DataSchemaSite.Log.Add(LogEntryKind.Warning, ex.Message);
                    }
                    adAdapter.GetLdsObjectByDN(entryDN, out userEntry);
                }

                if (expected)
                {
                    DirectoryEntry userAccEntry = null;
                    adAdapter.GetLdsObjectByDN(entryDN, out userAccEntry);
                    userEntry.RefreshCache(new string[] { "msDS-User-Account-Control-Computed" });
                    int uacComputed = int.Parse(userEntry.Properties["msDS-User-Account-Control-Computed"].Value.ToString());
                    int uacFlag = ParseUserAccountControlValue(
                        "ADS_UF_DONT_EXPIRE_PASSWD|ADS_UF_SMARTCARD_REQUIRED|"
                        + "ADS_UF_WORKSTATION_TRUST_ACCOUNT|ADS_UF_SERVER_TRUST_ACCOUNT|ADS_UF_INTERDOMAIN_TRUST_ACCOUNT");
                    expected = ((uacComputed & uacFlag) == 0);
                }
                if (expected)
                {
                    DirectorySearcher ds = new DirectorySearcher(userEntry, "(objectClass=*)",

                            new string[] { "pwdLastSet" },

                            System.DirectoryServices.SearchScope.Base);

                    System.DirectoryServices.SearchResult result = ds.FindOne();
                    string pwdLastSet = result.Properties["pwdLastSet"][0].ToString();

                    DirectoryEntry rootDomainEntry = null;
                    adAdapter.GetObjectByDN(adAdapter.rootDomainDN, out rootDomainEntry);

                    ds = new DirectorySearcher(rootDomainEntry, "(objectClass=*)",

                        new string[] { "maxPwdAge" },

                        System.DirectoryServices.SearchScope.Base);

                    result = ds.FindOne();

                    long maxPwdAge = (long)result.Properties["maxPwdAge"][0];
                    //9223372036854775808

                    DateTime st = DateTime.Now;
                    DateTime pLastSet = new DateTime(long.Parse(pwdLastSet));

                    expected = (
                        (pwdLastSet.Equals(null))
                        || (pwdLastSet.Equals("0"))
                        || ((maxPwdAge != -9223372036854775808)
                        && -(st.Ticks - long.Parse(pwdLastSet)) > maxPwdAge));

                }
                userEntry.RefreshCache(new string[] { "msDS-UserPasswordExpired" });
                actual = bool.Parse(userEntry.Properties["msDS-UserPasswordExpired"].Value.ToString());

                DataSchemaSite.CaptureRequirementIfAreEqual<bool>(
                    expected,
                    actual,
                    346,
                    @"In construction of msDS-UserPasswordExpired attribute, TO be the object from which the 
                    msDS-UserPasswordExpired attribute is being read and ST be the current time, read from the system 
                    clock. If the machine running AD/LDS is joined to a domain, let D be the root of the domain NC of 
                    the joined domain. Then TO!msDS-UserPasswordExpired is true if three of the following are true:
                    1. The LDAP configurable setting ADAMDisablePasswordPolicies ≠ 1. 
                    2. None of bits ADS_UF_SMARTCARD_REQUIRED, ADS_UF_DONT_EXPIRE_PASSWD, 
                    ADS_UF_WORKSTATION_TRUST_ACCOUNT, ADS_UF_SERVER_TRUST_ACCOUNT, ADS_UF_INTERDOMAIN_TRUST_ACCOUNT is 
                    set in TO!userAccountControl. 3. TO!pwdLastSet = null, or TO!pwdLastSet = 0, 
                    or (D!maxPwdAge ≠ 0x8000000000000000 and (ST - TO!pwdLastSet) > D!maxPwdAge)).");
            }
            AdLdapClient.Instance().Unbind();
            #endregion

            #region ms-DS-UserAccountAutoLocked

            foreach (string entryDN in listOfObjects)
            {
                bool expected = false;
                bool actual = false;

                DirectoryEntry dsEntry = null;
                DirectoryEntry userEntry = null;
                adAdapter.GetLdsObjectByDN(
                    "CN=Directory Service,CN=Windows NT,CN=Services,CN=Configuration,"
                    + adAdapter.LDSRootObjectName,
                    out dsEntry);

                PropertyValueCollection value = dsEntry.Properties["msDS-Other-Settings"];
                object[] valueArray = (object[])value.Value;
                string element = String.Empty;
                foreach (string tempElement in valueArray)
                {
                    if (tempElement.Contains("ADAMDisablePasswordPolicies"))
                    {
                        element = tempElement;
                        break;
                    }
                }
                if (!element.Equals("ADAMDisablePasswordPolicies=1"))
                {
                    expected = true;
                }
                else
                {
                    expected = false;
                }

                if (!adAdapter.GetLdsObjectByDN(entryDN, out userEntry))
                {
                    DirectoryEntry parentEntry = null;
                    string tt = entryDN.Substring(entryDN.IndexOf(',') + 1);
                    adAdapter.GetLdsObjectByDN(entryDN.Substring(entryDN.IndexOf(',') + 1), out parentEntry);
                    try
                    {
                        string tempUserName = entryDN.Substring(entryDN.IndexOf('=') + 1);
                        tempUserName = tempUserName.Substring(0, tempUserName.IndexOf(','));

                        DirectoryEntry newUser = parentEntry.Children.Add("CN=" + tempUserName, "user");

                        // Checking whether the temporary User (from config file) is already exists or not.
                        if (!adAdapter.GetLdsObjectByDN(entryDN, out userEntry))
                        {
                            newUser.CommitChanges();
                            newUser.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        DataSchemaSite.Log.Add(LogEntryKind.Warning, ex.Message);
                    }
                    adAdapter.GetLdsObjectByDN(entryDN, out userEntry);
                }

                if (expected)
                {
                    DirectoryEntry rootDomainEntry = null;
                    adAdapter.GetObjectByDN(adAdapter.rootDomainDN, out rootDomainEntry);

                    DirectorySearcher ds = new DirectorySearcher(rootDomainEntry, "(objectClass=*)",

                        new string[] { "lockoutDuration" },

                        System.DirectoryServices.SearchScope.Base);

                    System.DirectoryServices.SearchResult result = ds.FindOne();

                    long lockOutDuration = (long)result.Properties["lockoutDuration"][0];

                    ds = new DirectorySearcher(userEntry, "(objectClass=*)",

                        new string[] { "lockoutTime" },

                        System.DirectoryServices.SearchScope.Base);

                    result = ds.FindOne();

                    long lockOutTime = 0;
                    if (result.Properties.Contains("lockoutTime"))
                    {
                        lockOutTime = (long)result.Properties["lockoutTime"][0];
                    }

                    DateTime st = DateTime.Now;

                    expected = (lockOutTime != 0)
                        && ((lockOutDuration < -9223372036854775808)
                        || ((lockOutDuration + st.Ticks) <= lockOutTime));
                }
                userEntry.RefreshCache(new string[] { "ms-DS-UserAccountAutoLocked" });
                actual = bool.Parse(userEntry.Properties["ms-DS-UserAccountAutoLocked"].Value.ToString());

                DataSchemaSite.CaptureRequirementIfAreEqual<bool>(
                    expected,
                    actual,
                    343,
                    @"In construction of ms-DS-UserAccountAutoLocked attribute, TO be the object from which the 
                    ms-DS-UserAccountAutoLocked attribute is being read, and ST be the current time, read from the 
                    system clock. If the machine running AD/LDS is joined to a domain D, TO!ms-DS-UserAccountAutoLocked 
                    is true if the following are true: 1. The LDAP configurable setting ADAMDisablePasswordPolicies ≠ 1.
                    2. TO!lockoutTime ≠ 0 and either (1) D!lockoutDuration (regarded as an unsigned quantity) < 
                    0x8000000000000000, or (2) ST + D!lockoutDuration (regarded as a signed quantity) ≤ TO!lockoutTime.");
            }

            #endregion

            #region msDS-User-Account-Control-Computed

            bool isLDS = adAdapter.RunLDSTestCases;
            if (isLDS)
            {
                listOfObjects = new List<string>();
                listOfObjects.Add(
                    "CN=UserObject1,CN=Query-Policies,CN=Directory Service,CN=Windows NT,CN=Services,CN=Configuration,"
                    + adAdapter.LDSRootObjectName);
                bool isR317Satisfied = false, isR318De317Satisfied = false, isR319De317Satisfied = false,
                     isR320De317Satisfied = false, isR321De317Satisfied = false, isR322De317Satisfied = false;
                foreach (string entryDN in listOfObjects)
                {
                    DirectoryEntry userEntry = null;
                    adAdapter.GetLdsObjectByDN(entryDN, out userEntry);
                    userEntry.RefreshCache(new string[] { "msDS-User-Account-Control-Computed" });
                    PropertyValueCollection userAccountControl = userEntry.Properties["msDS-User-Account-Control-Computed"];
                    int uacComputed = int.Parse(userAccountControl.Value.ToString());

                    DirectorySearcher ds = new DirectorySearcher(userEntry, "(objectClass=*)",

                            new string[] { "msDS-UserAccountDisabled" ,"ms-DS-UserAccountAutoLocked" ,
                            "sd syncms-DS-UserPasswordNotRequired","msDS-UserDontExpirePassword","msDS-UserPasswordExpired"},
                            System.DirectoryServices.SearchScope.Base);

                    System.DirectoryServices.SearchResult result = ds.FindOne();
                    if (result.Properties.Contains("msDS-UserAccountDisabled"))
                    {
                        bool tempValue = false;
                        try
                        {
                            tempValue = bool.Parse(result.Properties["msDS-UserAccountDisabled"][0].ToString());
                        }
                        catch (Exception ex)
                        {
                            DataSchemaSite.Log.Add(LogEntryKind.Warning, ex.Message);
                        }
                        if (tempValue)
                        {
                            isR318De317Satisfied = ((uacComputed & 2) == 2);
                        }
                        else
                        {
                            isR318De317Satisfied = ((uacComputed & 2) == 0);
                        }
                    }
                    else
                    {
                        isR318De317Satisfied = ((uacComputed & 2) == 0);
                    }

                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        isR318De317Satisfied,
                        318,
                        @"In AD/LDS, The value of 
                        TO!msDS-User-Account-Control-Computed attribute is the bit pattern where (AD is 
                        Account Disable) AD (ADS_UF_ACCOUNT_DISABLE) is set if TO!msDS-UserAccountDisabled is 
                        true[AD is Account Disable].");

                    if (result.Properties.Contains("ms-DS-UserAccountAutoLocked"))
                    {
                        bool tempValue = bool.Parse(result.Properties["ms-DS-UserAccountAutoLocked"][0].ToString());
                        if (tempValue)
                        {
                            isR319De317Satisfied = ((uacComputed & 16) == 16);
                        }
                        else
                        {
                            isR319De317Satisfied = ((uacComputed & 16) == 0);
                        }
                    }
                    else
                    {
                        isR319De317Satisfied = ((uacComputed & 16) == 0);
                    }
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        isR319De317Satisfied,
                        319,
                        @"In AD/LDS, The value of 
                        TO!msDS-User-Account-Control-Computed attribute is the bit pattern where LO 
                        (ADS_UF_LOCKOUT) is set if TO!ms-DS-UserAccountAutoLocked is true[LO is Lockout].");

                    if (result.Properties.Contains("ms-DS-UserPasswordNotRequired"))
                    {
                        bool tempValue = bool.Parse(result.Properties["ms-DS-UserPasswordNotRequired"][0].ToString());
                        if (tempValue)
                        {
                            isR320De317Satisfied = ((uacComputed & 32) == 32);
                        }
                        else
                        {
                            isR320De317Satisfied = ((uacComputed & 32) == 0);
                        }
                    }
                    else
                    {
                        isR320De317Satisfied = ((uacComputed & 32) == 0);
                    }
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        isR320De317Satisfied,
                        320,
                        @"In AD/LDS, The value of 
                        TO!msDS-User-Account-Control-Computed attribute is the bit pattern where PNR 
                        (ADS_UF_PASSWD_NOTREQD) is set if TO!ms-DS-UserPasswordNotRequired is true[PNR is Password 
                        not required].");

                    if (result.Properties.Contains("msDS-UserDontExpirePassword"))
                    {
                        bool tempValue = bool.Parse(result.Properties["msDS-UserDontExpirePassword"][0].ToString());
                        if (tempValue)
                        {
                            isR321De317Satisfied = ((uacComputed & 65536) == 65536);
                        }
                        else
                        {
                            isR321De317Satisfied = ((uacComputed & 65536) == 0);
                        }
                    }
                    else
                    {
                        isR321De317Satisfied = ((uacComputed & 65536) == 0);
                    }
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        isR321De317Satisfied,
                        321,
                        @"In AD/LDS, The value of TO!msDS-User-Account-Control-Computed attribute is the bit pattern 
                        where DEP (ADS_UF_DONT_EXPIRE_PASSWD) is set if TO!msDS-UserDontExpirePassword is true[DEP is 
                        Don’t expire password].");

                    if (result.Properties.Contains("msDS-UserPasswordExpired"))
                    {
                        bool tempValue = bool.Parse(result.Properties["msDS-UserPasswordExpired"][0].ToString());
                        if (tempValue)
                        {
                            isR322De317Satisfied = ((uacComputed & 8388608) == 8388608);
                        }
                        else
                        {
                            isR322De317Satisfied = ((uacComputed & 8388608) == 0);
                        }
                    }
                    else
                    {
                        isR322De317Satisfied = ((uacComputed & 8388608) == 0);
                    }
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        isR322De317Satisfied,
                        322,
                        @"In AD/LDS, The value of 
                        TO!msDS-User-Account-Control-Computed attribute is the bit pattern where PE 
                        (ADS_UF_PASSWORD_EXPIRED) is set if TO!msDS-UserPasswordExpired is true[PE is Password 
                        Expired].");
                }

                isR317Satisfied = isR318De317Satisfied && isR319De317Satisfied && isR320De317Satisfied &&
                    isR321De317Satisfied && isR322De317Satisfied;

                DataSchemaSite.CaptureRequirementIfIsTrue(
                       isR317Satisfied,
                       317,
                       @"In AD/LDS, The value of TO!msDS-User-Account-Control-Computed attribute is the bit pattern where
                       AD (ADS_UF_ACCOUNT_DISABLE) is set if TO!msDS-UserAccountDisabled is true,
                       O (ADS_UF_LOCKOUT) is set if TO!ms-DS-UserAccountAutoLocked is true,
                       PNR (ADS_UF_PASSWD_NOTREQD) is set if TO!ms-DS-UserPasswordNotRequired is true,
                       DEP (ADS_UF_DONT_EXPIRE_PASSWD) is set if TO!msDS-UserDontExpirePassword is true,
                       PE (ADS_UF_PASSWORD_EXPIRED) is set if TO!msDS-UserPasswordExpired is true.");
            }

            #endregion

            #region allowedChildClassesEffective

            if (isLDS)
            {
                listOfObjects = new List<string>();
                listOfObjects.Add(
                    "CN=Query-Policies,CN=Directory Service,CN=Windows NT,CN=Services,CN=Configuration,"
                    + adAdapter.LDSRootObjectName);
                IADsSecurityDescriptor sd = null;
                IADsAccessControlList dacl = null;
                string ownerSid = String.Empty;
                bool isSuccess = false;
                foreach (string entryDN in listOfObjects)
                {
                    DirectoryEntry userEntry = null;
                    adAdapter.GetLdsObjectByDN(entryDN, out userEntry);

                    userEntry.RefreshCache(new string[] { "allowedChildClasses", "allowedChildClassesEffective" });
                    object[] childClasses = null;
                    object[] actualChildClassesEffective = null;
                    object[] expectedChildClassesEffective = null;

                    childClasses = (object[])userEntry.Properties["allowedChildClasses"].Value;
                    actualChildClassesEffective = (object[])userEntry.Properties["allowedChildClassesEffective"].Value;
                    expectedChildClassesEffective = new object[actualChildClassesEffective.Length];

                    sd = (IADsSecurityDescriptor)userEntry.Properties["ntsecuritydescriptor"].Value;
                    dacl = (IADsAccessControlList)sd.DiscretionaryAcl;
                    bool isCreateChild = false;
                    bool fAllowPrinicipals = true;
                    bool spcOfO = false;
                    bool isConfig = false;
                    bool isSchema = false;
                    ownerSid = "";
                    DirectoryEntry ownerEntry = null;
                    adAdapter.GetObjectByDN("CN=Enterprise Admins,CN=Users," + adAdapter.rootDomainDN, out ownerEntry);
                    byte[] sidArray = (byte[])ownerEntry.Properties["objectsid"].Value;
                    ownerSid = new SecurityIdentifier(sidArray, 0).ToString();
                    ownerSid = ownerSid.Substring(ownerSid.LastIndexOf('-') + 1);
                    foreach (IADsAccessControlEntry ace in dacl)
                    {
                        if (ace.Trustee.Contains(ownerSid))
                        {
                            if ((ace.AccessMask & 1) == 1)
                            {
                                isCreateChild = true;
                                break;
                            }
                        }
                    }

                    int loopVar = 0;
                    foreach (string childClass in childClasses)
                    {

                        spcOfO = SPCForLDS(childClass);


                        isConfig = entryDN.ToLower().Contains("schema");
                        isSchema = entryDN.ToLower().Contains("configuration");

                        if (isCreateChild
                            && (fAllowPrinicipals || !isConfig || !spcOfO)
                            && (fAllowPrinicipals || !isSchema || !spcOfO))
                        {
                            expectedChildClassesEffective[loopVar++] = childClass;
                        }
                    }
                    isSuccess = false;
                    for (loopVar = 0; loopVar < expectedChildClassesEffective.Length; loopVar++)
                    {
                        if (((string)actualChildClassesEffective[loopVar]).Equals(((string)expectedChildClassesEffective[loopVar])))
                        {
                            isSuccess = true;
                        }
                        else
                        {
                            isSuccess = false;
                            break;
                        }
                    }

                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        isSuccess,
                        287,
                        @"If the DC is running as AD LDS, then let fAllowPrincipals equal false if the value of the ADAMAllowADAMSecurityPrincipalsInConfigPartition
                        configuration setting (section 3.1.1.3.4.7) is other than 1.
                        Let TO be the object from which the allowedChildClassesEffective attribute is being read, contains each object class O in TO!allowedChildClasses such that:
                        -> (
                        (TO!nTSecurityDescriptor grants RIGHT_DS_CREATE_CHILD via a simple access control entry (ACE) to the client for instantiating an object beneath TO) or 
                        (TO.nTSecurityDescriptor grants RIGHT_DS_CREATE_CHILD via an object-specific ACE to the client for instantiating an object of class O beneath TO)
                        )
                        -> and (fAllowPrincipals or (not TO!distinguishedName in config NC) or (not SPC(O)))
                        -> and (fAllowPrincipals or (not TO!distinguishedName in schema NC) or (not SPC(O))).");
                }
            }

            #endregion

            #region msDS-LocalEffectiveRecycleTime

            //Verify MS-AD_Schema requirement:MS-AD_Schema_R4282
            //The msDS-LocalEffectiveDeletionTime attribute itsself is a new property for the Windows Server 2008R2 DS and LDS
            //And before calling the method,it has been determined the platform for AD DS and AD LDS.
            DataSchemaSite.CaptureRequirement(
                "MS-ADTS-Schema",
                4282,
                @"The msDS-LocalEffectiveDeletionTime attribute exists on AD DS and AD LDS, 
                beginning with Windows Server� 2008 R2 operating system.");

            #endregion

            #region msDS-LocalEffectiveRecycleTime

            //Verify MS-AD_Schema requirement:MS-AD_Schema_R4292
            //The msDS-LocalEffectiveDeletionTime attribute itsself is a new property for the Windows Server 2008R2 DS and LDS
            //And before calling the method,it has been determined the platform for AD DS and AD LDS.
            DataSchemaSite.CaptureRequirement(
                "MS-ADTS-Schema",
                4292,
                @"The msDS-LocalEffectiveRecycleTime attribute exists on AD DS and AD LDS, 
                    beginning with Windows Server� 2008 R2 operating system.");

            #endregion

        }

        #endregion

        #region CommonAttributesValidation

        /// <summary>
        /// Common Attributes Validation
        /// </summary>
        /// <param name="dirEntry">DirEntry</param>
        /// <param name="isDS">DS or LDS. TRUE - DS; FALSE - LDS</param>
        private static void CommonAttributesValidation(DirectoryEntry dirEntry, bool isDS)
        {
            #region subSchemaSubEntry
            dirEntry.RefreshCache(new string[] { "subSchemaSubEntry" });
            String actualSubSchemaSubEntry = dirEntry.Properties["subSchemaSubEntry"].Value.ToString().ToLower();
            String expectedSubSchemaSubEntry = null;
            string schemaDN = "CN=Schema,CN=Configuration,";
            string domainNC = adAdapter.PrimaryDomainDnsName;

            if (isDS)
            {
                foreach (string str in domainNC.Split('.'))
                {
                    schemaDN = schemaDN + "DC=" + str + ",";
                }
                schemaDN = schemaDN.Substring(0, schemaDN.Length - 1);
            }
            else
            {
                schemaDN = schemaDN + dirEntry.Properties["distinguishedname"].Value.ToString().Substring(
                    dirEntry.Properties["distinguishedname"].Value.ToString().LastIndexOf(',') + 1);
            }
            expectedSubSchemaSubEntry = "cn=aggregate," + schemaDN.ToLower();

            //MS-AD_Schema_R275
            DataSchemaSite.CaptureRequirementIfAreEqual<String>(
                expectedSubSchemaSubEntry,
                actualSubSchemaSubEntry,
                275,
                "The value of subSchemaSubEntry constructed attribute is the DN equal to the schema NC's DN appended "
            + "to \"CN=Aggregate,\".");

            #endregion

            #region CanonicalName

            string distinguishedName = String.Empty;
            string expectedCN = String.Empty;
            string actualCN = String.Empty;
            string[] allDCs = new string[10];
            int eachDC = 0;

            dirEntry.RefreshCache(new string[] { "canonicalName" });
            if (dirEntry.Properties["canonicalname"].Value != null)
            {
                expectedCN = (string)dirEntry.Properties["canonicalName"].Value;

                //Getting distinguishedName for the same object.
                distinguishedName = (string)dirEntry.Properties["distinguishedName"].Value;
                string[] splitedDN = distinguishedName.Split(',');
                foreach (string eachSplit in splitedDN)
                {
                    if (eachSplit.StartsWith("DC="))
                    {
                        string removeDC;
                        removeDC = eachSplit.Replace("DC=", "");
                        if (actualCN == "")
                            actualCN = actualCN.Insert(0, removeDC);
                        else
                        {
                            actualCN = actualCN.Insert(actualCN.Length, "." + removeDC);
                        }
                    }
                }
                foreach (string eachSplit in splitedDN)
                {
                    if (eachSplit.StartsWith("CN="))
                    {
                        string removeCN;
                        removeCN = eachSplit.Replace("CN=", "");
                        allDCs[eachDC] = removeCN;
                        eachDC++;
                    }
                }
                for (; eachDC > 0; --eachDC)
                {
                    actualCN = actualCN.Insert(actualCN.Length, "/" + allDCs[eachDC - 1]);
                }
            }
            //MS-AD_Schema_R277
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                expectedCN,
                actualCN,
                277,
                "The value of canonicalName constructed attribute is the canonical name of the object.");

            #endregion

            #region fromEntry

            dirEntry.RefreshCache(new string[] { "fromEntry" });
            String fromEntryValue = dirEntry.Properties["fromEntry"].Value.ToString().ToLower();
            if (dirEntry.Properties["instanceType"].Value.ToString().Contains("4"))
            {
                //MS-AD_Schema_R294
                DataSchemaSite.Log.Add(LogEntryKind.Comment, "fromEntry TDI is resolved");
                DataSchemaSite.CaptureRequirementIfAreEqual<String>(
                    fromEntryValue,
                    "true",
                    294,
                    @"The value of TO!fromEntry,where TO be the object from which the fromEntry attribute is being 
                        read, is true if TO!instanceType has bit 0x4 set.");

            }
            else
            {
                //MS-AD_Schema_R295
                DataSchemaSite.CaptureRequirementIfAreEqual<String>(
                    fromEntryValue,
                    "false",
                    295,
                    @"The value of TO!fromEntry, where TO be the object from which the fromEntry attribute is being 
                    read, is false if TO!instanceType has bit 0x4 is not set.");
            }

            #endregion

            #region createTimeStamp

            dirEntry.RefreshCache(new string[] { "createTimeStamp" });
            String actualcreateTimeStamp = dirEntry.Properties["createTimeStamp"].Value.ToString().ToLower();
            String expectedcreateTimeStamp = dirEntry.Properties["whenCreated"].Value.ToString().ToLower();

            //MS-AD_Schema_R297
            DataSchemaSite.CaptureRequirementIfAreEqual<String>(
                expectedcreateTimeStamp,
                actualcreateTimeStamp,
                297,
                @"The value of TO!createTimeStamp, where TO be the object from which the createTimeStamp attribute is 
                being read, is TO!whenCreated.");

            #endregion

            #region modifyTimeStamp

            dirEntry.RefreshCache(new string[] { "modifyTimeStamp", "whenChanged" });
            String actualmodifyTimeStamp = dirEntry.Properties["modifyTimeStamp"].Value.ToString().ToLower();
            String expectedmodifyTimeStamp = dirEntry.Properties["whenChanged"].Value.ToString().ToLower();

            //MS-AD_Schema_R299 Requirement.
            DataSchemaSite.CaptureRequirementIfAreEqual<String>(
                expectedmodifyTimeStamp,
                actualmodifyTimeStamp,
                299,
                @"The value of TO!modifyTimeStamp, where TO be the object from which the modifyTimeStamp attribute is 
                being read, is TO!whenChanged.");

            #endregion
        }

        private bool SPCForDS(string o)
        {
            string[] setOfClasses = new string[50];
            string serverName = adAdapter.PDCNetbiosName;
            if (serverOS >= OSVersion.WinSvr2008R2)
            {
                serverName += "." + adAdapter.PrimaryDomainDnsName;
            }
            LdapConnection conn = new LdapConnection(new LdapDirectoryIdentifier(serverName));
            conn.Bind();
            SearchRequest req = new SearchRequest(
                "CN=Schema,CN=Configuration,"
                + adAdapter.rootDomainDN,
                "ldapdisplayname="
                + o,
                System.DirectoryServices.Protocols.SearchScope.Subtree,
                null);
            SearchResponse res = (SearchResponse)conn.SendRequest(req);
            DirectoryEntry objEntry = null;
            adAdapter.GetObjectByDN(res.Entries[0].DistinguishedName, out objEntry);
            try
            {
                string s = dcModel.GetClass(o).attributes[StandardNames.objectClass].UnderlyingValues[0].ToString();
                object[] objClass = (object[])objEntry.Properties["objectclass"].Value;
                foreach (string cls in objClass)
                {
                    string className = cls.ToLower();
                    if (
                        className.Equals("builtindomain")
                        || className.Equals("samserver")
                        || className.Equals("samdomain")
                        || className.Equals("group")
                        || className.Equals("user"))
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, ex.Message);
            }
            return false;
        }

        private bool SPCForLDS(string o)
        {
            string[] setOfClasses = new string[50];
            string serverName = adAdapter.PDCNetbiosName;
            if (serverOS >= OSVersion.WinSvr2008R2)
            {
                serverName += "." + adAdapter.PrimaryDomainDnsName;
            }
            LdapConnection conn = new LdapConnection(new LdapDirectoryIdentifier(serverName));
            conn.Bind();
            DirectoryEntry objEntry = null;
            string dn = String.Empty;
            adAdapter.GetLdsObjectByDN("CN=Schema,CN=Configuration," + adAdapter.LDSRootObjectName, out objEntry);
            try
            {
                SearchRequest req = new SearchRequest(
                    dn,
                    "ldapdisplayname="
                    + o,
                    System.DirectoryServices.Protocols.SearchScope.Subtree,
                    null);
                SearchResponse res = (SearchResponse)conn.SendRequest(req);
                DirectorySearcher ds = new DirectorySearcher(
                    objEntry,
                    "ldapdisplayname=" +
                    o,
                    new string[] { "distinguishedname" },
                    System.DirectoryServices.SearchScope.Subtree);
                System.DirectoryServices.SearchResult result = ds.FindOne();
                dn = result.Properties["distinguishedname"][0].ToString();
                adAdapter.GetLdsObjectByDN(dn, out objEntry);
            }
            catch (Exception ex)
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, ex.Message);
            }
            try
            {
                string s = dcModel.GetClass(o).attributes[StandardNames.objectClass].UnderlyingValues[0].ToString();
                object[] objClass = (object[])objEntry.Properties["objectclass"].Value;
                foreach (string cls in objClass)
                {
                    string className = cls.ToLower();
                    if (
                        className.Equals("builtindomain")
                        || className.Equals("samserver")
                        || className.Equals("samdomain")
                        || className.Equals("group")
                        || className.Equals("user"))
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, ex.Message);
            }
            return false;
        }

        #endregion
    }
}

