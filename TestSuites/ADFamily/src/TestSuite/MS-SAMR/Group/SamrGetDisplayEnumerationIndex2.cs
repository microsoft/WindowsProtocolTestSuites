// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Security.Samr;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Net;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Samr
{
    public partial class SAMR_TestSuite : TestClassBase
    {
        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrGetDisplayEnumerationIndex2 with DomainDisplayGroup.")]
        public void SamrGetDisplayEnumerationIndex2_Group()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrGetDisplayEnumerationIndex2: obtains an index into an ascending account-name¨Csorted list of accounts.");
            uint index;
            string prefix = "Domain";
            HRESULT result = _samrProtocolAdapter.SamrGetDisplayEnumerationIndex2(_domainHandle, _DOMAIN_DISPLAY_INFORMATION.DomainDisplayGroup, prefix, out index);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrGetDisplayEnumerationIndex2 returns:{0}.", result);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrGetDisplayEnumerationIndex2 with DomainHandle.HandleType not equal to Domain.")]
        public void SamrGetDisplayEnumerationIndex2_Group_InvalidHandle()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrGetDisplayEnumerationIndex2 with invalid handle.");
            uint index;
            string prefix = "Domain";
            HRESULT result = _samrProtocolAdapter.SamrGetDisplayEnumerationIndex2(_serverHandle, _DOMAIN_DISPLAY_INFORMATION.DomainDisplayGroup, prefix, out index);
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "3.1.5.3.4 The server MUST return an error if DomainHandle.HandleType is not equal to Domain.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrGetDisplayEnumerationIndex2 with no DOMAIN_LIST_ACCOUNTS access.")]
        public void SamrGetDisplayEnumerationIndex2_Group_STATUS_ACCESS_DENIED()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle, Utilities.DOMAIN_READ);

            Site.Log.Add(LogEntryKind.TestStep, "SamrGetDisplayEnumerationIndex2 with no DOMAIN_LIST_ACCOUNTS access.");
            uint index;
            string prefix = "Domain";
            HRESULT result = _samrProtocolAdapter.SamrGetDisplayEnumerationIndex2(_domainHandle, _DOMAIN_DISPLAY_INFORMATION.DomainDisplayGroup, prefix, out index);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "3.1.5.3.4 DomainHandle.GrantedAccess MUST have the required access specified in section 3.1.2.1. Otherwise, the server MUST return STATUS_ACCESS_DENIED.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrGetDisplayEnumerationIndex2 with invalid DisplayInformationClass.")]
        public void SamrGetDisplayEnumerationIndex2_Group_InvalidDisplayInformationClass()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrGetDisplayEnumerationIndex2 with invalid DisplayInformationClass.");
            uint index;
            string prefix = "Domain";
            HRESULT result = _samrProtocolAdapter.SamrGetDisplayEnumerationIndex2(_domainHandle, _DOMAIN_DISPLAY_INFORMATION.DomainDisplayOemGroup, prefix, out index);
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "3.1.5.3.4 If DisplayInformationClass is not one of the following values, the server MUST return an error code: DomainDisplayUser, DomainDisplayMachine, DomainDisplayGroup.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrGetDisplayEnumerationIndex2 of STATUS_NO_MORE_ENTRIES.")]
        public void SamrGetDisplayEnumerationIndex2_Group_STATUS_NO_MORE_ENTRIES()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrGetDisplayEnumerationIndex2: obtains an index into an ascending account-name¨Csorted list of accounts.");
            uint index;
            string prefix = "X";
            HRESULT result = _samrProtocolAdapter.SamrGetDisplayEnumerationIndex2(_domainHandle, _DOMAIN_DISPLAY_INFORMATION.DomainDisplayGroup, prefix, out index);

            Site.Log.Add(LogEntryKind.TestStep, "Check whether there is such group with prefix {0} existing in the database.", prefix);
            LdapConnection con = new LdapConnection(new LdapDirectoryIdentifier(_samrProtocolAdapter.pdcFqdn),
                new NetworkCredential(_samrProtocolAdapter.DomainAdministratorName, _samrProtocolAdapter.DomainUserPassword, _samrProtocolAdapter.PrimaryDomainDnsName));
            string filter = string.Format("(&(objectclass=group)(sAMAccountName={0}*))", prefix);
            SearchRequest request = new SearchRequest(_samrProtocolAdapter.primaryDomainUserContainerDN,
                filter, System.DirectoryServices.Protocols.SearchScope.Subtree, "sAMAccountName");
            SearchResponse response = (SearchResponse)con.SendRequest(request);
            if (response.Entries.Count > 0)
            {
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "STATUS_SUCCESS is returned if there is such group.");
            }
            else
            {
                Site.Assert.AreEqual(HRESULT.STATUS_NO_MORE_ENTRIES, result, "3.1.5.3.4 If no such element exists, the server MUST return STATUS_NO_MORE_ENTRIES.");
            }
        }
    }
}
