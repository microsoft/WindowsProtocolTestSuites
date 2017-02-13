// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.DirectoryServices.Protocols;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
using Microsoft.Protocols.TestTools.StackSdk.Security.Samr;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Modeling;
using System.Collections.Generic;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Samr
{
    public partial class SAMR_TestSuite : TestClassBase
    {
        void aliasTestPrerequisite()
        {
            Site.Log.Add(LogEntryKind.TestStep, "SamrBind-->SamrConnect5-->SamrOpenDomain");
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);
            try
            {
                AdLdapClient.Instance().ConnectAndBind(_samrProtocolAdapter.pdcNetBIOSName, System.Net.IPAddress.Parse(_samrProtocolAdapter.PDCIPAddress),
                    389,
                    _samrProtocolAdapter.DomainAdministratorName,
                    _samrProtocolAdapter.DomainUserPassword,
                    _samrProtocolAdapter.primaryDomainFqdn, AuthType.Kerberos);
                string result = AdLdapClient.Instance().DeleteObject("cn=" + testAliasName + "," + _samrProtocolAdapter.primaryDomainUserContainerDN, null);
            }
            catch
            { }
        }

        void createAlias()
        {
            Site.Log.Add(LogEntryKind.TestStep, "SamrCreateAliasInDomain: create an alias with Name:{0}, and DesiredAccess:{1}",
               testAliasName, Common_ACCESS_MASK.MAXIMUM_ALLOWED);

            uint relativeId;
            HRESULT result = _samrProtocolAdapter.SamrCreateAliasInDomain(
                _domainHandle,
                testAliasName,
                (uint)Group_ACCESS_MASK.GROUP_ALL_ACCESS,
                out _aliasHandle,
                out relativeId);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateAliasInDomain returns:{0}.", result);
            Site.Assert.IsNotNull(_userHandle, "The returned user handle is: {0}.", _aliasHandle);
            Site.Assert.IsTrue(_samrProtocolAdapter.VerifyRelativeID(relativeId), "The Rid value MUST be within the Rid-Range");
        }

        void deleteAlias()
        {
            Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteAlias: delete the created group.");
            HRESULT result = _samrProtocolAdapter.SamrDeleteAlias(ref _aliasHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteAlias returns:{0}.", result);
        }

        /// <summary>
        /// Test method of SamrCreateGroupInDomain and SamrDeleteGroup
        /// </summary>
        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("Alias")]
        public void SamrCreateAliasInDomainAndDelete()
        {
            aliasTestPrerequisite();
            createAlias();
            deleteAlias();
        }
    }
}
