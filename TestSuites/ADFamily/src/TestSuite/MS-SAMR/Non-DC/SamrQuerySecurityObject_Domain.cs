// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.Security.Samr;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Samr
{
    public partial class SAMR_TestSuite : TestClassBase
    {
        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Domain")]
        [Description("Non-DC Test: This is to test SamrQuerySecurityObject with SACL_SECURITY_INFORMATION for DM domain.")]
        public void SamrQuerySecurityObject_SACL_SECURITY_INFORMATION_Domain_NonDC()
        {
            HRESULT hResult;

            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
                out _serverHandle, out _domainHandle, (uint)Common_ACCESS_MASK.ACCESS_SYSTEM_SECURITY);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQuerySecurityObject, SecurityInformation: SACL_SECURITY_INFORMATION.");
            _SAMPR_SR_SECURITY_DESCRIPTOR? securityDescriptor;
            hResult = _samrProtocolAdapter.SamrQuerySecurityObject(_domainHandle, SecurityInformation.SACL_SECURITY_INFORMATION, out securityDescriptor);

            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrQuerySecurityObject must return STATUS_SUCCESS.");
            Site.Assert.IsNotNull(securityDescriptor, "The SecurityDescriptor returned by SamrQuerySecurityObject is not null.");

            _SECURITY_DESCRIPTOR sd = DtypUtility.DecodeSecurityDescriptor(securityDescriptor.Value.SecurityDescriptor);
            Site.Assert.IsNull(sd.OwnerSid, "3.1.5.12.2 If this bit(OWNER_SECURITY_INFORMATION) is not set, the client requests that the Owner member not be returned.");
            Site.Assert.IsNull(sd.GroupSid, "3.1.5.12.2 If this bit(GROUP_SECURITY_INFORMATION) is not set, the client requests that the Group member not be returned.");
            Site.Assert.IsNull(sd.Dacl, "3.1.5.12.2 If this bit(DACL_SECURITY_INFORMATION) is not set, the client requests that the DACL not be returned.");
            Site.Assert.IsNotNull(sd.Sacl, "3.1.5.12.2 If this bit(SACL_SECURITY_INFORMATION) is set, the client requests that the SACL be returned.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Domain")]
        [Description("Non-DC Test: This is to test SamrQuerySecurityObject with OWNER_SECURITY_INFORMATION for DM domain.")]
        public void SamrQuerySecurityObject_OWNER_SECURITY_INFORMATION_Domain_NonDC()
        {
            HRESULT hResult;

            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
                out _serverHandle, out _domainHandle, (uint)Common_ACCESS_MASK.READ_CONTROL);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQuerySecurityObject, SecurityInformation: OWNER_SECURITY_INFORMATION.");
            _SAMPR_SR_SECURITY_DESCRIPTOR? securityDescriptor;
            hResult = _samrProtocolAdapter.SamrQuerySecurityObject(_domainHandle, SecurityInformation.OWNER_SECURITY_INFORMATION, out securityDescriptor);

            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrQuerySecurityObject must return STATUS_SUCCESS.");
            Site.Assert.IsNotNull(securityDescriptor, "The SecurityDescriptor returned by SamrQuerySecurityObject is not null.");

            _SECURITY_DESCRIPTOR sd = DtypUtility.DecodeSecurityDescriptor(securityDescriptor.Value.SecurityDescriptor);
            Site.Assert.IsNotNull(sd.OwnerSid, "3.1.5.12.2 If this bit(OWNER_SECURITY_INFORMATION) is set, the client requests that the Owner member be returned.");
            Site.Assert.IsNull(sd.GroupSid, "3.1.5.12.2 If this bit(GROUP_SECURITY_INFORMATION) is not set, the client requests that the Group member not be returned.");
            Site.Assert.IsNull(sd.Dacl, "3.1.5.12.2 If this bit(DACL_SECURITY_INFORMATION) is not set, the client requests that the DACL not be returned.");
            Site.Assert.IsNull(sd.Sacl, "3.1.5.12.2 If this bit(SACL_SECURITY_INFORMATION) is not set, the client requests that the SACL not be returned.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Domain")]
        [Description("Non_DC Test: This is to test SamrQuerySecurityObject with OWNER_SECURITY_INFORMATION for DM domain.")]
        public void SamrQuerySecurityObject_GROUP_SECURITY_INFORMATION_Domain_NonDC()
        {
            HRESULT hResult;

            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
                out _serverHandle, out _domainHandle, (uint)Common_ACCESS_MASK.READ_CONTROL);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQuerySecurityObject, SecurityInformation: GROUP_SECURITY_INFORMATION.");
            _SAMPR_SR_SECURITY_DESCRIPTOR? securityDescriptor;
            hResult = _samrProtocolAdapter.SamrQuerySecurityObject(_domainHandle, SecurityInformation.GROUP_SECURITY_INFORMATION, out securityDescriptor);

            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrQuerySecurityObject must return STATUS_SUCCESS.");
            Site.Assert.IsNotNull(securityDescriptor, "The SecurityDescriptor returned by SamrQuerySecurityObject is not null.");

            _SECURITY_DESCRIPTOR sd = DtypUtility.DecodeSecurityDescriptor(securityDescriptor.Value.SecurityDescriptor);
            Site.Assert.IsNull(sd.OwnerSid, "3.1.5.12.2 If this bit(OWNER_SECURITY_INFORMATION) is not set, the client requests that the Owner member not be returned.");
            Site.Assert.IsNotNull(sd.GroupSid, "3.1.5.12.2 If this bit(GROUP_SECURITY_INFORMATION) is set, the client requests that the Group member be returned.");
            Site.Assert.IsNull(sd.Dacl, "3.1.5.12.2 If this bit(DACL_SECURITY_INFORMATION) is not set, the client requests that the DACL not be returned.");
            Site.Assert.IsNull(sd.Sacl, "3.1.5.12.2 If this bit(SACL_SECURITY_INFORMATION) is not set, the client requests that the SACL not be returned.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Domain")]
        [Description("Non-DC Test: This is to test SamrQuerySecurityObject with DACL_SECURITY_INFORMATION for DM domain.")]
        public void SamrQuerySecurityObject_DACL_SECURITY_INFORMATION_Domain_NonDC()
        {
            HRESULT hResult;

            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
                out _serverHandle, out _domainHandle, (uint)Common_ACCESS_MASK.READ_CONTROL);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQuerySecurityObject, SecurityInformation: DACL_SECURITY_INFORMATION.");
            _SAMPR_SR_SECURITY_DESCRIPTOR? securityDescriptor;
            hResult = _samrProtocolAdapter.SamrQuerySecurityObject(_domainHandle, SecurityInformation.DACL_SECURITY_INFORMATION, out securityDescriptor);

            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrQuerySecurityObject must return STATUS_SUCCESS.");
            Site.Assert.IsNotNull(securityDescriptor, "The SecurityDescriptor returned by SamrQuerySecurityObject is not null.");

            _SECURITY_DESCRIPTOR sd = DtypUtility.DecodeSecurityDescriptor(securityDescriptor.Value.SecurityDescriptor);
            Site.Assert.IsNull(sd.OwnerSid, "3.1.5.12.2 If this bit(OWNER_SECURITY_INFORMATION) is not set, the client requests that the Owner member not be returned.");
            Site.Assert.IsNull(sd.GroupSid, "3.1.5.12.2 If this bit(GROUP_SECURITY_INFORMATION) is not set, the client requests that the Group member not be returned.");
            Site.Assert.IsNotNull(sd.Dacl, "3.1.5.12.2 If this bit(DACL_SECURITY_INFORMATION) is set, the client requests that the DACL be returned.");
            Site.Assert.IsNull(sd.Sacl, "3.1.5.12.2 If this bit(SACL_SECURITY_INFORMATION) is not set, the client requests that the SACL not be returned.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Domain")]
        [Description("Non-DC Test: This is to test SamrQuerySecurityObject with DACL_SECURITY_INFORMATION and GROUP_SECURITY_INFORMATION for DM domain.")]
        public void SamrQuerySecurityObject_Group_DACL_SECURITY_INFORMATION_Domain_NonDC()
        {
            HRESULT hResult;

            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
                out _serverHandle, out _domainHandle, (uint)Common_ACCESS_MASK.READ_CONTROL);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQuerySecurityObject, SecurityInformation: GROUP_SECURITY_INFORMATION | DACL_SECURITY_INFORMATION.");
            _SAMPR_SR_SECURITY_DESCRIPTOR? securityDescriptor;
            hResult = _samrProtocolAdapter.SamrQuerySecurityObject(_domainHandle, SecurityInformation.GROUP_SECURITY_INFORMATION | SecurityInformation.DACL_SECURITY_INFORMATION, out securityDescriptor);

            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrQuerySecurityObject must return STATUS_SUCCESS.");
            Site.Assert.IsNotNull(securityDescriptor, "The SecurityDescriptor returned by SamrQuerySecurityObject is not null.");

            _SECURITY_DESCRIPTOR sd = DtypUtility.DecodeSecurityDescriptor(securityDescriptor.Value.SecurityDescriptor);
            Site.Assert.IsNull(sd.OwnerSid, "3.1.5.12.2 If this bit(OWNER_SECURITY_INFORMATION) is not set, the client requests that the Owner member not be returned.");
            Site.Assert.IsNotNull(sd.GroupSid, "3.1.5.12.2 If this bit(GROUP_SECURITY_INFORMATION) is set, the client requests that the Group member be returned.");
            Site.Assert.IsNotNull(sd.Dacl, "3.1.5.12.2 If this bit(DACL_SECURITY_INFORMATION) is set, the client requests that the DACL be returned.");
            Site.Assert.IsNull(sd.Sacl, "3.1.5.12.2 If this bit(SACL_SECURITY_INFORMATION) is not set, the client requests that the SACL not be returned.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Domain")]
        [Description("Non-DC Test: This is to test SamrQuerySecurityObject with SACL_SECURITY_INFORMATION but no ACCESS_SYSTEM_SECURITY access for DM domain.")]
        public void SamrQuerySecurityObject_SACL_SECURITY_INFORMATION_STATUS_ACCESS_DENIED_Domain_NonDC()
        {
            HRESULT hResult;

            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
                out _serverHandle, out _domainHandle, (uint)Common_ACCESS_MASK.READ_CONTROL);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQuerySecurityObject, SecurityInformation: SACL_SECURITY_INFORMATION with no ACCESS_SYSTEM_SECURITY access.");
            _SAMPR_SR_SECURITY_DESCRIPTOR? securityDescriptor;
            hResult = _samrProtocolAdapter.SamrQuerySecurityObject(_domainHandle, SecurityInformation.SACL_SECURITY_INFORMATION, out securityDescriptor);

            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, hResult, "On error, the server MUST abort processing and return STATUS_ACCESS_DENIED.");
            Site.Assert.IsNull(securityDescriptor, "The SecurityDescriptor returned by SamrQuerySecurityObject is null.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Domain")]
        [Description("Non-DC Test: This is to test SamrQuerySecurityObject with OWNER_SECURITY_INFORMATION but no READ_CONTROL access for DM domain.")]
        public void SamrQuerySecurityObject_OWNER_SECURITY_INFORMATION_STATUS_ACCESS_DENIED_Domain_NonDC()
        {
            HRESULT hResult;

            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
                out _serverHandle, out _domainHandle, (uint)Common_ACCESS_MASK.DELETE);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQuerySecurityObject, SecurityInformation: OWNER_SECURITY_INFORMATION with no READ_CONTROL access.");
            _SAMPR_SR_SECURITY_DESCRIPTOR? securityDescriptor;
            hResult = _samrProtocolAdapter.SamrQuerySecurityObject(_domainHandle, SecurityInformation.OWNER_SECURITY_INFORMATION, out securityDescriptor);

            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, hResult, "On error, the server MUST abort processing and return STATUS_ACCESS_DENIED.");
            Site.Assert.IsNull(securityDescriptor, "The SecurityDescriptor returned by SamrQuerySecurityObject is null.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Domain")]
        [Description("Non-DC Test: This is to test SamrQuerySecurityObject with INVALID_SECURITY_INFORMATION for DM domain.")]
        public void SamrQuerySecurityObject_INVALID_SECURITY_INFORMATION_Domain_NonDC()
        {
            HRESULT hResult;

            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
                out _serverHandle, out _domainHandle, (uint)Common_ACCESS_MASK.READ_CONTROL);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQuerySecurityObject, SecurityInformation: INVALID_SECURITY_INFORMATION.");
            _SAMPR_SR_SECURITY_DESCRIPTOR? securityDescriptor;
            hResult = _samrProtocolAdapter.SamrQuerySecurityObject(_domainHandle, SecurityInformation.INVALID_SECURITY_INFORMATION, out securityDescriptor);

            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "If there is not any valid bit in the SECURITY_INFORMATION, all other bits MUST be zero when sent and ignored on receipt.");

            _SECURITY_DESCRIPTOR sd = DtypUtility.DecodeSecurityDescriptor(securityDescriptor.Value.SecurityDescriptor);
            Site.Assert.IsNull(sd.OwnerSid, "3.1.5.12.2 If this bit(OWNER_SECURITY_INFORMATION) is not set, the client requests that the Owner member not be returned.");
            Site.Assert.IsNull(sd.GroupSid, "3.1.5.12.2 If this bit(GROUP_SECURITY_INFORMATION) is not set, the client requests that the Group member not be returned.");
            Site.Assert.IsNull(sd.Dacl, "3.1.5.12.2 If this bit(DACL_SECURITY_INFORMATION) is not set, the client requests that the DACL not be returned.");
            Site.Assert.IsNull(sd.Sacl, "3.1.5.12.2 If this bit(SACL_SECURITY_INFORMATION) is not set, the client requests that the SACL not be returned.");
        }
    }
}
