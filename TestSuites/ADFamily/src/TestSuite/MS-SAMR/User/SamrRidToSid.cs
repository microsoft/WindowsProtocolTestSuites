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
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrRidToSid with right Rid.")]
        public void SamrRidToSid()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Connect and open a user handle.");
            ConnectAndOpenUser(_samrProtocolAdapter.PDCNetbiosName, _samrProtocolAdapter.PrimaryDomainDnsName,
                _samrProtocolAdapter.DomainAdministratorName, out _userHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrRidToSid: obtain the SID of an account, given a RID.");
            string userPath = string.Format("LDAP://{0}/CN={1},{2}",
    _samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.DomainAdministratorName, _samrProtocolAdapter.primaryDomainUserContainerDN);
            DirectoryEntry entry = new DirectoryEntry(userPath);
            SecurityIdentifier objSid = new SecurityIdentifier((byte[])entry.Properties["objectSid"].Value, 0);
            string objectSid = objSid.ToString();
            string[] sidArray = objectSid.Split('-');
            uint rid = Convert.ToUInt32(sidArray[sidArray.Length - 1]);
            _RPC_SID? sid;
            HRESULT result = _samrProtocolAdapter.SamrRidToSid(_userHandle,rid,out sid);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrRidToSid succeeded.");
            byte[] ia = sid.Value.IdentifierAuthority.Value;
            Site.Assert.AreEqual(sidArray[2], ia[ia.Length - 1].ToString(), "The IdentifierAuthority in Sid should be right.");
            Site.Assert.AreEqual(sidArray.Length - 3, (int)sid.Value.SubAuthorityCount, "The SubAuthority count should be right.");
            for (int i = 0; i < sid.Value.SubAuthority.Length; i++)
            {
                Site.Assert.AreEqual(sidArray[i+3],sid.Value.SubAuthority[i].ToString(),"The SubAuthority{0} should be right.", i);
            }
        }

    }
}
