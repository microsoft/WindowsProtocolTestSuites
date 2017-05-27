// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Threading;
using System.Security.Principal;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts;
using System.DirectoryServices.Protocols;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
using Microsoft.Protocols.TestTools.StackSdk.Security.Samr;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Modeling;
using System.Security.AccessControl;

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
        [Description("Non-DC Test: This is to test SamrEnumerateGroupsInDomain.")]
        public void SamrEnumerateGroupsInDomain_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            Site.Log.Add(LogEntryKind.TestStep, "SamrEnumerateGroupsInDomain enumerates all groups.");
            uint? enumerationContext = 0;
            _SAMPR_ENUMERATION_BUFFER? buffer = null;
            uint preferedMaximumLength = 3000;
            uint countReturned = 0;
            HRESULT result = _samrProtocolAdapter.SamrEnumerateGroupsInDomain(_domainHandle, ref enumerationContext, out buffer, preferedMaximumLength, out countReturned);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "3.1.5.2.2 The server MUST enable a client to obtain a listing, without duplicates, of all database objects that satisfy the criteria of Enumerate-Filter.");
            Site.Assert.AreEqual(buffer.Value.EntriesRead, countReturned, "3.1.5.2.2 On output, CountReturned MUST equal Buffer.EntriesRead.");
            _samrProtocolAdapter.VerifyConstraintsInCommonProcessingOfEnumeration(buffer.Value.Buffer, preferedMaximumLength);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Group")]
        [Description("Non-DC Test: This is to test SamrEnumerateGroupsInDomain.")]
        public void SamrEnumerateGroupsInDomain_AddDeleteGroup_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            string groupName = testGroupName;
            
            Site.Log.Add(LogEntryKind.TestStep, "Record the current DACL of domain DM.");
            _SAMPR_SR_SECURITY_DESCRIPTOR? sd;
            HRESULT hResult = _samrProtocolAdapter.SamrQuerySecurityObject(_domainHandle, SecurityInformation.DACL_SECURITY_INFORMATION, out sd);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrQuerySecurityObject must return STATUS_SUCCESS.");
            Site.Assert.IsNotNull(sd, "The SecurityDescriptor returned by SamrQuerySecurityObject is not null.");
            CommonSecurityDescriptor ObtainedSecurityDescriptor = new CommonSecurityDescriptor(false, true, sd.Value.SecurityDescriptor, 0);
            string oldDACL = ObtainedSecurityDescriptor.GetSddlForm(AccessControlSections.Access);
            Site.Log.Add(LogEntryKind.TestStep, "Old DACL: {0}", oldDACL);
  
            Site.Log.Add(LogEntryKind.TestStep, "Modify the DACL value of domain DM to get the access to create group in this domain.");
            CommonSecurityDescriptor commonsd = new CommonSecurityDescriptor(false, true, "D:(A;;0xf07ff;;;WD)(A;;0xf07ff;;;BA)(A;;0xf07ff;;;AO)");
            byte[] buff = new byte[commonsd.BinaryLength];
            commonsd.GetBinaryForm(buff, 0);
            _SAMPR_SR_SECURITY_DESCRIPTOR securityDescriptor = new _SAMPR_SR_SECURITY_DESCRIPTOR()
            {
                SecurityDescriptor = buff,
                Length = (uint)buff.Length
            };
            Site.Log.Add(LogEntryKind.TestStep,
                "SamrSetSecurityObject, SecurityInformation: OWNER_SECURITY_INFORMATION. ");
            hResult = _samrProtocolAdapter.SamrSetSecurityObject(_domainHandle, SecurityInformation.DACL_SECURITY_INFORMATION, securityDescriptor);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrSetSecurityObject must return STATUS_SUCCESS.");
            hResult = _samrProtocolAdapter.SamrQuerySecurityObject(_domainHandle, SecurityInformation.DACL_SECURITY_INFORMATION, out sd);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrQuerySecurityObject must return STATUS_SUCCESS.");
            ObtainedSecurityDescriptor = new CommonSecurityDescriptor(false, true, sd.Value.SecurityDescriptor, 0);
            Site.Log.Add(LogEntryKind.TestStep, "New DACL: {0}", ObtainedSecurityDescriptor.GetSddlForm(AccessControlSections.Access));

            Site.Log.Add(LogEntryKind.TestStep, "Reopen the domain DM");
            _RPC_SID domainSid;
            hResult = _samrProtocolAdapter.SamrLookupDomainInSamServer(
                _serverHandle,
                _samrProtocolAdapter.domainMemberFqdn,
                out domainSid);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrLookupDomainInSamServer returns:{0}.", hResult);
            Site.Log.Add(LogEntryKind.TestStep, "SamrOpenDomain: obtain a handle to a domain object, given SID.");
            hResult = _samrProtocolAdapter.SamrOpenDomain(
                _serverHandle,
                Utilities.MAXIMUM_ALLOWED,
                domainSid,
                out _domainHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrOpenDomain returns:{0}.", hResult);
            Site.Assert.IsNotNull(hResult, "The returned domain handle is:{0}.", _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrEnumerateUsersInDomain enumerates all groups.");
            uint? enumerationContext = 0;
            _SAMPR_ENUMERATION_BUFFER? buffer = null;
            uint preferedMaximumLength = 3000;
            uint countReturned = 0;
            HRESULT result = _samrProtocolAdapter.SamrEnumerateGroupsInDomain(_domainHandle, ref enumerationContext, out buffer, preferedMaximumLength, out countReturned);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "3.1.5.2.2 The server MUST enable a client to obtain a listing, without duplicates, of all database objects that satisfy the criteria of Enumerate-Filter.");
            Site.Assert.AreEqual(buffer.Value.EntriesRead, countReturned, "3.1.5.2.2 On output, CountReturned MUST equal Buffer.EntriesRead.");
            _samrProtocolAdapter.VerifyConstraintsInCommonProcessingOfEnumeration(buffer.Value.Buffer, preferedMaximumLength);
           
            uint countReturnedAdded = 0;
            uint relativeId = 0;
            try
            {               
                buffer = null;
                Site.Log.Add(LogEntryKind.TestStep, "Create a group with name \"{0}\".", groupName);
                result = _samrProtocolAdapter.SamrCreateGroupInDomain(_domainHandle, groupName, (uint)Group_ACCESS_MASK.GROUP_ALL_ACCESS,
                    out _groupHandle, out relativeId);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateGroupInDomain succeeded.");
                
                result = _samrProtocolAdapter.SamrEnumerateGroupsInDomain(_domainHandle, ref enumerationContext, out buffer, preferedMaximumLength, out countReturnedAdded);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "3.1.5.2.2 The server MUST enable a client to obtain a listing, without duplicates, of all database objects that satisfy the criteria of Enumerate-Filter.");
                Site.Assert.AreEqual(buffer.Value.EntriesRead, countReturnedAdded, "3.1.5.2.2 On output, CountReturned MUST equal Buffer.EntriesRead.");                    
                _samrProtocolAdapter.VerifyConstraintsInCommonProcessingOfEnumeration(buffer.Value.Buffer, preferedMaximumLength);
                bool flag = false;
                foreach (_SAMPR_RID_ENUMERATION rid in buffer.Value.Buffer)
                {
                    if (relativeId == rid.RelativeId)
                    {
                        flag = true;
                    }                   
                }
                Site.Assert.IsTrue(flag, "3.1.5.2.2 If an object that satisfies Enumerate-Filter is added between successive Enumerate method calls in a session, and said object has a RID that is greater than the RIDs of all objects returned in previous calls, the server MUST return said object before the enumeration is complete.");
            }
            finally
            {
                uint countReturnedDeleted = 0;
                buffer = null;
                result = _samrProtocolAdapter.SamrDeleteGroup(ref _groupHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteGroup returns:{0}.", result);

                result = _samrProtocolAdapter.SamrEnumerateGroupsInDomain(_domainHandle, ref enumerationContext, out buffer, preferedMaximumLength, out countReturnedDeleted);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "3.1.5.2.2 The server MUST enable a client to obtain a listing, without duplicates, of all database objects that satisfy the criteria of Enumerate-Filter.");
                Site.Assert.AreEqual(buffer.Value.EntriesRead, countReturnedDeleted, "3.1.5.2.2 On output, CountReturned MUST equal Buffer.EntriesRead.");                              
                _samrProtocolAdapter.VerifyConstraintsInCommonProcessingOfEnumeration(buffer.Value.Buffer, preferedMaximumLength);

                bool flag = false;
                foreach (_SAMPR_RID_ENUMERATION rid in buffer.Value.Buffer)
                {
                    if (relativeId == rid.RelativeId)
                    {
                        flag = true;
                    }
                }
                Site.Assert.IsFalse(flag, "3.1.5.2.2 If an object that satisfies Enumerate-Filter is deleted between successive Enumerate method calls in a session, and said object has not already been returned by a previous method call in the session, the server MUST NOT return said object before the enumeration is complete.");

                Site.Log.Add(LogEntryKind.TestStep, "Change back the DACL of domain DM.");
                commonsd = new CommonSecurityDescriptor(false, true, oldDACL);
                buff = new byte[commonsd.BinaryLength];
                commonsd.GetBinaryForm(buff, 0);
                securityDescriptor = new _SAMPR_SR_SECURITY_DESCRIPTOR()
                {
                    SecurityDescriptor = buff,
                    Length = (uint)buff.Length
                };
                Site.Log.Add(LogEntryKind.TestStep,
                    "SamrSetSecurityObject, SecurityInformation: OWNER_SECURITY_INFORMATION. ");
                hResult = _samrProtocolAdapter.SamrSetSecurityObject(_domainHandle, SecurityInformation.DACL_SECURITY_INFORMATION, securityDescriptor);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrSetSecurityObject must return STATUS_SUCCESS.");
                hResult = _samrProtocolAdapter.SamrQuerySecurityObject(_domainHandle, SecurityInformation.DACL_SECURITY_INFORMATION, out sd);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrQuerySecurityObject must return STATUS_SUCCESS.");
                ObtainedSecurityDescriptor = new CommonSecurityDescriptor(false, true, sd.Value.SecurityDescriptor, 0);
                Site.Assert.AreEqual(oldDACL, ObtainedSecurityDescriptor.GetSddlForm(AccessControlSections.Access), "The DACL of domain DM should be changed back");   
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
        [Description("Non-DC Test: This is to test SamrEnumerateGroupsInDomain.")]
        public void SamrEnumerateGroupsInDomain_STATUS_MORE_ENTRIES_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            Site.Log.Add(LogEntryKind.TestStep, "SamrEnumerateGroupsInDomain enumerates all groups.");
            uint? enumerationContext = 0;
            _SAMPR_ENUMERATION_BUFFER? buffer = null;
            uint preferedMaximumLength = 0;
            uint countReturned = 0;
            HRESULT result = _samrProtocolAdapter.SamrEnumerateGroupsInDomain(_domainHandle, ref enumerationContext, out buffer, preferedMaximumLength, out countReturned);

            Site.Assert.AreEqual(HRESULT.STATUS_MORE_ENTRIES, result, "3.1.5.2.2 9.	STATUS_MORE_ENTRIES MUST be returned if the server returns less than all of the database objects in Buffer.Buffer because of the PreferedMaximumLength restriction. ");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Group")]
        [Description("Non-DC Test: This is to test SamrEnumerateGroupsInDomain with invalid handle.")]
        public void SamrEnumerateGroupsInDomain_InvalidHandle_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            Site.Log.Add(LogEntryKind.TestStep, "SamrEnumerateGroupsInDomain enumerates all groups.");
            uint? enumerationContext = 0;
            _SAMPR_ENUMERATION_BUFFER? buffer = null;
            uint preferedMaximumLength = 3000;
            uint countReturned = 0;
            HRESULT result = _samrProtocolAdapter.SamrEnumerateGroupsInDomain(_serverHandle, ref enumerationContext, out buffer, preferedMaximumLength, out countReturned);
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "3.1.5.2.2 The server MUST return an error if DomainHandle.HandleType is not equal to Domain.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Group")]
        [Description("Non-DC Test: This is to test SamrEnumerateGroupsInDomain with no required access.")]
        public void SamrEnumerateGroupsInDomain_STATUS_ACCESS_DENIED_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.DOMAIN_READ);

            Site.Log.Add(LogEntryKind.TestStep, "SamrEnumerateGroupsInDomain with DOMAIN_READ access.");
            uint? enumerationContext = 0;
            _SAMPR_ENUMERATION_BUFFER? buffer = null;
            uint preferedMaximumLength = 3000;
            uint countReturned = 0;
            HRESULT result = _samrProtocolAdapter.SamrEnumerateGroupsInDomain(_domainHandle, ref enumerationContext, out buffer, preferedMaximumLength, out countReturned);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "3.1.5.2.2 DomainHandle.GrantedAccess MUST have the required access specified in section 3.1.2.1. Otherwise, the server MUST return STATUS_ACCESS_DENIED.");
        }
    }
}
