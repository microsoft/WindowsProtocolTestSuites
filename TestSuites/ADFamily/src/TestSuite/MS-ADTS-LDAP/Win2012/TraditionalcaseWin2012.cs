// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.Protocols;
using System.Security.Principal;
using System.Threading;
using System.Text;
using System.Net;


using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.Messages;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3;
using Microsoft.Protocols.TestTools.StackSdk.Transport;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;


namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Ldap
{
    /// <summary>
    /// MS-ADTS-LDAP traditional testcase
    /// </summary>
    [TestClass]
    public class TraditionalcaseWin2012 : TestClassBase
    {
        #region Variables

        AdtsLdapClient ldapClient;

        const string LDAP_SERVER_BATCH_REQUEST_OID = "1.2.840.113556.1.4.2212";

        #endregion

        #region private methods

        private void verifyExtendedControls_LDAP_SERVER_TREE_DELETE_EX_OID(bool isLDS)
        {
            string addr = AD_LDAPModelAdapter.Instance(Site).PDCIPAddress;
            string port = isLDS ? AD_LDAPModelAdapter.Instance(Site).ADLDSPortNum : AD_LDAPModelAdapter.Instance(Site).ADDSPortNum;
            LdapConnection con = new LdapConnection(
                new LdapDirectoryIdentifier(addr, int.Parse(port)),
                new NetworkCredential(AD_LDAPModelAdapter.Instance(Site).DomainAdministratorName,
                    AD_LDAPModelAdapter.Instance(Site).DomainUserPassword,
                    AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName));
            con.SessionOptions.Sealing = false;
            con.SessionOptions.Signing = false;

            #region Add a tree to the directory

            long numTreeObject = 4;
            string treeRootDN = "OU=testRootDN," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC;
            string treeEntry1 = "CN=testEntry1," + treeRootDN;
            string treeEntry2 = "CN=testEntry2," + treeRootDN;
            string treeEntry3 = "CN=testEntry3," + treeEntry2;

            if (!Utilities.IsObjectExist(treeRootDN, addr, port))
            {
                ManagedAddRequest add = new ManagedAddRequest(treeRootDN, "organizationalUnit");
                System.DirectoryServices.Protocols.AddResponse response = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(add);
            }
            if (!Utilities.IsObjectExist(treeEntry1, addr, port))
            {
                ManagedAddRequest add = new ManagedAddRequest(treeEntry1, "user");
                System.DirectoryServices.Protocols.AddResponse response = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(add);
            }
            if (!Utilities.IsObjectExist(treeEntry2, addr, port))
            {
                ManagedAddRequest add = new ManagedAddRequest(treeEntry2, "computer");
                System.DirectoryServices.Protocols.AddResponse response = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(add);
            }
            if (!Utilities.IsObjectExist(treeEntry3, addr, port))
            {
                ManagedAddRequest add = new ManagedAddRequest(treeEntry3, "classStore");
                System.DirectoryServices.Protocols.AddResponse response = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(add);
            }

            #endregion

            #region Delete Tree using extended control
            System.DirectoryServices.Protocols.DeleteRequest delreq = new System.DirectoryServices.Protocols.DeleteRequest(treeRootDN);
            TreeDeleteEx treeDelExCtrlVal = new TreeDeleteEx(numTreeObject);
            Asn1BerEncodingBuffer buffer = new Asn1BerEncodingBuffer();
            treeDelExCtrlVal.BerEncode(buffer, true);

            System.DirectoryServices.Protocols.DirectoryControl treeDelExConrol = new System.DirectoryServices.Protocols.DirectoryControl(
                ExtendedControl.LDAP_SERVER_TREE_DELETE_EX_OID,
                buffer.Data,
                false,
                true);
            delreq.Controls.Add(treeDelExConrol);
            System.DirectoryServices.Protocols.DeleteResponse delresp = (System.DirectoryServices.Protocols.DeleteResponse)con.SendRequest(delreq);
            BaseTestSite.Assert.AreEqual(ResultCode.Success, delresp.ResultCode, "The LDAP_SERVER_TREE_DELETE_EX_OID control is used with an LDAP delete operation to cause the server to recursively delete the entire subtree of objects located underneath the object specified in the delete operation.");
            #endregion
        }

        private void verifyExtendedControls_LDAP_SERVER_EXPECTED_ENTRY_COUNT_OID(bool isLDS)
        {
            string addr = AD_LDAPModelAdapter.Instance(Site).PDCIPAddress;
            string port = isLDS ? AD_LDAPModelAdapter.Instance(Site).ADLDSPortNum : AD_LDAPModelAdapter.Instance(Site).ADDSPortNum;
            LdapConnection con = new LdapConnection(
                new LdapDirectoryIdentifier(addr, int.Parse(port)),
                new NetworkCredential(AD_LDAPModelAdapter.Instance(Site).DomainAdministratorName,
                    AD_LDAPModelAdapter.Instance(Site).DomainUserPassword,
                    AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName));
            con.SessionOptions.Sealing = false;
            con.SessionOptions.Signing = false;

            #region Add a tree to the directory

            long numTreeObject = 4;
            string treeRootDN = "OU=testRootDN," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC;
            string treeEntry1 = "CN=testEntry1," + treeRootDN;
            string treeEntry2 = "CN=testEntry2," + treeRootDN;
            string treeEntry3 = "CN=testEntry3," + treeEntry2;

            if (!Utilities.IsObjectExist(treeRootDN, addr, port))
            {
                ManagedAddRequest add = new ManagedAddRequest(treeRootDN, "organizationalUnit");
                System.DirectoryServices.Protocols.AddResponse response = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(add);
            }
            if (!Utilities.IsObjectExist(treeEntry1, addr, port))
            {
                ManagedAddRequest add = new ManagedAddRequest(treeEntry1, "user");
                System.DirectoryServices.Protocols.AddResponse response = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(add);
            }
            if (!Utilities.IsObjectExist(treeEntry2, addr, port))
            {
                ManagedAddRequest add = new ManagedAddRequest(treeEntry2, "computer");
                System.DirectoryServices.Protocols.AddResponse response = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(add);
            }
            if (!Utilities.IsObjectExist(treeEntry3, addr, port))
            {
                ManagedAddRequest add = new ManagedAddRequest(treeEntry3, "classStore");
                System.DirectoryServices.Protocols.AddResponse response = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(add);
            }
            #endregion

            #region search directory
            System.DirectoryServices.Protocols.SearchRequest searchreq = new System.DirectoryServices.Protocols.SearchRequest(treeRootDN, "(ObjectClass=*)", System.DirectoryServices.Protocols.SearchScope.Subtree);
            System.DirectoryServices.Protocols.SearchResponse searchresp = (System.DirectoryServices.Protocols.SearchResponse)con.SendRequest(searchreq);
            BaseTestSite.Assert.AreEqual(ResultCode.Success, searchresp.ResultCode, "The LDAP_SERVER_TREE_DELETE_EX_OID control is used with an LDAP search operation to potentially modify the return code of the operation. ");
            #endregion

            #region search directory using extended control
            searchresp = null;
            ExpectedEntryCount expEntryCountCtrlVal = new ExpectedEntryCount(numTreeObject + 10, 100);
            Asn1BerEncodingBuffer buffer = new Asn1BerEncodingBuffer();
            expEntryCountCtrlVal.BerEncode(buffer, true);
            System.DirectoryServices.Protocols.DirectoryControl expEntryCount = new System.DirectoryServices.Protocols.DirectoryControl(
                ExtendedControl.LDAP_SERVER_EXPECTED_ENTRY_COUNT_OID,
                buffer.Data,
                false,
                true);
            searchreq.Controls.Add(expEntryCount);
            try
            {
                searchresp = (System.DirectoryServices.Protocols.SearchResponse)con.SendRequest(searchreq);
            }
            catch (System.DirectoryServices.Protocols.DirectoryOperationException e)
            {
                BaseTestSite.Assert.AreEqual(ResultCode.ConstraintViolation, e.Response.ResultCode, "When the search operation would normally return success/<unrestricted> and the number of searchEntries returned by the search is less than searchEntriesMin or greater than searchEntriesMax, the return code of the search operation is modified to be constraintViolation/<unrestricted>.");
            }

            #endregion
        }

        private void verifyExtendedControls_LDAP_SERVER_DIRSYNC_EX_OID(bool isLDS)
        {
            string addr = AD_LDAPModelAdapter.Instance(Site).PDCIPAddress;
            string port = isLDS ? AD_LDAPModelAdapter.Instance(Site).ADLDSPortNum : AD_LDAPModelAdapter.Instance(Site).ADDSPortNum;
            LdapConnection con = new LdapConnection(
                new LdapDirectoryIdentifier(addr, int.Parse(port)),
                new NetworkCredential(AD_LDAPModelAdapter.Instance(Site).DomainAdministratorName,
                    AD_LDAPModelAdapter.Instance(Site).DomainUserPassword,
                    AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName));
            con.SessionOptions.Sealing = false;
            con.SessionOptions.Signing = false;

            #region Add a tree to the directory

            string treeRootDN = "OU=testRootDN," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC;
            string treeEntry1 = "CN=testEntry1," + treeRootDN;

            if (!Utilities.IsObjectExist(treeRootDN, addr, port))
            {
                ManagedAddRequest add = new ManagedAddRequest(treeRootDN, "organizationalUnit");
                System.DirectoryServices.Protocols.AddResponse response = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(add);
            }
            if (!Utilities.IsObjectExist(treeEntry1, addr, port))
            {
                ManagedAddRequest add = new ManagedAddRequest(treeEntry1, "computer");
                System.DirectoryServices.Protocols.AddResponse response = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(add);
            }
            #endregion

            #region search directory using extended control
            //The LDAP_SERVER_DIRSYNC_OID control can only be used to monitor for changes across an entire NC replica, not a subtree within an NC replica.
            string dn = AD_LDAPModelAdapter.Instance(Site).rootDomainNC;
            string[] attributesToReturn = new string[] { "displayName", "name;dirSyncAlwaysReturn" };
            System.DirectoryServices.Protocols.SearchRequest searchreq = new System.DirectoryServices.Protocols.SearchRequest(
                dn,
                "(ObjectClass=computer)",
                System.DirectoryServices.Protocols.SearchScope.Subtree,
                attributesToReturn);
            //incremental flag and object security flag are set
            long tmpFlag = 0x80000001;
            Int32 flag = (Int32)tmpFlag;
            Asn1Integer flagsVal = new Asn1Integer(flag);
            long maxbytes = 0x100000;
            byte[] cookie = null;
            DirsyncExReq dirsyncExReqCtrlVal = new DirsyncExReq(flag, maxbytes, cookie);
            Asn1BerEncodingBuffer encBuf = new Asn1BerEncodingBuffer();
            dirsyncExReqCtrlVal.BerEncode(encBuf, true);
            System.DirectoryServices.Protocols.DirectoryControl dirsyncExReq = new System.DirectoryServices.Protocols.DirectoryControl(
                ExtendedControl.LDAP_SERVER_DIRSYNC_EX_OID,
                encBuf.Data,
                true,
                true);
            searchreq.Controls.Add(dirsyncExReq);
            System.DirectoryServices.Protocols.SearchResponse searchresp = (System.DirectoryServices.Protocols.SearchResponse)con.SendRequest(searchreq);
            BaseTestSite.Assert.AreEqual(ResultCode.Success, searchresp.ResultCode, "The LDAP_SERVER_DIRSYNC_EX_OID control is used with an LDAP search operation to retrieve the changes made to objects since a previous search with an LDAP_SERVER_DIRSYNC_EX_OID control was performed.");
            BaseTestSite.Assert.AreEqual(ExtendedControl.LDAP_SERVER_DIRSYNC_EX_OID, searchresp.Controls[0].Type, "The controlType field of the returned Control structure is set to the OID of the LDAP_SERVER_DIRSYNC_EX_OID control.");
            Asn1DecodingBuffer decBuf = new Asn1DecodingBuffer(searchresp.Controls[0].GetValue());
            DirsyncExRep dirsyncExRepCtrlVal = new DirsyncExRep();
            dirsyncExRepCtrlVal.BerDecode(decBuf);
            if (dirsyncExRepCtrlVal.moreResults.Value != 0)
            {
                BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "MoreResults is {0}, there are more changes to retrieve.", dirsyncExRepCtrlVal.moreResults.Value);
            }
            cookie = dirsyncExRepCtrlVal.cookieServer.ByteArrayValue;
            #endregion

            #region make some changes to the original directory
            string attrValAfterChange = "testEntryDisplayName";
            System.DirectoryServices.Protocols.ModifyRequest modreq = new System.DirectoryServices.Protocols.ModifyRequest(treeEntry1, DirectoryAttributeOperation.Replace, attributesToReturn[0], attrValAfterChange);
            System.DirectoryServices.Protocols.ModifyResponse modrep = (System.DirectoryServices.Protocols.ModifyResponse)con.SendRequest(modreq);
            #endregion

            #region second search request
            dirsyncExReqCtrlVal = new DirsyncExReq(flag, maxbytes, cookie);
            dirsyncExReqCtrlVal.BerEncode(encBuf, true);
            dirsyncExReq = new System.DirectoryServices.Protocols.DirectoryControl(
                ExtendedControl.LDAP_SERVER_DIRSYNC_EX_OID,
                encBuf.Data,
                true,
                true);
            searchreq.Controls.Clear();
            searchreq.Controls.Add(dirsyncExReq);
            searchresp = (System.DirectoryServices.Protocols.SearchResponse)con.SendRequest(searchreq);
            BaseTestSite.Assert.AreEqual(ResultCode.Success, searchresp.ResultCode, "The LDAP_SERVER_DIRSYNC_EX_OID control is used with an LDAP search operation to retrieve the changes made to objects since a previous search with an LDAP_SERVER_DIRSYNC_EX_OID control was performed.");
            // Deleted objects in Deleted Objects container will also be returned
            BaseTestSite.Assert.IsTrue(searchresp.Entries.Count >= 1, "Only those objects for which these attributes have been created or modified since the time represented by Cookie will be considered for inclusion in the search.");

            bool found = false;
            foreach (System.DirectoryServices.Protocols.SearchResultEntry sr in searchresp.Entries)
            {
                BaseTestSite.Assert.IsTrue(sr.Attributes.Contains("objectGUID"), "The search results MUST always contain the objectGUID attribute, even if it is not specified in the search request.");
                BaseTestSite.Assert.IsTrue(sr.Attributes.Contains("instanceType"), "The search results MUST always contain the instanceType attribute, even if it is not specified in the search request.");
                BaseTestSite.Assert.IsTrue(sr.Attributes.Contains("name"), "However, where the LDAP_SERVER_DIRSYNC_OID control returns only those attributes that have changed, the LDAP_SERVER_DIRSYNC_EX_OID control also returns unchanged attributes when the attribute name in the request is appended with the string ;dirSyncAlwaysReturn.");
                if (sr.Attributes["name"][0].ToString().Contains("\nDEL:"))
                {
                    continue;
                }
                if (sr.Attributes.Contains("displayName")
                    && sr.Attributes["displayName"].Count != 0
                    && sr.Attributes["displayName"][0].Equals(attrValAfterChange))
                {
                    found = true;
                }
            }
            BaseTestSite.Assert.IsTrue(found, "The search results MUST return the attributes that have been changed.");

            #endregion
        }

        private void verifyExtendedControls_LDAP_SERVER_UPDATE_STATS_OID(bool isLDS)
        {
            string addr = AD_LDAPModelAdapter.Instance(Site).PDCIPAddress;
            string port = isLDS ? AD_LDAPModelAdapter.Instance(Site).ADLDSPortNum : AD_LDAPModelAdapter.Instance(Site).ADDSPortNum;
            LdapConnection con = new LdapConnection(
                new LdapDirectoryIdentifier(addr, int.Parse(port)),
                new NetworkCredential(AD_LDAPModelAdapter.Instance(Site).DomainAdministratorName,
                    AD_LDAPModelAdapter.Instance(Site).DomainUserPassword,
                    AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName));
            con.SessionOptions.Sealing = false;
            con.SessionOptions.Signing = false;

            #region Add an OU in the directory
            string treeRootDN = "OU=testRootDN," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC;
            string treeEntry1 = "CN=testEntry1," + treeRootDN;

            if (!Utilities.IsObjectExist(treeRootDN, addr, port))
            {
                ManagedAddRequest add = new ManagedAddRequest(treeRootDN, "organizationalUnit");
                System.DirectoryServices.Protocols.AddResponse response = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(add);
            }
            #endregion

            #region add an entry to the directory using extended control
            ManagedAddRequest addreq = new ManagedAddRequest(treeEntry1, "user");
            System.DirectoryServices.Protocols.DirectoryControl updateStatsReq = new System.DirectoryServices.Protocols.DirectoryControl(
                ExtendedControl.LDAP_SERVER_UPDATE_STATS_OID,
                null,
                false,
                true);
            addreq.Controls.Add(updateStatsReq);
            System.DirectoryServices.Protocols.AddResponse addresp = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(addreq);
            BaseTestSite.Assert.AreEqual(ResultCode.Success, addresp.ResultCode, "The LDAP operation should succeed.");
            BaseTestSite.Assert.AreEqual(ExtendedControl.LDAP_SERVER_UPDATE_STATS_OID, addresp.Controls[0].Type, "When the server receives a request with the LDAP_SERVER_UPDATE_STATS_OID control attached to it, the server includes a response control in the response that contains statistics.");
            Asn1DecodingBuffer decBuf = new Asn1DecodingBuffer(addresp.Controls[0].GetValue());
            UpdateStats updateStatsVal = new UpdateStats();
            updateStatsVal.BerDecode(decBuf);

            BaseTestSite.Assert.AreEqual("1.2.840.113556.1.4.2209", System.Text.Encoding.ASCII.GetString(updateStatsVal.Elements[0].statID.ByteArrayValue).Replace("\0", ""), "Invocation ID Of Server 1.2.840.113556.1.4.2194");
            BaseTestSite.Assert.IsNotNull(updateStatsVal.Elements[0].statValue.Value, "The statValue for this statID contains dc.invocationId. This value is returned in little-endian byte order.");
            BaseTestSite.Assert.AreEqual("1.2.840.113556.1.4.2208", System.Text.Encoding.ASCII.GetString(updateStatsVal.Elements[1].statID.ByteArrayValue).Replace("\0", ""), "Highest USN Allocated 1.2.840.113556.1.4.2193");
            BaseTestSite.Assert.IsNotNull(updateStatsVal.Elements[1].statValue.Value, "The statValue for this statID contains the highest USN that the DC allocated during the LDAP operation.");
            #endregion
        }

        private void verifyExtendedControls_LDAP_SERVER_SEARCH_HINTS_OID(bool isLDS)
        {
            string addr = AD_LDAPModelAdapter.Instance(Site).PDCIPAddress;
            string port = isLDS ? AD_LDAPModelAdapter.Instance(Site).ADLDSPortNum : AD_LDAPModelAdapter.Instance(Site).ADDSPortNum;
            LdapConnection con = new LdapConnection(
                new LdapDirectoryIdentifier(addr, int.Parse(port)),
                new NetworkCredential(AD_LDAPModelAdapter.Instance(Site).DomainAdministratorName,
                    AD_LDAPModelAdapter.Instance(Site).DomainUserPassword,
                    AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName));
            con.SessionOptions.Sealing = false;
            con.SessionOptions.Signing = false;

            #region Add a tree to the directory

            string treeRootDN = "OU=testRootDN," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC;
            string treeEntry1 = "CN=testEntry1," + treeRootDN;
            string treeEntry2 = "CN=testEntry2," + treeRootDN;
            string treeEntry3 = "CN=testEntry3," + treeRootDN;
            string treeEntry4 = "CN=testEntry4," + treeRootDN;
            string treeEntry5 = "CN=testEntry5," + treeRootDN;

            if (!Utilities.IsObjectExist(treeRootDN, addr, port))
            {
                ManagedAddRequest add = new ManagedAddRequest(treeRootDN, "organizationalUnit");
                add.Attributes.Add(new DirectoryAttribute("displayName", "aaa"));
                System.DirectoryServices.Protocols.AddResponse response = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(add);
            }
            if (!Utilities.IsObjectExist(treeEntry1, addr, port))
            {
                ManagedAddRequest add = new ManagedAddRequest(treeEntry1, "user");
                add.Attributes.Add(new DirectoryAttribute("displayName", "bbb"));
                System.DirectoryServices.Protocols.AddResponse response = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(add);
            }
            if (!Utilities.IsObjectExist(treeEntry2, addr, port))
            {
                ManagedAddRequest add = new ManagedAddRequest(treeEntry2, "user");
                add.Attributes.Add(new DirectoryAttribute("displayName", "ddd"));
                System.DirectoryServices.Protocols.AddResponse response = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(add);
            }
            if (!Utilities.IsObjectExist(treeEntry3, addr, port))
            {
                ManagedAddRequest add = new ManagedAddRequest(treeEntry3, "user");
                add.Attributes.Add(new DirectoryAttribute("displayName", "ccc"));
                System.DirectoryServices.Protocols.AddResponse response = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(add);
            }
            if (!Utilities.IsObjectExist(treeEntry4, addr, port))
            {
                ManagedAddRequest add = new ManagedAddRequest(treeEntry4, "user");
                add.Attributes.Add(new DirectoryAttribute("displayName", "ddd"));
                System.DirectoryServices.Protocols.AddResponse response = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(add);
            }
            if (!Utilities.IsObjectExist(treeEntry5, addr, port))
            {
                ManagedAddRequest add = new ManagedAddRequest(treeEntry5, "user");
                add.Attributes.Add(new DirectoryAttribute("displayName", "eee"));
                System.DirectoryServices.Protocols.AddResponse response = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(add);
            }
            #endregion

            #region search directory using extended control
            System.DirectoryServices.Protocols.SearchRequest searchreq = 
                new System.DirectoryServices.Protocols.SearchRequest(
                    treeRootDN,
                    "(ObjectClass=*)",
                    System.DirectoryServices.Protocols.SearchScope.Subtree,
                    "displayName");
            //displayName is an attribue with IX flags set on searchFlags attribute
            //The exactly what an index is in relationship to a DC is implementation-specific
            System.DirectoryServices.Protocols.SortRequestControl sortCtrl = new SortRequestControl("displayName", false);
            searchreq.Controls.Add(sortCtrl);
            searchreq.SizeLimit = 6;
            #endregion

            #region Require Sort Index and Soft Size Limit

            Asn1BerEncodingBuffer buffer = new Asn1BerEncodingBuffer();
            SearchHints searchHintsCtrlVal = new SearchHints(2);
            RequireSortIndexHintValue requireSortIndex = new RequireSortIndexHintValue(new Asn1Boolean(true));
            requireSortIndex.BerEncode(buffer, true);
            searchHintsCtrlVal.Elements[0] = new SearchHints_element(
                new LDAPOID("1.2.840.113556.1.4.2207"),
                buffer.Data);
            //buffer.Reset();

            // "softSizeLimit" is not taking any effect:
            // expecting the search result will return 5 elements in the list.
            // The 4th and the 5th object have the same value. Therefore, the 6th object is removed.
            buffer = new Asn1BerEncodingBuffer();
            SoftSizeLimitHintValue softSizeLimit = new SoftSizeLimitHintValue(new Asn1Integer(4));
            softSizeLimit.BerEncode(buffer, true);
            searchHintsCtrlVal.Elements[1] = new SearchHints_element(
                new LDAPOID("1.2.840.113556.1.4.2210"),
                buffer.Data);
            //buffer.Reset();

            searchHintsCtrlVal.BerEncode(buffer, true);

            System.DirectoryServices.Protocols.DirectoryControl searchHintsCtrl = new System.DirectoryServices.Protocols.DirectoryControl(
                ExtendedControl.LDAP_SERVER_SEARCH_HINTS_OID,
                buffer.Data,
                true,
                true);
            searchreq.Controls.Add(searchHintsCtrl);
            System.DirectoryServices.Protocols.SearchResponse searchresp;
            try
            {
                searchresp = (System.DirectoryServices.Protocols.SearchResponse)con.SendRequest(searchreq);
                BaseTestSite.Assert.Fail("The LDAP search operation must not be succeeded because SoftSizeLimit should be taking effect.");
            }
            catch (System.DirectoryServices.Protocols.DirectoryOperationException e)
            {
                BaseTestSite.Assert.AreEqual(
                    ResultCode.SizeLimitExceeded,
                    e.Response.ResultCode,
                    @"The LDAP_SERVER_SEARCH_HINTS_OID control is used with an LDAP search operation. This control supplies hints to the search operation on how to satisfy the search.
                    If one or more objects are removed from the list according to the earlier algorithm, the search operation will return sizeLimitExceeded / <unrestricted>.");
            }
            #endregion
        }

        private void verifyExtendedControls_LDAP_SERVER_SET_OWNER_OID(bool isLDS)
        {
            string addr = AD_LDAPModelAdapter.Instance(Site).PDCIPAddress;
            string port = isLDS ? AD_LDAPModelAdapter.Instance(Site).ADLDSPortNum : AD_LDAPModelAdapter.Instance(Site).ADDSPortNum;
            LdapConnection con = new LdapConnection(
                new LdapDirectoryIdentifier(addr, int.Parse(port)),
                new NetworkCredential(AD_LDAPModelAdapter.Instance(Site).DomainAdministratorName,
                    AD_LDAPModelAdapter.Instance(Site).DomainUserPassword,
                    AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName));
            con.SessionOptions.Sealing = false;
            con.SessionOptions.Signing = false;

            #region Add an OU in the directory
            string treeRootDN = "OU=testRootDN," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC;
            string treeEntry1 = "CN=testEntry1," + treeRootDN;

            if (!Utilities.IsObjectExist(treeRootDN, addr, port))
            {
                ManagedAddRequest add = new ManagedAddRequest(treeRootDN, "organizationalUnit");
                System.DirectoryServices.Protocols.AddResponse response = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(add);
            }
            #endregion

            #region add an entry to the directory using extended control
            ManagedAddRequest addreq = new ManagedAddRequest(treeEntry1, "user");
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
            UserPrincipal user = UserPrincipal.FindByIdentity(ctx, AD_LDAPModelAdapter.Instance(Site).testUser7Name);
            BaseTestSite.Assert.IsNotNull(user, "The owner for the new added object MUST exist!");
            byte[] userSid = Encoding.UTF8.GetBytes(user.Sid.Value);

            // This is a critical control only
            System.DirectoryServices.Protocols.DirectoryControl setOwnerOid = new System.DirectoryServices.Protocols.DirectoryControl(
                ExtendedControl.LDAP_SERVER_SET_OWNER_OID,
                userSid,
                true,
                true);
            addreq.Controls.Add(setOwnerOid);
            System.DirectoryServices.Protocols.AddResponse addresp = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(addreq);
            BaseTestSite.Assert.AreEqual(ResultCode.Success, addresp.ResultCode, "The LDAP operation should succeed.");

            System.DirectoryServices.Protocols.SearchRequest searchReq = new System.DirectoryServices.Protocols.SearchRequest(
                treeEntry1,
                "(objectclass=*)",
                System.DirectoryServices.Protocols.SearchScope.Base,
                new string[] { "ntSecurityDescriptor" });
            System.DirectoryServices.Protocols.SearchResponse searchResp = (System.DirectoryServices.Protocols.SearchResponse)con.SendRequest(searchReq);
            BaseTestSite.Assert.AreEqual<ResultCode>(ResultCode.Success, searchResp.ResultCode, "should return success when query ntSecurityDescriptor");
            DirectoryAttribute attr = searchResp.Entries[0].Attributes["ntSecurityDescriptor"];
            object[] values = attr.GetValues(Type.GetType("System.Byte[]"));
            byte[] sd = (byte[])values[0];
            ActiveDirectorySecurity securityDescriptor = new ActiveDirectorySecurity();
            securityDescriptor.SetSecurityDescriptorBinaryForm(sd);
            //GetsSecurityDescriptorOwner method will return the owner part of Secuirty Descriptor
            string owner = Utilities.GetSecurityDescriptorOwner(securityDescriptor);
            BaseTestSite.Assert.IsTrue(owner.Contains(AD_LDAPModelAdapter.Instance(Site).testUser7Name), "The owner is to be set into the owner portion of the security descriptor stored in the ntSecurityDescriptor attribute of the object to be created.");

            #endregion
        }

        #endregion

        #region Test Suite Initialization

        /// <summary>
        /// Class initialization
        /// </summary>
        /// <param name="testContext">test context</param>
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassBase.Initialize(testContext);
            EnvironmentConfig.ServerVer = (ServerVersion)AD_LDAPModelAdapter.Instance(BaseTestSite).PDCOSVersion;
        }

        /// <summary>
        /// Class cleanup
        /// </summary>
        [ClassCleanup]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }

        #endregion

        #region Test Case Initialization and clean up

        /// <summary>
        /// Test initialize
        /// </summary>
        protected override void TestInitialize()
        {
            base.TestInitialize();
            Site.DefaultProtocolDocShortName = "MS-ADTS-LDAP";
            AD_LDAPModelAdapter.Instance(Site).Initialize();
            Utilities.DomainAdmin = AD_LDAPModelAdapter.Instance(Site).DomainAdministratorName;
            Utilities.DomainAdminPassword = AD_LDAPModelAdapter.Instance(Site).DomainUserPassword;
            Utilities.TargetServerFqdn = AD_LDAPModelAdapter.Instance(Site).PDCNetbiosName + "." + AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName + ":" + AD_LDAPModelAdapter.Instance(Site).ADDSPortNum + "/";
            
            using (LdapConnection con = new LdapConnection(
                new LdapDirectoryIdentifier(AD_LDAPModelAdapter.Instance(Site).PDCIPAddress, int.Parse(AD_LDAPModelAdapter.Instance(Site).ADDSPortNum)),
                new NetworkCredential(AD_LDAPModelAdapter.Instance(Site).DomainAdministratorName,
                    AD_LDAPModelAdapter.Instance(Site).DomainUserPassword,
                    AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName)))
            {
                con.SessionOptions.Sealing = false;
                con.SessionOptions.Signing = false;


                string treeRootDN = "OU=testRootDN," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC;

                if (Utilities.IsObjectExist(treeRootDN, AD_LDAPModelAdapter.Instance(Site).PDCIPAddress, AD_LDAPModelAdapter.Instance(Site).ADDSPortNum))
                {
                    System.DirectoryServices.Protocols.DeleteRequest delreq = new System.DirectoryServices.Protocols.DeleteRequest(treeRootDN);
                    System.DirectoryServices.Protocols.TreeDeleteControl treeDelCtrl = new TreeDeleteControl();
                    delreq.Controls.Add(treeDelCtrl);
                    try
                    {
                        System.DirectoryServices.Protocols.DeleteResponse response = (System.DirectoryServices.Protocols.DeleteResponse)con.SendRequest(delreq);
                    }
                    catch
                    {
                    }
                }
            }

        }

        /// <summary>
        /// Test clean up
        /// </summary>
        protected override void TestCleanup()
        {
            string addr = AD_LDAPModelAdapter.Instance(Site).PDCIPAddress;
            string port = AD_LDAPModelAdapter.Instance(Site).ADDSPortNum;
            LdapConnection con = new LdapConnection(
                new LdapDirectoryIdentifier(addr),
                new NetworkCredential(AD_LDAPModelAdapter.Instance(Site).DomainAdministratorName,
                    AD_LDAPModelAdapter.Instance(Site).DomainUserPassword,
                    AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName));
            con.SessionOptions.Sealing = false;
            con.SessionOptions.Signing = false;

            string treeRootDN = "OU=testRootDN," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC;

            if (Utilities.IsObjectExist(treeRootDN, addr, port))
            {
                System.DirectoryServices.Protocols.DeleteRequest delreq = new System.DirectoryServices.Protocols.DeleteRequest(treeRootDN);
                System.DirectoryServices.Protocols.TreeDeleteControl treeDelCtrl = new TreeDeleteControl();
                delreq.Controls.Add(treeDelCtrl);
                try
                {
                    System.DirectoryServices.Protocols.DeleteResponse response = (System.DirectoryServices.Protocols.DeleteResponse)con.SendRequest(delreq);
                }
                catch
                {
                }
            }

            System.DirectoryServices.Protocols.ModifyRequest mod = new System.DirectoryServices.Protocols.ModifyRequest("",
                DirectoryAttributeOperation.Add, "schemaupgradeinprogress", "0");
            con.SendRequest(mod);
            base.TestCleanup();
        }

        #endregion

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2012")]
		[TestCategory("ForestWin2012")]
        [TestCategory("Main")]
        public void LDAP_Add_Object_ClaimType_WithoutADEXTPrefix()
        {
            BaseTestSite.Assume.IsTrue(EnvironmentConfig.ServerVer >= ServerVersion.Win2012, "Server OS version should be not less than Windows Server 2012");
            LdapConnection con = new LdapConnection(AD_LDAPModelAdapter.Instance(Site).PDCIPAddress);
            string dn = "cn=testclaim_withoutprefix, cn=claim types, cn=claims configuration, cn=services,cn=configuration," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC;
            DeleteRequest dr = new DeleteRequest(dn);
            dr.Controls.Add(new TreeDeleteControl());
            try
            {
                con.SendRequest(dr);
            }
            catch
            {
            }
            System.DirectoryServices.Protocols.AddRequest add = new System.DirectoryServices.Protocols.AddRequest(dn, "msds-claimtype");
            con.SendRequest(add);
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2012")]
		[TestCategory("ForestWin2012")]
        [TestCategory("Main")]
        public void LDAP_ExtendedControlsDS_LDAP_SERVER_TREE_DELETE_EX_OID()
        {
            BaseTestSite.Assume.IsTrue(EnvironmentConfig.ServerVer >= ServerVersion.Win2012, "Server OS version should be not less than Windows Server 2012");
            verifyExtendedControls_LDAP_SERVER_TREE_DELETE_EX_OID(false);
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2012")]
		[TestCategory("ForestWin2012")]
        [TestCategory("Main")]
        public void LDAP_ExtendedControlsDS_LDAP_SERVER_EXPECTED_ENTRY_COUNT_OID()
        {
            BaseTestSite.Assume.IsTrue(EnvironmentConfig.ServerVer >= ServerVersion.Win2012, "Server OS version should be not less than Windows Server 2012");
            verifyExtendedControls_LDAP_SERVER_EXPECTED_ENTRY_COUNT_OID(false);
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2012")]
		[TestCategory("ForestWin2012")]
        [TestCategory("Main")]
        public void LDAP_ExtendedControlsDS_LDAP_SERVER_DIRSYNC_EX_OID()
        {
            BaseTestSite.Assume.IsTrue(EnvironmentConfig.ServerVer >= ServerVersion.Win2012, "Server OS version should be not less than Windows Server 2012");
            verifyExtendedControls_LDAP_SERVER_DIRSYNC_EX_OID(false);
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2012")]
		[TestCategory("ForestWin2012")]
        [TestCategory("Main")]
        public void LDAP_ExtendedControlsDS_LDAP_SERVER_UPDATE_STATS_OID()
        {
            BaseTestSite.Assume.IsTrue(EnvironmentConfig.ServerVer >= ServerVersion.Win2012, "Server OS version should be not less than Windows Server 2012");
            verifyExtendedControls_LDAP_SERVER_UPDATE_STATS_OID(false);
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2012")]
		[TestCategory("ForestWin2012")]
        [TestCategory("Main")]
        public void LDAP_ExtendedControlsDS_LDAP_SERVER_SEARCH_HINTS_OID()
        {
            BaseTestSite.Assume.IsTrue(EnvironmentConfig.ServerVer >= ServerVersion.Win2012, "Server OS version should be not less than Windows Server 2012");
            verifyExtendedControls_LDAP_SERVER_SEARCH_HINTS_OID(false);
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2012")]
		[TestCategory("ForestWin2012")]
        [TestCategory("Main")]
        public void LDAP_ExtendedControlsDS_LDAP_SERVER_SET_OWNER_OID()
        {
            BaseTestSite.Assume.IsTrue(EnvironmentConfig.ServerVer >= ServerVersion.Win2012R2, "Server OS version should be not less than Windows Server 2012 R2");
            verifyExtendedControls_LDAP_SERVER_SET_OWNER_OID(false);
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2012")]
		[TestCategory("ForestWin2012")]
        [TestCategory("Main")]
        public void LDAP_AD_DS_Add_Constraints_SetOwnerControl_ntSecurityDescriptor()
        {
            BaseTestSite.Assume.IsTrue(EnvironmentConfig.ServerVer >= ServerVersion.Win2012R2, "Server OS version should be not less than Windows Server 2012 R2");

            #region Connect

            string addr = AD_LDAPModelAdapter.Instance(Site).PDCIPAddress;
            string port = AD_LDAPModelAdapter.Instance(Site).ADDSPortNum;
            LdapConnection con = new LdapConnection(
                new LdapDirectoryIdentifier(addr, int.Parse(port)),
                new NetworkCredential(AD_LDAPModelAdapter.Instance(Site).testUserName,
                    AD_LDAPModelAdapter.Instance(Site).testUserPwd,
                    AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName));
            con.SessionOptions.Sealing = false;
            con.SessionOptions.Signing = false;

            #endregion

            #region Add an OU in the directory

            string treeRootDN = "OU=testRootDN," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC;
            string treeEntry1 = "CN=testEntry1," + treeRootDN;

            if (!Utilities.IsObjectExist(treeRootDN, addr, port))
            {
                ManagedAddRequest add = new ManagedAddRequest(treeRootDN, "organizationalUnit");
                System.DirectoryServices.Protocols.AddResponse response = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(add);
            }

            #endregion

            #region Make an add request with ntSecurityDescriptor attribute specified

            ManagedAddRequest addreq = new ManagedAddRequest(treeEntry1, "user");
            ActiveDirectorySecurity securityDescriptor = new ActiveDirectorySecurity();
            // Set Owner as CONTOSO\administrator
            securityDescriptor.SetOwner(new NTAccount(AD_LDAPModelAdapter.Instance(Site).PrimaryDomainNetBiosName + "\\" + AD_LDAPModelAdapter.Instance(Site).DomainAdministratorName));
            addreq.Attributes.Add(new DirectoryAttribute("ntSecurityDescriptor", securityDescriptor.GetSecurityDescriptorBinaryForm()));

            #endregion

            #region Attach Set Owner extended control to this add request
            // Set Owner as CONTOSO\adts_user10
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
            UserPrincipal user = UserPrincipal.FindByIdentity(ctx, AD_LDAPModelAdapter.Instance(Site).testUser7Name);
            BaseTestSite.Assert.IsNotNull(user, "The owner for the new added object MUST exist!");
            byte[] userSid = Encoding.UTF8.GetBytes(user.Sid.Value);
            // This is a critical control only
            System.DirectoryServices.Protocols.DirectoryControl setOwnerOid = new System.DirectoryServices.Protocols.DirectoryControl(
                ExtendedControl.LDAP_SERVER_SET_OWNER_OID,
                userSid,
                true,
                true);
            addreq.Controls.Add(setOwnerOid);

            #endregion

            #region send add request and receive add response

            System.DirectoryServices.Protocols.AddResponse addresp = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(addreq);
            BaseTestSite.Assert.AreEqual(ResultCode.Success, addresp.ResultCode, "The LDAP operation should succeed.");

            #endregion

            #region verify object owner

            System.DirectoryServices.Protocols.SearchRequest searchReq = new System.DirectoryServices.Protocols.SearchRequest(
                treeEntry1,
                "(objectclass=*)",
                System.DirectoryServices.Protocols.SearchScope.Base,
                new string[] { "ntSecurityDescriptor" });
            System.DirectoryServices.Protocols.SearchResponse searchResp = (System.DirectoryServices.Protocols.SearchResponse)con.SendRequest(searchReq);
            BaseTestSite.Assert.AreEqual<ResultCode>(ResultCode.Success, searchResp.ResultCode, "should return success when query ntSecurityDescriptor");
            DirectoryAttribute attr = searchResp.Entries[0].Attributes["ntSecurityDescriptor"];
            object[] values = attr.GetValues(Type.GetType("System.Byte[]"));
            byte[] sd = (byte[])values[0];
            securityDescriptor.SetSecurityDescriptorBinaryForm(sd);
            //GetsSecurityDescriptorOwner method will return the owner part of Secuirty Descriptor
            string owner = Utilities.GetSecurityDescriptorOwner(securityDescriptor);
            //Object owner should be set by SetOwner control when specified, even if nTSecurityDescriptor is provided
            BaseTestSite.Assert.IsTrue(owner.Contains(AD_LDAPModelAdapter.Instance(Site).testUser7Name),
                @"If the requester has specified an owner using the LDAP_SERVER_SET_OWNER_OID LDAP control
                and has specified a value for the nTSecurityDescriptor, the owner in the security descriptor
                is set to the owner supplied by the control.");

            #endregion
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2012")]
		[TestCategory("ForestWin2012")]
        [TestCategory("Main")]
        public void LDAP_AD_DS_Add_Constraints_ntSecurityDescriptor()
        {
            BaseTestSite.Assume.IsTrue(EnvironmentConfig.ServerVer >= ServerVersion.Win2012R2, "Server OS version should be not less than Windows Server 2012 R2");

            #region Connect

            string addr = AD_LDAPModelAdapter.Instance(Site).PDCIPAddress;
            string port = AD_LDAPModelAdapter.Instance(Site).ADDSPortNum;
            LdapConnection con = new LdapConnection(
                new LdapDirectoryIdentifier(addr, int.Parse(port)),
                new NetworkCredential(AD_LDAPModelAdapter.Instance(Site).testUserName,
                    AD_LDAPModelAdapter.Instance(Site).testUserPwd,
                    AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName));
            con.SessionOptions.Sealing = false;
            con.SessionOptions.Signing = false;

            #endregion

            #region Add an OU in the directory

            string treeRootDN = "OU=testRootDN," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC;
            string treeEntry1 = "CN=testEntry1," + treeRootDN;

            if (!Utilities.IsObjectExist(treeRootDN, addr, port))
            {
                ManagedAddRequest add = new ManagedAddRequest(treeRootDN, "organizationalUnit");
                System.DirectoryServices.Protocols.AddResponse response = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(add);
            }

            #endregion

            #region Make an add request with ntSecurityDescriptor attribute specified

            ManagedAddRequest addreq = new ManagedAddRequest(treeEntry1, "user");
            ActiveDirectorySecurity securityDescriptor = new ActiveDirectorySecurity();
            // Set Owner as CONTOSO\administrator
            securityDescriptor.SetOwner(new NTAccount(AD_LDAPModelAdapter.Instance(Site).PrimaryDomainNetBiosName + "\\" + AD_LDAPModelAdapter.Instance(Site).DomainAdministratorName));
            addreq.Attributes.Add(new DirectoryAttribute("ntSecurityDescriptor", securityDescriptor.GetSecurityDescriptorBinaryForm()));

            #endregion

            #region send add request and receive add response

            System.DirectoryServices.Protocols.AddResponse addresp = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(addreq);
            BaseTestSite.Assert.AreEqual(ResultCode.Success, addresp.ResultCode, "The LDAP operation should succeed.");

            #endregion

            #region verify object owner

            System.DirectoryServices.Protocols.SearchRequest searchReq = new System.DirectoryServices.Protocols.SearchRequest(
                treeEntry1,
                "(objectclass=*)",
                System.DirectoryServices.Protocols.SearchScope.Base,
                new string[] { "ntSecurityDescriptor" });
            System.DirectoryServices.Protocols.SearchResponse searchResp = (System.DirectoryServices.Protocols.SearchResponse)con.SendRequest(searchReq);
            BaseTestSite.Assert.AreEqual<ResultCode>(ResultCode.Success, searchResp.ResultCode, "should return success when query ntSecurityDescriptor");
            DirectoryAttribute attr = searchResp.Entries[0].Attributes["ntSecurityDescriptor"];
            object[] values = attr.GetValues(Type.GetType("System.Byte[]"));
            byte[] sd = (byte[])values[0];
            securityDescriptor.SetSecurityDescriptorBinaryForm(sd);
            //GetsSecurityDescriptorOwner method will return the owner part of Secuirty Descriptor
            string owner = Utilities.GetSecurityDescriptorOwner(securityDescriptor);
            //Object owner should be set by SetOwner control when specified, even if nTSecurityDescriptor is provided
            BaseTestSite.Assert.IsTrue(owner.ToLower(CultureInfo.InvariantCulture).Contains(AD_LDAPModelAdapter.Instance(Site).DomainAdministratorName.ToLower(CultureInfo.InvariantCulture)),
                @"If the requester has not specified an owner using the LDAP_SERVER_SET_OWNER_OID LDAP
                control but has specified a value for nTSecurityDescriptor, the value is a valid security
                descriptor value in self-relative format, and it satisfies the security descriptor constraints.");

            #endregion
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2012")]
		[TestCategory("ForestWin2012")]
        [TestCategory("Main")]
        public void LDAP_AD_DS_RootDSEModify_schema_upgrade_in_progress()
        {
            BaseTestSite.Assume.IsTrue(EnvironmentConfig.ServerVer >= ServerVersion.Win2012, "Server OS version should be not less than Windows Server 2012");
            LdapConnection con = new LdapConnection(new LdapDirectoryIdentifier(AD_LDAPModelAdapter.Instance(Site).PDCIPAddress),
              new NetworkCredential(AD_LDAPModelAdapter.Instance(Site).testUserName,
                  AD_LDAPModelAdapter.Instance(Site).testUserPwd,
                  AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName));
            con.SessionOptions.Sealing = false;
            con.SessionOptions.Signing = false;
            System.DirectoryServices.Protocols.ModifyRequest mod = new System.DirectoryServices.Protocols.ModifyRequest("",
                 DirectoryAttributeOperation.Add, "schemaupgradeinprogress", "0");
            System.DirectoryServices.Protocols.ModifyResponse response = (System.DirectoryServices.Protocols.ModifyResponse)con.SendRequest(mod);
            BaseTestSite.Assert.AreEqual<ResultCode>(ResultCode.Success, response.ResultCode, "Should return success when set SchemaUpgradeInProgress to 0");
            string serverDN = "cn=testsvr,cn=servers,cn=default-first-site-name,cn=sites,cn=configuration," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC;
            System.DirectoryServices.Protocols.DeleteRequest delete = new DeleteRequest(serverDN);
            delete.Controls.Add(new TreeDeleteControl());
            try
            {
                con.SendRequest(delete);
            }
            catch
            {
            }
            ManagedAddRequest req = new ManagedAddRequest(serverDN, "server");
            System.DirectoryServices.Protocols.AddResponse addRes = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(req);
            BaseTestSite.Assert.AreEqual<ResultCode>(ResultCode.Success, addRes.ResultCode, "should return success for adding item server object {0}", serverDN);
            req = new ManagedAddRequest(
                "cn=ntds settings," + serverDN, "ntdsdsa");
            bool failed = false;
            try
            {
                con.SendRequest(req);
            }
            catch
            {
                failed = true;
            }
            BaseTestSite.Assert.IsTrue(failed, "Should failed when create an nTDSDSA object {0} when schemaupgradeinprogress is 0", "cn=ntds settings," + serverDN);


            mod = new System.DirectoryServices.Protocols.ModifyRequest("",
                 DirectoryAttributeOperation.Add, "schemaupgradeinprogress", "1");
            response = (System.DirectoryServices.Protocols.ModifyResponse)con.SendRequest(mod);
            BaseTestSite.Assert.AreEqual<ResultCode>(ResultCode.Success, response.ResultCode, "Should return success when set SchemaUpgradeInProgress to 1");
            addRes = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(req);
            BaseTestSite.Assert.AreEqual<ResultCode>(ResultCode.Success, addRes.ResultCode, "Should success when create an nTDSDSA object {0} when schemaupgradeinprogress is 1", "cn=ntds settings," + serverDN);
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2012")]
		[TestCategory("ForestWin2012")]
        [TestCategory("Main")]
        public void LDAP_ExtendedOperations_Batch_Request_OID()
        {
            BaseTestSite.Assume.IsTrue(EnvironmentConfig.ServerVer >= ServerVersion.Win2012, "Server OS version should be not less than Windows Server 2012");
            SocketTransportConfig config = new SocketTransportConfig();
            config.RemoteIpAddress = IPAddress.Parse(AD_LDAPModelAdapter.Instance(Site).PDCIPAddress);
            config.RemoteIpPort = int.Parse(AD_LDAPModelAdapter.Instance(Site).ADDSPortNum, CultureInfo.InvariantCulture);
            config.BufferSize = AD_LDAPModelAdapter.Instance(Site).transportBufferSize;
            config.Type = StackTransportType.Tcp;
            config.Role = Role.Client;
            ldapClient = new AdtsLdapClient(AdtsLdapVersion.V3, config);
            ldapClient.Connect();

            #region make a search request
            Filter[] searchFilter = new Filter[1];

            Asn1SetOf<Filter> setOfFilter = new Asn1SetOf<Filter>(searchFilter);
            Filter setFilter = new Filter();
            AttributeDescription attributeDescription = new AttributeDescription("objectclass");
            AssertionValue attributeValue = new AssertionValue("*");
            AttributeValueAssertion attributeValueAssertion = new AttributeValueAssertion(attributeDescription, attributeValue);
            Filter tempFilter = new Filter();
            tempFilter.SetData(Filter.present, attributeDescription);
            //tempFilter.Set_present(attributeDescription);
            searchFilter[0] = tempFilter;


            //setFilter.Set_and(setOfFilter);
            setFilter.SetData(Filter.and, setOfFilter);
            Asn1OctetString[] msg = new Asn1OctetString[2];
            //Com.Objsys.Asn1.Runtime.Asn1OctetString[] msg = new Com.Objsys.Asn1.Runtime.Asn1OctetString[2];

            #endregion

            #region make an add request
            Asn1BerEncodingBuffer buffer = new Asn1BerEncodingBuffer();
            AdtsSearchRequestPacket search = ldapClient.CreateSearchRequest(
                $"cn={Utilities.DomainAdmin},cn=users,{AD_LDAPModelAdapter.Instance(Site).rootDomainNC}",
                0, 0, System.DirectoryServices.Protocols.SearchScope.Base, System.DirectoryServices.Protocols.DereferenceAlias.Never,
                setFilter, false,
                new string[] { "*" });

            search.LdapMessage.BerEncode(buffer);
            msg[0] = new Asn1OctetString(buffer.Data);

            KeyValuePair<string, string[]>[] attrs = new KeyValuePair<string, string[]>[1];
            attrs[0] = new KeyValuePair<string, string[]>("objectclass", new string[] { "user" });

            AdtsAddRequestPacket add = ldapClient.CreateAddRequest(
                   "cn=tmp,cn=users," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC,
                   attrs);
            buffer = new Asn1BerEncodingBuffer();
            add.LdapMessage.BerEncode(buffer);
            msg[1] = new Asn1OctetString(buffer.Data);
            #endregion

            BatchRequestValue batch = new BatchRequestValue(msg);

            buffer = new Asn1BerEncodingBuffer();
            batch.BerEncode(buffer);

            //send extended request
            System.DirectoryServices.Protocols.ExtendedRequest ext = new System.DirectoryServices.Protocols.ExtendedRequest(LDAP_SERVER_BATCH_REQUEST_OID, buffer.Data);
            LdapConnection con = new LdapConnection(AD_LDAPModelAdapter.Instance(Site).PDCIPAddress);
            con.SessionOptions.Sealing = false;
            con.SessionOptions.Signing = false;
            System.DirectoryServices.Protocols.ExtendedResponse response = (System.DirectoryServices.Protocols.ExtendedResponse)con.SendRequest(ext);
            BaseTestSite.Assert.AreEqual<string>(LDAP_SERVER_BATCH_REQUEST_OID, response.ResponseName, "Expect ResponseName in ExtendedResponse is " + LDAP_SERVER_BATCH_REQUEST_OID);
            BaseTestSite.Assert.IsNotNull(response.ResponseValue, "ResponseValue of ExtendedResponse should not be null");
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2012")]
		[TestCategory("ForestWin2012")]
        [TestCategory("Main")]
        public void LDAP_AD_DS_Policies()
        {
            BaseTestSite.Assume.IsTrue(EnvironmentConfig.ServerVer >= ServerVersion.Win2012, "Server OS version should be not less than Windows Server 2012");
            verifyLDAPPolicies_2012(false);
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2012")]
		[TestCategory("ForestWin2012")]
        [TestCategory("LDS")]
        public void LDAP_AD_LDS_Policies()
        {
            BaseTestSite.Assume.IsTrue(EnvironmentConfig.ServerVer >= ServerVersion.Win2012, "Server OS version should be not less than Windows Server 2012");
            verifyLDAPPolicies_2012(true);
        }

        void verifyLDAPPolicies_2012(bool isLDS)
        {
            string addr = isLDS ? (AD_LDAPModelAdapter.Instance(Site).PDCIPAddress + ":" + AD_LDAPModelAdapter.Instance(Site).ADLDSPortNum) : AD_LDAPModelAdapter.Instance(Site).PDCIPAddress;
            LdapConnection con = new LdapConnection(new LdapDirectoryIdentifier(addr),
                new NetworkCredential(AD_LDAPModelAdapter.Instance(Site).testUserName,
                    AD_LDAPModelAdapter.Instance(Site).testUserPwd,
                    AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName));
            con.SessionOptions.Sealing = false;
            con.SessionOptions.Signing = false;
            System.DirectoryServices.Protocols.SearchRequest sr = new System.DirectoryServices.Protocols.SearchRequest(
                "",
                "(objectclass=*)",
                 System.DirectoryServices.Protocols.SearchScope.Base,
                 new string[] { "supportedldappolicies" });
            System.DirectoryServices.Protocols.SearchResponse response = (System.DirectoryServices.Protocols.SearchResponse)con.SendRequest(sr);
            BaseTestSite.Assert.AreEqual<ResultCode>(ResultCode.Success, response.ResultCode, "should return success when query supportedLDAPPolicies");
            DirectoryAttribute attr = response.Entries[0].Attributes["supportedldappolicies"];
            Dictionary<string, bool> toFind = new Dictionary<string, bool>();
            toFind.Add("maxbatchreturnmessages", true);
            for (int i = 0; i < attr.Count; i++)
            {
                string str = attr[i].ToString().ToLower(CultureInfo.InvariantCulture);
                if (toFind.ContainsKey(str))
                {
                    toFind.Remove(str);
                }
            }
            Dictionary<string, bool>.Enumerator enumer = toFind.GetEnumerator();
            while (enumer.MoveNext())
            {
                BaseTestSite.Log.Add(LogEntryKind.CheckFailed, "Failed to find LDAP Policy {0} from rootDSE of {1}", enumer.Current.Key, addr);
            }
            BaseTestSite.Assert.AreEqual<int>(0, toFind.Count, "Failed to find some LDAP Policies, please check previous logs");
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2012")]
		[TestCategory("ForestWin2012")]
        [TestCategory("Main")]
        public void LDAP_AD_DS_MatchingRules_DnWithData()
        {
            BaseTestSite.Assume.IsTrue(EnvironmentConfig.ServerVer >= ServerVersion.Win2012R2, "Server OS version should be not less than Windows Server 2012 R2");
            verifyMatchingRules_DnWithData(false);
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2012")]
		[TestCategory("ForestWin2012")]
        [TestCategory("LDS")]
        public void LDAP_AD_LDS_MatchingRules_DnWithData()
        {
            BaseTestSite.Assume.IsTrue(EnvironmentConfig.ServerVer >= ServerVersion.Win2012R2, "Server OS version should be not less than Windows Server 2012 R2");
            verifyMatchingRules_DnWithData(true);
        }

        void verifyMatchingRules_DnWithData(bool isLDS)
        {
            #region Connect

            string addr = AD_LDAPModelAdapter.Instance(Site).PDCIPAddress;
            string port = isLDS ? AD_LDAPModelAdapter.Instance(Site).ADLDSPortNum : AD_LDAPModelAdapter.Instance(Site).ADDSPortNum;
            LdapConnection con = new LdapConnection(
                new LdapDirectoryIdentifier(addr, int.Parse(port)),
                new NetworkCredential(AD_LDAPModelAdapter.Instance(Site).testUserName,
                    AD_LDAPModelAdapter.Instance(Site).testUserPwd,
                    AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName));
            con.SessionOptions.Sealing = false;
            con.SessionOptions.Signing = false;

            #endregion

            #region Make a search request with LDAP_MATCHING_RULE_DN_WITH_DATA matching rule specified

            System.DirectoryServices.Protocols.SearchRequest req = new System.DirectoryServices.Protocols.SearchRequest(
                null,
                "(objectClass=*)",
                System.DirectoryServices.Protocols.SearchScope.Base,
                new string[] { "*" });
            System.DirectoryServices.Protocols.SearchResponse resp = (System.DirectoryServices.Protocols.SearchResponse)con.SendRequest(req);
            string rootDN;
            if (isLDS)
            {
                rootDN = resp.Entries[0].Attributes[RootDSEAttribute.namingContexts][2].ToString();
            }
            else
            {
                rootDN = resp.Entries[0].Attributes[RootDSEAttribute.rootDomainNamingContext][0].ToString();
            }

            string container = isLDS ? "CN=Roles," + rootDN : "CN=Users," + rootDN;
            string filterVal = "B:32:A9D1CA15768811D1ADED00C04FD8D5CD:" + container;
            req = new System.DirectoryServices.Protocols.SearchRequest(
                rootDN,
                // Attribute wellKnownObjects is of syntax Object(DN-Binary)
                // LDAP_MATCHING_RULE_DN_WITH_DATA has OID 1.2.840.113556.1.4.2253
                // User container has GUID A9D1CA15768811D1ADED00C04FD8D5CD
                "(wellKnownObjects:1.2.840.113556.1.4.2253:=" + filterVal + ")",
                System.DirectoryServices.Protocols.SearchScope.Subtree,
                new string[] { "wellKnownObjects" }
                );

            resp = (System.DirectoryServices.Protocols.SearchResponse)con.SendRequest(req);
            BaseTestSite.Assert.AreEqual(ResultCode.Success, resp.ResultCode, "The LDAP operation should succeed.");

            #endregion

            #region verify search result

            BaseTestSite.Assert.IsTrue(resp.Entries.Count > 0, "The LDAP search should return more than 1 entries.");
            BaseTestSite.Assert.IsNotNull(resp.Entries[0].Attributes["wellKnownObjects"],
                @"This rule provides a way to match on portions of values of syntax Object(DN-String) and Object(DN-Binary).");

            #endregion

            #region Negative

            // Generate a wrong GUID, which can never be found in the database
            filterVal = "B:32:A9D1CA15768811D1ADED00C04FD8D5C1:" + container;
            req.Filter = filterVal;
            resp = (System.DirectoryServices.Protocols.SearchResponse)con.SendRequest(req);
            BaseTestSite.Assert.AreEqual(ResultCode.Success, resp.ResultCode, "The LDAP operation should succeed.");
            BaseTestSite.Assert.IsTrue(resp.Entries.Count == 0, "The LDAP search should return no entry.");

            #endregion
        }
    }
}
