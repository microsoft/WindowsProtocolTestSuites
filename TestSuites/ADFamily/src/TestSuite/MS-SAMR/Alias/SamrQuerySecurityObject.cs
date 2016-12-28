// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.Security.Samr;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Samr
{
    public partial class SAMR_TestSuite : TestClassBase
    {
        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("Alias")]
        [Description("This is to test SamrQuerySecurityObject with OWNER_SECURITY_INFORMATION for a group object.")]
        public void SamrQuerySecurityObjectForAlias_OWNER_SECURITY_INFORMATION()
        {
            aliasTestPrerequisite();
            createAlias();

            Site.Log.Add(LogEntryKind.TestStep, "SamrQuerySecurityObject with OWNER_SECURITY_INFORMATION");
            _SAMPR_SR_SECURITY_DESCRIPTOR? securityDescriptor;

            HRESULT result = _samrProtocolAdapter.SamrQuerySecurityObject(_aliasHandle, SecurityInformation.OWNER_SECURITY_INFORMATION, out securityDescriptor);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQuerySecurityObject returns:{0}.", result);
            Site.Assert.IsNotNull(securityDescriptor, "The returned securityDescriptor should not be null.");
            _SECURITY_DESCRIPTOR sd = DtypUtility.DecodeSecurityDescriptor(securityDescriptor.Value.SecurityDescriptor);
            Site.Assert.IsNotNull(sd.OwnerSid, "3.1.5.12.2 If this bit(OWNER_SECURITY_INFORMATION) is set, the client requests that the Owner member be returned.");
            Site.Assert.IsNull(sd.GroupSid, "3.1.5.12.2 If this bit(GROUP_SECURITY_INFORMATION) is not set, the client requests that the Group member not be returned.");
            Site.Assert.IsNull(sd.Dacl, "3.1.5.12.2 If this bit(DACL_SECURITY_INFORMATION) is not set, the client requests that the DACL not be returned.");
            Site.Assert.IsNull(sd.Sacl, "3.1.5.12.2 If this bit(SACL_SECURITY_INFORMATION) is not set, the client requests that the SACL not be returned.");
            Site.Assert.AreEqual(AdministratorSid, DtypUtility.ToSddlString((_SID)sd.OwnerSid), "3.1.5.12.2 The Owner and Group fields of the security descriptor MUST be the administrator's SID (S-1-5-32-544).");
        }
    }

}
