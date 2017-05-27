// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.Security.Samr;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.DirectoryServices;
using System.Security.Principal;

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
        [Description("This is to test SamrRidToSid with right Rid.")]
        public void SamrRidToSid_Group()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName,
                out _serverHandle, out _domainHandle);
            LookupAndOpenGroup(_domainHandle, _samrProtocolAdapter.DomainAdminGroup, out _groupHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrRidToSid: obtain the SID of an account, given a RID.");
            string groupPath = string.Format("LDAP://{0}/CN={1},{2}",
                _samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.DomainAdminGroup, _samrProtocolAdapter.primaryDomainUserContainerDN);
            DirectoryEntry entry = new DirectoryEntry(groupPath);
            SecurityIdentifier objSid = new SecurityIdentifier((byte[])entry.Properties["objectSid"].Value, 0);
            string objectSid = objSid.ToString();
            string[] sidArray = objectSid.Split('-');
            uint rid = Convert.ToUInt32(sidArray[sidArray.Length - 1]);
            _RPC_SID? sid;
            _samrProtocolAdapter.SamrRidToSid(_groupHandle, rid, out sid);
            byte[] ia = sid.Value.IdentifierAuthority.Value;
            Site.Assert.AreEqual(sidArray[2], ia[ia.Length - 1].ToString(), "The IdentifierAuthority in Sid should be right.");
            Site.Assert.AreEqual(sidArray.Length - 3, (int)sid.Value.SubAuthorityCount, "The SubAuthority count should be right.");
            for (int i = 0; i < 5; i++)
            {
                Site.Assert.AreEqual(sidArray[i + 3], sid.Value.SubAuthority[i].ToString(), "The SubAuthority{0} should be right.", i);
            }

        }

    }
}
