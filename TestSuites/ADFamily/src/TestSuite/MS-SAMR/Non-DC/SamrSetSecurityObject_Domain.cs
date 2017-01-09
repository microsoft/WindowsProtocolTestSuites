// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Security.AccessControl;
using System.Security.Principal;
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
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Domain")]
        [Description("Non-DC Test: This is to test SamrSetSecurityObject with INVALID_SECURITY_INFORMATION for DM domain.")]
        public void SamrSetSecurityObject_INVALID_SECURITY_INFORMATION_Domain_NonDC()
        {
            HRESULT hResult;

            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
                out _serverHandle, out _domainHandle, (uint)Common_ACCESS_MASK.WRITE_OWNER);

           
            CommonSecurityDescriptor commonsd = new CommonSecurityDescriptor(false, true, "O:BAG:BA"); 
            byte[] buffer = new byte[commonsd.BinaryLength];
            commonsd.GetBinaryForm(buffer, 0);
            _SAMPR_SR_SECURITY_DESCRIPTOR securityDescriptor = new _SAMPR_SR_SECURITY_DESCRIPTOR()
            {
                SecurityDescriptor = buffer,
                Length = (uint)buffer.Length
            };
            Site.Log.Add(LogEntryKind.TestStep,
                "SamrSetSecurityObject, SecurityInformation: INVALID_SECURITY_INFORMATION.");
            hResult = _samrProtocolAdapter.SamrSetSecurityObject(_domainHandle, SecurityInformation.INVALID_SECURITY_INFORMATION, securityDescriptor);
            Site.Assert.AreEqual(Utilities.STATUS_INVALID_PARAMETER, (uint)hResult, "If the SECURITY_INFORMATION type is not valid, the server MUST return STATUS_INVALID_PARAMETER.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Domain")]
        [Description("Non-DC Test: This is to test SamrSetSecurityObject with DACL_SECURITY_INFORMATION but no WRITE_DAC for DM domain.")]
        public void SamrSetSecurityObject_DACL_SECURITY_INFORMATION_STATUS_ACCESS_DENIED_Domain_NonDC()
        {
            HRESULT hResult;

            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
                out _serverHandle, out _domainHandle, (uint)Common_ACCESS_MASK.WRITE_OWNER);

            CommonSecurityDescriptor commonsd = new CommonSecurityDescriptor(false, true, "O:BAG:BA");
            byte[] buffer = new byte[commonsd.BinaryLength];
            commonsd.GetBinaryForm(buffer, 0);
            _SAMPR_SR_SECURITY_DESCRIPTOR securityDescriptor = new _SAMPR_SR_SECURITY_DESCRIPTOR()
            {
                SecurityDescriptor = buffer,
                Length = (uint)buffer.Length
            };
            Site.Log.Add(LogEntryKind.TestStep,
                "SamrSetSecurityObject, SecurityInformation: DACL_SECURITY_INFORMATION with no WRITE_DAC access.");
            hResult = _samrProtocolAdapter.SamrSetSecurityObject(_domainHandle, SecurityInformation.DACL_SECURITY_INFORMATION, securityDescriptor);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, hResult, "On error, the server MUST abort processing and return STATUS_ACCESS_DENIED.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Domain")]
        [Description("Non-DC Test: This is to test SamrSetSecurityObject with OWNER_SECURITY_INFORMATION for DM domain.")]
        public void SamrSetSecurityObject_Owner_SECURITY_INFORMATION_Domain_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
                out _serverHandle, out _domainHandle, Utilities.WRITE_OWNER | Utilities.DOMAIN_READ);

            try
            {
                Site.Log.Add(LogEntryKind.TestStep,
                    "Set the owner of DM domain to be Built-in Guest.");
                CommonSecurityDescriptor commonsd = new CommonSecurityDescriptor(false, true, "O:BGG:BA");
                byte[] buffer = new byte[commonsd.BinaryLength];
                commonsd.GetBinaryForm(buffer, 0);
                _SAMPR_SR_SECURITY_DESCRIPTOR securityDescriptor = new _SAMPR_SR_SECURITY_DESCRIPTOR()
                {
                    SecurityDescriptor = buffer,
                    Length = (uint)buffer.Length
                };

                _SAMPR_SR_SECURITY_DESCRIPTOR? sd;
                Site.Log.Add(LogEntryKind.TestStep,
                    "SamrSetSecurityObject, SecurityInformation: OWNER_SECURITY_INFORMATION. ");
                HRESULT hResult = _samrProtocolAdapter.SamrSetSecurityObject(_domainHandle, SecurityInformation.OWNER_SECURITY_INFORMATION, securityDescriptor);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrSetSecurityObject must return STATUS_SUCCESS.");

                Site.Log.Add(LogEntryKind.TestStep, "SamrQuerySecurityObject to check again, SecurityInformation: OWNER_SECURITY_INFORMATION.");
                hResult = _samrProtocolAdapter.SamrQuerySecurityObject(_domainHandle, SecurityInformation.OWNER_SECURITY_INFORMATION, out sd);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrQuerySecurityObject must return STATUS_SUCCESS.");
                Site.Assert.IsNotNull(sd, "The SecurityDescriptor returned by SamrQuerySecurityObject is not null.");
                CommonSecurityDescriptor ObtainedSecurityDescriptor = new CommonSecurityDescriptor(false, true, sd.Value.SecurityDescriptor, 0);
                Site.Assert.AreEqual("O:BG", ObtainedSecurityDescriptor.GetSddlForm(AccessControlSections.Owner), "Owner should be Built-in Guest.");

                _SECURITY_DESCRIPTOR secdes = DtypUtility.DecodeSecurityDescriptor(sd.Value.SecurityDescriptor);
                Site.Assert.IsNotNull(secdes.OwnerSid, "3.1.5.12.2 If this bit(OWNER_SECURITY_INFORMATION) is set, the client requests that the Owner member be returned.");
                Site.Assert.IsNull(secdes.GroupSid, "3.1.5.12.2 If this bit(GROUP_SECURITY_INFORMATION) is not set, the client requests that the Group member not be returned.");
                Site.Assert.IsNull(secdes.Dacl, "3.1.5.12.2 If this bit(DACL_SECURITY_INFORMATION) is not set, the client requests that the DACL not be returned.");
                Site.Assert.IsNull(secdes.Sacl, "3.1.5.12.2 If this bit(SACL_SECURITY_INFORMATION) is not set, the client requests that the SACL not be returned.");
            }
            finally
            {
                Site.Log.Add(LogEntryKind.TestStep, "Change the owner of DM domain back to Built-in Administrator.");
                CommonSecurityDescriptor commonsd = new CommonSecurityDescriptor(false, true, "O:BAG:BA");
                byte[] buffer = new byte[commonsd.BinaryLength];
                commonsd.GetBinaryForm(buffer, 0);
                _SAMPR_SR_SECURITY_DESCRIPTOR securityDescriptor = new _SAMPR_SR_SECURITY_DESCRIPTOR()
                {
                    SecurityDescriptor = buffer,
                    Length = (uint)buffer.Length
                };
                Site.Log.Add(LogEntryKind.TestStep,
                    "SamrSetSecurityObject, SecurityInformation: OWNER_SECURITY_INFORMATION. ");
                HRESULT hResult = _samrProtocolAdapter.SamrSetSecurityObject(_domainHandle, SecurityInformation.OWNER_SECURITY_INFORMATION, securityDescriptor);

                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrSetSecurityObject must return STATUS_SUCCESS.");
                Site.Log.Add(LogEntryKind.TestStep, "SamrQuerySecurityObject to check again, SecurityInformation: OWNER_SECURITY_INFORMATION.");
                _SAMPR_SR_SECURITY_DESCRIPTOR? sd;
                hResult = _samrProtocolAdapter.SamrQuerySecurityObject(_domainHandle, SecurityInformation.OWNER_SECURITY_INFORMATION, out sd);
                CommonSecurityDescriptor ObtainedSecurityDescriptor =
                    new CommonSecurityDescriptor(false, true, sd.Value.SecurityDescriptor, 0);
                Site.Assert.AreEqual("O:BA", ObtainedSecurityDescriptor.GetSddlForm(AccessControlSections.Owner), "Owner should be changed back to the Administrator.");
            }
        }
    }
} 
