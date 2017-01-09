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
        [TestCategory("SAMR-Group")]
        [Description("Non-DC Test: This is to test SamrQuerySecurityObject with SACL_SECURITY_INFORMATION.")]
        public void SamrQuerySecurityObject_Group_SACL_SECURITY_INFORMATION_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
                out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            string groupName = testGroupName;
            uint relativeId = 0;
            try
            {
                CreateGroup_NonDC(groupName, _serverHandle, _domainHandle, out _groupHandle, out relativeId);
                HRESULT result = _samrProtocolAdapter.SamrOpenGroup(_domainHandle, (uint)Common_ACCESS_MASK.ACCESS_SYSTEM_SECURITY, relativeId, out _groupHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrOpenGroup succeeded.");

                Site.Log.Add(LogEntryKind.TestStep, "SamrQuerySecurityObject with SACL_SECURITY_INFORMATION.");
                _SAMPR_SR_SECURITY_DESCRIPTOR? securityDescriptor;

                result = _samrProtocolAdapter.SamrQuerySecurityObject(_groupHandle, SecurityInformation.SACL_SECURITY_INFORMATION, out securityDescriptor);
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
                HRESULT result = _samrProtocolAdapter.SamrOpenGroup(_domainHandle, (uint)Group_ACCESS_MASK.GROUP_ALL_ACCESS, relativeId, out _groupHandle);
                Site.Log.Add(LogEntryKind.TestStep, "SamrOpenGroup: delete the created group.");
                result = _samrProtocolAdapter.SamrDeleteGroup(ref _groupHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteGroup succeeded.");
            } 
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Group")]
        [Description("Non-DC Test: This is to test SamrQuerySecurityObject with OWNER_SECURITY_INFORMATION for a group object.")]
        public void SamrQuerySecurityObject_Group_OWNER_SECURITY_INFORMATION_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
               out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            string groupName = testGroupName;
            uint relativeId = 0;
            try
            {
                CreateGroup_NonDC(groupName, _serverHandle, _domainHandle, out _groupHandle, out relativeId);
                HRESULT result = _samrProtocolAdapter.SamrOpenGroup(_domainHandle, (uint)Common_ACCESS_MASK.READ_CONTROL, relativeId, out _groupHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrOpenGroup succeeded.");

                Site.Log.Add(LogEntryKind.TestStep, "SamrQuerySecurityObject with OWNER_SECURITY_INFORMATION");
                _SAMPR_SR_SECURITY_DESCRIPTOR? securityDescriptor;

                result = _samrProtocolAdapter.SamrQuerySecurityObject(_groupHandle, SecurityInformation.OWNER_SECURITY_INFORMATION, out securityDescriptor);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQuerySecurityObject returns:{0}.", result);
                Site.Assert.IsNotNull(securityDescriptor, "The returned securityDescriptor should not be null.");
                _SECURITY_DESCRIPTOR sd = DtypUtility.DecodeSecurityDescriptor(securityDescriptor.Value.SecurityDescriptor);
                Site.Assert.IsNotNull(sd.OwnerSid, "3.1.5.12.2 If this bit(OWNER_SECURITY_INFORMATION) is set, the client requests that the Owner member be returned.");
                Site.Assert.IsNull(sd.GroupSid, "3.1.5.12.2 If this bit(GROUP_SECURITY_INFORMATION) is not set, the client requests that the Group member not be returned.");
                Site.Assert.IsNull(sd.Dacl, "3.1.5.12.2 If this bit(DACL_SECURITY_INFORMATION) is not set, the client requests that the DACL not be returned.");
                Site.Assert.IsNull(sd.Sacl, "3.1.5.12.2 If this bit(SACL_SECURITY_INFORMATION) is not set, the client requests that the SACL not be returned.");
                Site.Assert.AreEqual(AdministratorSid, DtypUtility.ToSddlString((_SID)sd.OwnerSid), "3.1.5.12.2 The Owner and Group fields of the security descriptor MUST be the administrator's SID (S-1-5-32-544).");  
       
            }
            finally
            {
                HRESULT result = _samrProtocolAdapter.SamrOpenGroup(_domainHandle, (uint)Group_ACCESS_MASK.GROUP_ALL_ACCESS, relativeId, out _groupHandle);
                Site.Log.Add(LogEntryKind.TestStep, "SamrOpenGroup: delete the created group.");
                result = _samrProtocolAdapter.SamrDeleteGroup(ref _groupHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteGroup succeeded.");
            } 
        }


        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Group")]
        [Description("Non-DC Test: This is to test SamrQuerySecurityObject with DACL_SECURITY_INFORMATION.")]
        public void SamrQuerySecurityObject_Group_DACL_SECURITY_INFORMATION_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
               out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            string groupName = testGroupName;
            uint relativeId = 0;
            try
            {
                CreateGroup_NonDC(groupName, _serverHandle, _domainHandle, out _groupHandle, out relativeId);
                HRESULT result = _samrProtocolAdapter.SamrOpenGroup(_domainHandle, (uint)Common_ACCESS_MASK.READ_CONTROL, relativeId, out _groupHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrOpenGroup succeeded.");

                Site.Log.Add(LogEntryKind.TestStep, "SamrQuerySecurityObject with DACL_SECURITY_INFORMATION.");
                _SAMPR_SR_SECURITY_DESCRIPTOR? securityDescriptor;

                result = _samrProtocolAdapter.SamrQuerySecurityObject(_groupHandle, SecurityInformation.DACL_SECURITY_INFORMATION, out securityDescriptor);
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
                HRESULT result = _samrProtocolAdapter.SamrOpenGroup(_domainHandle, (uint)Group_ACCESS_MASK.GROUP_ALL_ACCESS, relativeId, out _groupHandle);
                Site.Log.Add(LogEntryKind.TestStep, "SamrOpenGroup: delete the created group.");
                result = _samrProtocolAdapter.SamrDeleteGroup(ref _groupHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteGroup succeeded.");
            } 
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Group")]
        [Description("Non-DC Test: This is to test SamrQuerySecurityObject when ObjectHandle.GrantedAccess has no required access.")]
        public void SamrQuerySecurityObject_Group_AccessDenied_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
              out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            string groupName = testGroupName;
            uint relativeId = 0;
            try
            {
                CreateGroup_NonDC(groupName, _serverHandle, _domainHandle, out _groupHandle, out relativeId);
                HRESULT result = _samrProtocolAdapter.SamrOpenGroup(_domainHandle, (uint)Group_ACCESS_MASK.GROUP_ADD_MEMBER, relativeId, out _groupHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrOpenGroup succeeded.");

                Site.Log.Add(LogEntryKind.TestStep, "SamrQuerySecurityObject with OWNER_SECURITY_INFORMATION");
                _SAMPR_SR_SECURITY_DESCRIPTOR? securityDescriptor;

                result = _samrProtocolAdapter.SamrQuerySecurityObject(_groupHandle, SecurityInformation.OWNER_SECURITY_INFORMATION, out securityDescriptor);
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
            finally
            {
                HRESULT result = _samrProtocolAdapter.SamrOpenGroup(_domainHandle, (uint)Group_ACCESS_MASK.GROUP_ALL_ACCESS, relativeId, out _groupHandle);
                Site.Log.Add(LogEntryKind.TestStep, "SamrOpenGroup: delete the created group.");
                result = _samrProtocolAdapter.SamrDeleteGroup(ref _groupHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteGroup succeeded.");
            } 
        }
    }
}
