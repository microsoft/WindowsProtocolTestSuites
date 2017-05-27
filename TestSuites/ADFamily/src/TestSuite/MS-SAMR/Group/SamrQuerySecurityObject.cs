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
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrQuerySecurityObject with SACL_SECURITY_INFORMATION.")]
        public void SamrQuerySecurityObject_Group_SACL_SECURITY_INFORMATION()
        {
            ConnectAndOpenGroup(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName,
                _samrProtocolAdapter.DomainAdminGroup, out _groupHandle, (uint)Common_ACCESS_MASK.ACCESS_SYSTEM_SECURITY);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQuerySecurityObject with SACL_SECURITY_INFORMATION.");
            _SAMPR_SR_SECURITY_DESCRIPTOR? securityDescriptor;

            HRESULT result = _samrProtocolAdapter.SamrQuerySecurityObject(_groupHandle, SecurityInformation.SACL_SECURITY_INFORMATION, out securityDescriptor);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQuerySecurityObject returns:{0}.", result);
            Site.Assert.IsNotNull(securityDescriptor, "The returned securityDescriptor should not be null.");
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
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrQuerySecurityObject with OWNER_SECURITY_INFORMATION for a group object.")]
        public void SamrQuerySecurityObject_Group_OWNER_SECURITY_INFORMATION()
        {
            ConnectAndOpenGroup(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName,
                _samrProtocolAdapter.DomainAdminGroup, out _groupHandle, (uint)Common_ACCESS_MASK.READ_CONTROL);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQuerySecurityObject with OWNER_SECURITY_INFORMATION");
            _SAMPR_SR_SECURITY_DESCRIPTOR? securityDescriptor;

            HRESULT result = _samrProtocolAdapter.SamrQuerySecurityObject(_groupHandle, SecurityInformation.OWNER_SECURITY_INFORMATION, out securityDescriptor);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQuerySecurityObject returns:{0}.", result);
            Site.Assert.IsNotNull(securityDescriptor, "The returned securityDescriptor should not be null.");
            _SECURITY_DESCRIPTOR sd = DtypUtility.DecodeSecurityDescriptor(securityDescriptor.Value.SecurityDescriptor);
            Site.Assert.IsNotNull(sd.OwnerSid, "3.1.5.12.2 If this bit(OWNER_SECURITY_INFORMATION) is set, the client requests that the Owner member be returned.");
            Site.Assert.IsNull(sd.GroupSid, "3.1.5.12.2 If this bit(GROUP_SECURITY_INFORMATION) is not set, the client requests that the Group member not be returned.");
            Site.Assert.IsNull(sd.Dacl, "3.1.5.12.2 If this bit(DACL_SECURITY_INFORMATION) is not set, the client requests that the DACL not be returned.");
            Site.Assert.IsNull(sd.Sacl, "3.1.5.12.2 If this bit(SACL_SECURITY_INFORMATION) is not set, the client requests that the SACL not be returned.");
            Site.Assert.AreEqual(AdministratorSid, DtypUtility.ToSddlString((_SID)sd.OwnerSid), "3.1.5.12.2 The Owner and Group fields of the security descriptor MUST be the administrator's SID (S-1-5-32-544).");  
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrQuerySecurityObject with DACL_SECURITY_INFORMATION.")]
        public void SamrQuerySecurityObject_Group_DACL_SECURITY_INFORMATION_DomainAdminGroup()
        {
            ConnectAndOpenGroup(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName,
                _samrProtocolAdapter.DomainAdminGroup, out _groupHandle, (uint)Common_ACCESS_MASK.READ_CONTROL);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQuerySecurityObject with DACL_SECURITY_INFORMATION.");
            _SAMPR_SR_SECURITY_DESCRIPTOR? securityDescriptor;

            HRESULT result = _samrProtocolAdapter.SamrQuerySecurityObject(_groupHandle, SecurityInformation.DACL_SECURITY_INFORMATION, out securityDescriptor);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQuerySecurityObject returns:{0}.", result);
            Site.Assert.IsNotNull(securityDescriptor, "The returned securityDescriptor should not be null.");
            _SECURITY_DESCRIPTOR sd = DtypUtility.DecodeSecurityDescriptor(securityDescriptor.Value.SecurityDescriptor);
            Site.Assert.IsNull(sd.OwnerSid, "3.1.5.12.2 If this bit(OWNER_SECURITY_INFORMATION) is not set, the client requests that the Owner member not be returned.");
            Site.Assert.IsNull(sd.GroupSid, "3.1.5.12.2 If this bit(GROUP_SECURITY_INFORMATION) is not set, the client requests that the Group member not be returned.");
            Site.Assert.IsNotNull(sd.Dacl, "3.1.5.12.2 If this bit(DACL_SECURITY_INFORMATION) is set, the client requests that the DACL be returned.");
            Site.Assert.IsNull(sd.Sacl, "3.1.5.12.2 If this bit(SACL_SECURITY_INFORMATION) is not set, the client requests that the SACL not be returned.");
            bool isAdminSidMatched = false;
            bool isWorldSidMatched = false;
            foreach (var ace in sd.Dacl.Value.Aces)
            {
                if (DtypUtility.ToSddlString(((_ACCESS_ALLOWED_ACE)ace).Sid).Equals(AdministratorSid, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (((_ACCESS_ALLOWED_ACE)ace).Mask == (uint)Group_ACCESS_MASK.GROUP_ALL_ACCESS)
                    {
                        isAdminSidMatched = true;
                    }
                }
                if (DtypUtility.ToSddlString(((_ACCESS_ALLOWED_ACE)ace).Sid).Equals(WorldSid, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (((_ACCESS_ALLOWED_ACE)ace).Mask == (uint)(Group_ACCESS_MASK.GROUP_EXECUTE | Group_ACCESS_MASK.GROUP_READ))
                    {
                        isWorldSidMatched = true;
                    }
                }
            }
            Site.Assert.IsTrue(isAdminSidMatched, "3.1.5.12.2.1 if ObjectHandle.Object refers to a group or alias object that is the Domain Admins group or Administrators alias, or a member of Domain Admins or Administrators, the DACL MUST contain AdministratorSid:GROUP_ALL_ACCESS.");
            Site.Assert.IsTrue(isWorldSidMatched, "3.1.5.12.2.1 if ObjectHandle.Object refers to a group or alias object that is the Domain Admins group or Administrators alias, or a member of Domain Admins or Administrators, the DACL MUST contain WorldSid:GROUP_EXECUTE|GROUP_READ.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrQuerySecurityObject with DACL_SECURITY_INFORMATION.")]
        public void SamrQuerySecurityObject_Group_DACL_SECURITY_INFORMATION_NonDomainAdminGroup()
        {
            ConnectAndOpenGroup(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName,
                "Domain Users", out _groupHandle, (uint)Common_ACCESS_MASK.READ_CONTROL);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQuerySecurityObject with DACL_SECURITY_INFORMATION.");
            _SAMPR_SR_SECURITY_DESCRIPTOR? securityDescriptor;

            HRESULT result = _samrProtocolAdapter.SamrQuerySecurityObject(_groupHandle, SecurityInformation.DACL_SECURITY_INFORMATION, out securityDescriptor);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQuerySecurityObject returns:{0}.", result);
            Site.Assert.IsNotNull(securityDescriptor, "The returned securityDescriptor should not be null.");
            _SECURITY_DESCRIPTOR sd = DtypUtility.DecodeSecurityDescriptor(securityDescriptor.Value.SecurityDescriptor);
            Site.Assert.IsNull(sd.OwnerSid, "3.1.5.12.2 If this bit(OWNER_SECURITY_INFORMATION) is not set, the client requests that the Owner member not be returned.");
            Site.Assert.IsNull(sd.GroupSid, "3.1.5.12.2 If this bit(GROUP_SECURITY_INFORMATION) is not set, the client requests that the Group member not be returned.");
            Site.Assert.IsNotNull(sd.Dacl, "3.1.5.12.2 If this bit(DACL_SECURITY_INFORMATION) is set, the client requests that the DACL be returned.");
            Site.Assert.IsNull(sd.Sacl, "3.1.5.12.2 If this bit(SACL_SECURITY_INFORMATION) is not set, the client requests that the SACL not be returned.");
            bool isAdminSidMatched = false;
            bool isWorldSidMatched = false;
            bool isAccountOperatorsSidMatched = false;
            foreach (var ace in sd.Dacl.Value.Aces)
            {
                if (DtypUtility.ToSddlString(((_ACCESS_ALLOWED_ACE)ace).Sid).Equals(AdministratorSid, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (((_ACCESS_ALLOWED_ACE)ace).Mask == (uint)Group_ACCESS_MASK.GROUP_ALL_ACCESS)
                    {
                        isAdminSidMatched = true;
                    }
                    else
                    {
                        Site.Log.Add(LogEntryKind.Debug, "The AdministratorSid's Access Mask is {0}.", ((_ACCESS_ALLOWED_ACE)ace).Mask);
                    }
                }
                if (DtypUtility.ToSddlString(((_ACCESS_ALLOWED_ACE)ace).Sid).Equals(WorldSid, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (((_ACCESS_ALLOWED_ACE)ace).Mask == (uint)(Group_ACCESS_MASK.GROUP_EXECUTE | Group_ACCESS_MASK.GROUP_READ))
                    {
                        isWorldSidMatched = true;
                    }
                    else
                    {
                        Site.Log.Add(LogEntryKind.Debug, "The WorldSid's Access Mask is {0}.", ((_ACCESS_ALLOWED_ACE)ace).Mask);
                    }
                }
                if (DtypUtility.ToSddlString(((_ACCESS_ALLOWED_ACE)ace).Sid).Equals(AccountOperatorsSid, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (((_ACCESS_ALLOWED_ACE)ace).Mask == (uint)(Group_ACCESS_MASK.GROUP_ALL_ACCESS))
                    {
                        isAccountOperatorsSidMatched = true;
                    }
                    else
                    {
                        Site.Log.Add(LogEntryKind.Debug, "The AccountOperatorsSid's Access Mask is {0}.", ((_ACCESS_ALLOWED_ACE)ace).Mask);
                    }
                }
            }
            Site.Assert.IsTrue(isAdminSidMatched, "3.1.5.12.2.1 if ObjectHandle.Object refers to any group object that does not satisfy the previous condition, the DACL MUST contain AdministratorSid:GROUP_ALL_ACCESS.");
            Site.Assert.IsTrue(isWorldSidMatched, "3.1.5.12.2.1 if ObjectHandle.Object refers to any group object that does not satisfy the previous condition, the DACL MUST contain WorldSid:GROUP_EXECUTE|GROUP_READ.");
            Site.Assert.IsTrue(isAccountOperatorsSidMatched, "3.1.5.12.2.1 if ObjectHandle.Object refers to any group object that does not satisfy the previous condition, the DACL MUST contain AccountOperatorSid:GROUP_ALL_ACCESS.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrQuerySecurityObject when ObjectHandle.GrantedAccess has no required access.")]
        public void SamrQuerySecurityObject_Group_AccessDenied()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Connect and open a user handle with GROUP_ADD_MEMBER.");
            ConnectAndOpenGroup(_samrProtocolAdapter.PDCNetbiosName, _samrProtocolAdapter.PrimaryDomainDnsName,
                _samrProtocolAdapter.DomainAdminGroup, out _groupHandle, (uint)Group_ACCESS_MASK.GROUP_ADD_MEMBER);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQuerySecurityObject with OWNER_SECURITY_INFORMATION");
            _SAMPR_SR_SECURITY_DESCRIPTOR? securityDescriptor;

            HRESULT result = _samrProtocolAdapter.SamrQuerySecurityObject(_groupHandle, SecurityInformation.OWNER_SECURITY_INFORMATION, out securityDescriptor);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "SamrQuerySecurityObject returns error code:{0}.", result.ToString("X"));
            Site.Assert.IsNull(securityDescriptor, "3.1.5.12.2.1 ObjectHandle.GrantedAccess MUST have the required access specified in the following table. On error, the server MUST abort processing and return STATUS_ACCESS_DENIED.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrQuerySecurityObject with SACL_SECURITY_INFORMATION");
            result = _samrProtocolAdapter.SamrQuerySecurityObject(_groupHandle, SecurityInformation.SACL_SECURITY_INFORMATION, out securityDescriptor);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "SamrQuerySecurityObject returns error code:{0}.", result.ToString("X"));
            Site.Assert.IsNull(securityDescriptor, "3.1.5.12.2.1 ObjectHandle.GrantedAccess MUST have the required access specified in the following table. On error, the server MUST abort processing and return STATUS_ACCESS_DENIED.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrQuerySecurityObject with GROUP_SECURITY_INFORMATION");
            result = _samrProtocolAdapter.SamrQuerySecurityObject(_groupHandle, SecurityInformation.GROUP_SECURITY_INFORMATION, out securityDescriptor);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "SamrQuerySecurityObject returns error code:{0}.", result.ToString("X"));
            Site.Assert.IsNull(securityDescriptor, "3.1.5.12.2.1 ObjectHandle.GrantedAccess MUST have the required access specified in the following table. On error, the server MUST abort processing and return STATUS_ACCESS_DENIED.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrQuerySecurityObject with DACL_SECURITY_INFORMATION");
            result = _samrProtocolAdapter.SamrQuerySecurityObject(_groupHandle, SecurityInformation.DACL_SECURITY_INFORMATION, out securityDescriptor);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "SamrQuerySecurityObject returns error code:{0}.", result.ToString("X"));
            Site.Assert.IsNull(securityDescriptor, "3.1.5.12.2.1 ObjectHandle.GrantedAccess MUST have the required access specified in the following table. On error, the server MUST abort processing and return STATUS_ACCESS_DENIED.");
        }
    }
}
