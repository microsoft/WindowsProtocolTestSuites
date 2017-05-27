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
        [TestCategory("SAMR-Alias")]
        [Description("Non-DC Test: This is to test SamrQuerySecurityObject with SACL_SECURITY_INFORMATION.")]
        public void SamrQuerySecurityObject_Alias_SACL_SECURITY_INFORMATION_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
                out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            string aliasName = testAliasName;
            uint relativeId = 0;
            try
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrCreateAliasInDomain: create an alias with Name:{0}, and DesiredAccess:{1}",
                    testAliasName, ALIAS_ACCESS_MASK.ALIAS_ALL_ACCESS);

                HRESULT result = _samrProtocolAdapter.SamrCreateAliasInDomain(
                    _domainHandle,
                    aliasName,
                    (uint)ALIAS_ACCESS_MASK.ALIAS_ALL_ACCESS,
                    out _aliasHandle,
                    out relativeId);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateAliasInDomain returns:{0}.", result);
                Site.Assert.IsNotNull(_aliasHandle, "The returned alias handle is: {0}.", _aliasHandle);

                result = _samrProtocolAdapter.SamrOpenAlias(_domainHandle, (uint)Common_ACCESS_MASK.ACCESS_SYSTEM_SECURITY, relativeId, out _aliasHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrOpenAlias succeeded.");

                Site.Log.Add(LogEntryKind.TestStep, "SamrQuerySecurityObject with SACL_SECURITY_INFORMATION.");
                _SAMPR_SR_SECURITY_DESCRIPTOR? securityDescriptor;

                result = _samrProtocolAdapter.SamrQuerySecurityObject(_aliasHandle, SecurityInformation.SACL_SECURITY_INFORMATION, out securityDescriptor);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQuerySecurityObject returns:{0}.", result);
                Site.Assert.IsNotNull(securityDescriptor, "The returned securityDescriptor should not be null.");
                _SECURITY_DESCRIPTOR sd = DtypUtility.DecodeSecurityDescriptor(securityDescriptor.Value.SecurityDescriptor);
                Site.Assert.IsNull(sd.OwnerSid, "3.1.5.12.2 If this bit(OWNER_SECURITY_INFORMATION) is not set, the client requests that the Owner member not be returned.");
                Site.Assert.IsNull(sd.GroupSid, "3.1.5.12.2 If this bit(GROUP_SECURITY_INFORMATION) is not set, the client requests that the Group member not be returned.");
                Site.Assert.IsNull(sd.Dacl, "3.1.5.12.2 If this bit(DACL_SECURITY_INFORMATION) is not set, the client requests that the DACL not be returned.");
                Site.Assert.IsNotNull(sd.Sacl, "3.1.5.12.2 If this bit(SACL_SECURITY_INFORMATION) is set, the client requests that the SACL be returned.");
            }
            finally
            {
                HRESULT result = _samrProtocolAdapter.SamrOpenAlias(_domainHandle, (uint)ALIAS_ACCESS_MASK.ALIAS_ALL_ACCESS, relativeId, out _aliasHandle);
                Site.Log.Add(LogEntryKind.TestStep, "SamrOpenAlias: delete the created alias.");
                result = _samrProtocolAdapter.SamrDeleteAlias(ref _aliasHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteAlias succeeded.");
            }
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Alias")]
        [Description("Non-DC Test: This is to test SamrQuerySecurityObject with OWNER_SECURITY_INFORMATION for a alias object.")]
        public void SamrQuerySecurityObject_Alias_OWNER_SECURITY_INFORMATION_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
               out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            string aliasName = testAliasName;
            uint relativeId = 0;
            try
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrCreateAliasInDomain: create an alias with Name:{0}, and DesiredAccess:{1}",
                     testAliasName, ALIAS_ACCESS_MASK.ALIAS_ALL_ACCESS);

                HRESULT result = _samrProtocolAdapter.SamrCreateAliasInDomain(
                    _domainHandle,
                    aliasName,
                    (uint)ALIAS_ACCESS_MASK.ALIAS_ALL_ACCESS,
                    out _aliasHandle,
                    out relativeId);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateAliasInDomain returns:{0}.", result);
                Site.Assert.IsNotNull(_aliasHandle, "The returned alias handle is: {0}.", _aliasHandle);

                result = _samrProtocolAdapter.SamrOpenAlias(_domainHandle, (uint)Common_ACCESS_MASK.READ_CONTROL, relativeId, out _aliasHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrOpenAlias succeeded.");

                Site.Log.Add(LogEntryKind.TestStep, "SamrQuerySecurityObject with OWNER_SECURITY_INFORMATION");
                _SAMPR_SR_SECURITY_DESCRIPTOR? securityDescriptor;

                result = _samrProtocolAdapter.SamrQuerySecurityObject(_aliasHandle, SecurityInformation.OWNER_SECURITY_INFORMATION, out securityDescriptor);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQuerySecurityObject returns:{0}.", result);
                Site.Assert.IsNotNull(securityDescriptor, "The returned securityDescriptor should not be null.");
                _SECURITY_DESCRIPTOR sd = DtypUtility.DecodeSecurityDescriptor(securityDescriptor.Value.SecurityDescriptor);
                Site.Assert.IsNotNull(sd.OwnerSid, "3.1.5.12.2 If this bit(OWNER_SECURITY_INFORMATION) is set, the client requests that the Owner member be returned.");
                Site.Assert.IsNull(sd.GroupSid, "3.1.5.12.2 If this bit(GROUP_SECURITY_INFORMATION) is not set, the client requests that the Group member not be returned.");
                Site.Assert.IsNull(sd.Dacl, "3.1.5.12.2 If this bit(DACL_SECURITY_INFORMATION) is not set, the client requests that the DACL not be returned.");
                Site.Assert.IsNull(sd.Sacl, "3.1.5.12.2 If this bit(SACL_SECURITY_INFORMATION) is not set, the client requests that the SACL not be returned.");
                Site.Assert.AreEqual(AdministratorSid, DtypUtility.ToSddlString((_SID)sd.OwnerSid), "3.1.5.12.2 The Owner and Alias fields of the security descriptor MUST be the administrator's SID (S-1-5-32-544).");

            }
            finally
            {
                HRESULT result = _samrProtocolAdapter.SamrOpenAlias(_domainHandle, (uint)ALIAS_ACCESS_MASK.ALIAS_ALL_ACCESS, relativeId, out _aliasHandle);
                Site.Log.Add(LogEntryKind.TestStep, "SamrOpenAlias: delete the created alias.");
                result = _samrProtocolAdapter.SamrDeleteAlias(ref _aliasHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteAlias succeeded.");
            }
        }


        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Alias")]
        [Description("Non-DC Test: This is to test SamrQuerySecurityObject with DACL_SECURITY_INFORMATION.")]
        public void SamrQuerySecurityObject_Alias_DACL_SECURITY_INFORMATION_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
               out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            string aliasName = testAliasName;
            uint relativeId = 0;
            try
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrCreateAliasInDomain: create an alias with Name:{0}, and DesiredAccess:{1}",
                     testAliasName, ALIAS_ACCESS_MASK.ALIAS_ALL_ACCESS);

                HRESULT result = _samrProtocolAdapter.SamrCreateAliasInDomain(
                    _domainHandle,
                    aliasName,
                    (uint)ALIAS_ACCESS_MASK.ALIAS_ALL_ACCESS,
                    out _aliasHandle,
                    out relativeId);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateAliasInDomain returns:{0}.", result);
                Site.Assert.IsNotNull(_aliasHandle, "The returned alias handle is: {0}.", _aliasHandle);

                result = _samrProtocolAdapter.SamrOpenAlias(_domainHandle, (uint)Common_ACCESS_MASK.READ_CONTROL, relativeId, out _aliasHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrOpenAlias succeeded.");

                Site.Log.Add(LogEntryKind.TestStep, "SamrQuerySecurityObject with DACL_SECURITY_INFORMATION.");
                _SAMPR_SR_SECURITY_DESCRIPTOR? securityDescriptor;

                result = _samrProtocolAdapter.SamrQuerySecurityObject(_aliasHandle, SecurityInformation.DACL_SECURITY_INFORMATION, out securityDescriptor);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQuerySecurityObject returns:{0}.", result);
                Site.Assert.IsNotNull(securityDescriptor, "The returned securityDescriptor should not be null.");
                _SECURITY_DESCRIPTOR sd = DtypUtility.DecodeSecurityDescriptor(securityDescriptor.Value.SecurityDescriptor);
                Site.Assert.IsNull(sd.OwnerSid, "3.1.5.12.2 If this bit(OWNER_SECURITY_INFORMATION) is not set, the client requests that the Owner member not be returned.");
                Site.Assert.IsNull(sd.GroupSid, "3.1.5.12.2 If this bit(GROUP_SECURITY_INFORMATION) is not set, the client requests that the Group member not be returned.");
                Site.Assert.IsNotNull(sd.Dacl, "3.1.5.12.2 If this bit(DACL_SECURITY_INFORMATION) is set, the client requests that the DACL be returned.");
                Site.Assert.IsNull(sd.Sacl, "3.1.5.12.2 If this bit(SACL_SECURITY_INFORMATION) is not set, the client requests that the SACL not be returned.");

            }
            finally
            {
                HRESULT result = _samrProtocolAdapter.SamrOpenAlias(_domainHandle, (uint)ALIAS_ACCESS_MASK.ALIAS_ALL_ACCESS, relativeId, out _aliasHandle);
                Site.Log.Add(LogEntryKind.TestStep, "SamrOpenAlias: delete the created alias.");
                result = _samrProtocolAdapter.SamrDeleteAlias(ref _aliasHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteAlias succeeded.");
            }
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Alias")]
        [Description("Non-DC Test: This is to test SamrQuerySecurityObject when ObjectHandle.GrantedAccess has no required access.")]
        public void SamrQuerySecurityObject_Alias_AccessDenied_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
              out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            string aliasName = testAliasName;
            uint relativeId = 0;
            try
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrCreateAliasInDomain: create an alias with Name:{0}, and DesiredAccess:{1}",
                    testAliasName, ALIAS_ACCESS_MASK.ALIAS_ALL_ACCESS);

                HRESULT result = _samrProtocolAdapter.SamrCreateAliasInDomain(
                    _domainHandle,
                    aliasName,
                    (uint)ALIAS_ACCESS_MASK.ALIAS_ALL_ACCESS,
                    out _aliasHandle,
                    out relativeId);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateAliasInDomain returns:{0}.", result);
                Site.Assert.IsNotNull(_aliasHandle, "The returned alias handle is: {0}.", _aliasHandle);

                result = _samrProtocolAdapter.SamrOpenAlias(_domainHandle, (uint)ALIAS_ACCESS_MASK.ALIAS_ADD_MEMBER, relativeId, out _aliasHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrOpenAlias succeeded.");

                Site.Log.Add(LogEntryKind.TestStep, "SamrQuerySecurityObject with OWNER_SECURITY_INFORMATION");
                _SAMPR_SR_SECURITY_DESCRIPTOR? securityDescriptor;

                result = _samrProtocolAdapter.SamrQuerySecurityObject(_aliasHandle, SecurityInformation.OWNER_SECURITY_INFORMATION, out securityDescriptor);
                Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "SamrQuerySecurityObject returns error code:{0}.", result.ToString("X"));
                Site.Assert.IsNull(securityDescriptor, "3.1.5.12.2.1 ObjectHandle.GrantedAccess MUST have the required access specified in the following table. On error, the server MUST abort processing and return STATUS_ACCESS_DENIED.");

                Site.Log.Add(LogEntryKind.TestStep, "SamrQuerySecurityObject with SACL_SECURITY_INFORMATION");
                result = _samrProtocolAdapter.SamrQuerySecurityObject(_aliasHandle, SecurityInformation.SACL_SECURITY_INFORMATION, out securityDescriptor);
                Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "SamrQuerySecurityObject returns error code:{0}.", result.ToString("X"));
                Site.Assert.IsNull(securityDescriptor, "3.1.5.12.2.1 ObjectHandle.GrantedAccess MUST have the required access specified in the following table. On error, the server MUST abort processing and return STATUS_ACCESS_DENIED.");

                Site.Log.Add(LogEntryKind.TestStep, "SamrQuerySecurityObject with ALIAS_SECURITY_INFORMATION");
                result = _samrProtocolAdapter.SamrQuerySecurityObject(_aliasHandle, SecurityInformation.GROUP_SECURITY_INFORMATION, out securityDescriptor);
                Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "SamrQuerySecurityObject returns error code:{0}.", result.ToString("X"));
                Site.Assert.IsNull(securityDescriptor, "3.1.5.12.2.1 ObjectHandle.GrantedAccess MUST have the required access specified in the following table. On error, the server MUST abort processing and return STATUS_ACCESS_DENIED.");

                Site.Log.Add(LogEntryKind.TestStep, "SamrQuerySecurityObject with DACL_SECURITY_INFORMATION");
                result = _samrProtocolAdapter.SamrQuerySecurityObject(_aliasHandle, SecurityInformation.DACL_SECURITY_INFORMATION, out securityDescriptor);
                Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "SamrQuerySecurityObject returns error code:{0}.", result.ToString("X"));
                Site.Assert.IsNull(securityDescriptor, "3.1.5.12.2.1 ObjectHandle.GrantedAccess MUST have the required access specified in the following table. On error, the server MUST abort processing and return STATUS_ACCESS_DENIED.");
            }
            finally
            {
                HRESULT result = _samrProtocolAdapter.SamrOpenAlias(_domainHandle, (uint)ALIAS_ACCESS_MASK.ALIAS_ALL_ACCESS, relativeId, out _aliasHandle);
                Site.Log.Add(LogEntryKind.TestStep, "SamrOpenAlias: delete the created alias.");
                result = _samrProtocolAdapter.SamrDeleteAlias(ref _aliasHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteAlias succeeded.");
            }
        }
    }
}
