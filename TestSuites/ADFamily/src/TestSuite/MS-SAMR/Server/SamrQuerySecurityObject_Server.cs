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
        [TestCategory("Server")]
        [Description("SamrQuerySecurityObject, Query the Owner of PDC using SAM_SERVER_READ access.")]
        public void SamrQuerySecurityObject_Server_Owner()
        {
            HRESULT hResult;
            IChecker PtfAssert = TestClassBase.BaseTestSite.Assert;
            Site.Log.Add(LogEntryKind.TestStep,
                string.Format("SamrBind, Server:{0}, Domain:{1}, User:{2}, Password{3}.",
                _samrProtocolAdapter.pdcNetBIOSName,
                _samrProtocolAdapter.primaryDomainFqdn,
                _samrProtocolAdapter.DomainAdministratorName,
                _samrProtocolAdapter.DomainUserPassword));
            _samrProtocolAdapter.SamrBind(
                _samrProtocolAdapter.pdcNetBIOSName,
                _samrProtocolAdapter.primaryDomainFqdn,
                _samrProtocolAdapter.DomainAdministratorName,
                _samrProtocolAdapter.DomainUserPassword,
                false,
                true);
            Site.Log.Add(LogEntryKind.TestStep,
                string.Format("SamrConnect5, Server:{0}, Desired Access: READ_CONTROL.", _samrProtocolAdapter.PDCNetbiosName));
            hResult = (HRESULT)_samrProtocolAdapter.SamrConnect5(
                _samrProtocolAdapter.PDCNetbiosName,
                (uint)COMMON_ACCESS_MASK.READ_CONTROL,
                out _serverHandle);
            PtfAssert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrConnect5 must return STATUS_SUCCESS.");

            Site.Log.Add(LogEntryKind.TestStep,
                "SamrQuerySecurityObject, SecurityInformation: OWNER_SECURITY_INFORMATION.");
            _SAMPR_SR_SECURITY_DESCRIPTOR? securityDescriptor;
            hResult = (HRESULT)SAMRProtocolAdapter.RpcAdapter.SamrQuerySecurityObject(
                _serverHandle,
                SamrQuerySecurityObject_SecurityInformation_Values.OWNER_SECURITY_INFORMATION,
                out securityDescriptor);
            PtfAssert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrQuerySecurityObject must return STATUS_SUCCESS.");
            PtfAssert.IsNotNull(securityDescriptor, "The SecurityDescriptor returned by SamrQuerySecurityObject is not null.");
            _SECURITY_DESCRIPTOR sd = DtypUtility.DecodeSecurityDescriptor(securityDescriptor.Value.SecurityDescriptor);

            PtfAssert.IsNotNull(sd.OwnerSid,
                "[MS-SAMR] 3.1.5.12.2 If this bit(OWNER_SECURITY_INFORMATION) is set, the client requests that the Owner member be returned.");
            PtfAssert.IsNull(sd.GroupSid,
                "[MS-SAMR] 3.1.5.12.2 The fields(GroupSid) of the security descriptor is not returned and is set to zero.");
            PtfAssert.IsNull(sd.Sacl,
                "[MS-SAMR] 3.1.5.12.2 The fields(SACL) of the security descriptor is not returned and is set to zero.");
            PtfAssert.IsNull(sd.Dacl,
                "[MS-SAMR] 3.1.5.12.2 The fields(DACL) of the security descriptor is not returned and is set to zero.");
            PtfAssert.AreEqual(AdministratorSid, DtypUtility.ToSddlString((_SID)sd.OwnerSid),
                "[MS-SAMR] 3.1.5.12.2 The Owner and Group fields of the security descriptor MUST be the administrator's SID (S-1-5-32-544).");

        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("Server")]
        [Description("SamrQuerySecurityObject, Query the Group of PDC using SAM_SERVER_READ access.")]
        public void SamrQuerySecurityObject_Server_Group()
        {
            HRESULT hResult;
            IChecker PtfAssert = TestClassBase.BaseTestSite.Assert;
            Site.Log.Add(LogEntryKind.TestStep,
                string.Format("SamrBind, Server:{0}, Domain:{1}, User:{2}, Password{3}.",
                _samrProtocolAdapter.pdcNetBIOSName,
                _samrProtocolAdapter.primaryDomainFqdn,
                _samrProtocolAdapter.DomainAdministratorName,
                _samrProtocolAdapter.DomainUserPassword));
            _samrProtocolAdapter.SamrBind(
                _samrProtocolAdapter.pdcNetBIOSName,
                _samrProtocolAdapter.primaryDomainFqdn,
                _samrProtocolAdapter.DomainAdministratorName,
                _samrProtocolAdapter.DomainUserPassword,
                false,
                true);
            Site.Log.Add(LogEntryKind.TestStep,
                string.Format("SamrConnect5, Server:{0}, Desired Access: READ_CONTROL.", _samrProtocolAdapter.PDCNetbiosName));
            hResult = (HRESULT)_samrProtocolAdapter.SamrConnect5(
                _samrProtocolAdapter.PDCNetbiosName,
                (uint)COMMON_ACCESS_MASK.READ_CONTROL,
                out _serverHandle);
            PtfAssert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrConnect5 must return STATUS_SUCCESS.");

            Site.Log.Add(LogEntryKind.TestStep,
                "SamrQuerySecurityObject, SecurityInformation: GROUP_SECURITY_INFORMATION.");
            _SAMPR_SR_SECURITY_DESCRIPTOR? securityDescriptor;
            hResult = (HRESULT)SAMRProtocolAdapter.RpcAdapter.SamrQuerySecurityObject(
                _serverHandle,
                SamrQuerySecurityObject_SecurityInformation_Values.GROUP_SECURITY_INFORMATION,
                out securityDescriptor);
            PtfAssert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrQuerySecurityObject must return STATUS_SUCCESS.");
            PtfAssert.IsNotNull(securityDescriptor, "The SecurityDescriptor returned by SamrQuerySecurityObject is not null.");
            _SECURITY_DESCRIPTOR sd = DtypUtility.DecodeSecurityDescriptor(securityDescriptor.Value.SecurityDescriptor);

            PtfAssert.IsNotNull(sd.GroupSid,
                "[MS-SAMR] 3.1.5.12.2 If this bit(GROUP_SECURITY_INFORMATION) is set, the client requests that the Group member be returned.");
            PtfAssert.IsNull(sd.OwnerSid,
                "[MS-SAMR] 3.1.5.12.2 The field(OwnerSid) of the security descriptor is not returned and is set to zero.");
            PtfAssert.IsNull(sd.Sacl,
                "[MS-SAMR] 3.1.5.12.2 The field(SACL) of the security descriptor is not returned and is set to zero.");
            PtfAssert.IsNull(sd.Dacl,
                "[MS-SAMR] 3.1.5.12.2 The field(DACL) of the security descriptor is not returned and is set to zero.");
            PtfAssert.AreEqual(AdministratorSid, DtypUtility.ToSddlString((_SID)sd.GroupSid),
                "[MS-SAMR] 3.1.5.12.2 The Owner and Group fields of the security descriptor MUST be the administrator's SID (S-1-5-32-544).");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("Server")]
        [Description("SamrQuerySecurityObject, Query the SACL of PDC using ACCESS_SYSTEM_SECURITY access.")]
        public void SamrQuerySecurityObject_Server_SACL()
        {
            HRESULT hResult;
            IChecker PtfAssert = TestClassBase.BaseTestSite.Assert;
            Site.Log.Add(LogEntryKind.TestStep,
                string.Format("SamrBind, Server:{0}, Domain:{1}, User:{2}, Password{3}.",
                _samrProtocolAdapter.pdcNetBIOSName,
                _samrProtocolAdapter.primaryDomainFqdn,
                _samrProtocolAdapter.DomainAdministratorName,
                _samrProtocolAdapter.DomainUserPassword));
            _samrProtocolAdapter.SamrBind(
                _samrProtocolAdapter.pdcNetBIOSName,
                _samrProtocolAdapter.primaryDomainFqdn,
                _samrProtocolAdapter.DomainAdministratorName,
                _samrProtocolAdapter.DomainUserPassword,
                false,
                true);
            Site.Log.Add(LogEntryKind.TestStep,
                string.Format("SamrConnect5, Server:{0}, Desired Access: ACCESS_SYSTEM_SECURITY.", _samrProtocolAdapter.PDCNetbiosName));
            hResult = (HRESULT)_samrProtocolAdapter.SamrConnect5(
                _samrProtocolAdapter.PDCNetbiosName,
                (uint)COMMON_ACCESS_MASK.ACCESS_SYSTEM_SECURITY,
                out _serverHandle);
            PtfAssert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrConnect5 must return STATUS_SUCCESS.");

            Site.Log.Add(LogEntryKind.TestStep,
                "SamrQuerySecurityObject, SecurityInformation: SACL_SECURITY_INFORMATION.");
            _SAMPR_SR_SECURITY_DESCRIPTOR? securityDescriptor;
            hResult = (HRESULT)SAMRProtocolAdapter.RpcAdapter.SamrQuerySecurityObject(
                _serverHandle,
                SamrQuerySecurityObject_SecurityInformation_Values.SACL_SECURITY_INFORMATION,
                out securityDescriptor);
            PtfAssert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrQuerySecurityObject must return STATUS_SUCCESS.");
            PtfAssert.IsNotNull(securityDescriptor, "The SecurityDescriptor returned by SamrQuerySecurityObject is not null.");
            _SECURITY_DESCRIPTOR sd = DtypUtility.DecodeSecurityDescriptor(securityDescriptor.Value.SecurityDescriptor);

            PtfAssert.IsNotNull(sd.Sacl,
                "[MS-SAMR] 3.1.5.12.2 If this bit(SACL_SECURITY_INFORMATION) is set, the client requests that the Sacl be returned.");
            PtfAssert.IsNull(sd.OwnerSid,
                "[MS-SAMR] 3.1.5.12.2 The field(OwnerSid) of the security descriptor is not returned and is set to zero.");
            PtfAssert.IsNull(sd.GroupSid,
                "[MS-SAMR] 3.1.5.12.2 The field(GroupSid) of the security descriptor is not returned and is set to zero.");
            PtfAssert.IsNull(sd.Dacl,
                "[MS-SAMR] 3.1.5.12.2 The field(DACL) of the security descriptor is not returned and is set to zero.");
        }


        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("Server")]
        [Description("SamrQuerySecurityObject, Query the DACL of PDC using READ_CONTROL access.")]
        public void SamrQuerySecurityObject_Server_DACL()
        {
            HRESULT hResult;
            IChecker PtfAssert = TestClassBase.BaseTestSite.Assert;
            Site.Log.Add(LogEntryKind.TestStep,
                string.Format("SamrBind, Server:{0}, Domain:{1}, User:{2}, Password{3}.",
                _samrProtocolAdapter.pdcNetBIOSName,
                _samrProtocolAdapter.primaryDomainFqdn,
                _samrProtocolAdapter.DomainAdministratorName,
                _samrProtocolAdapter.DomainUserPassword));
            _samrProtocolAdapter.SamrBind(
                _samrProtocolAdapter.pdcNetBIOSName,
                _samrProtocolAdapter.primaryDomainFqdn,
                _samrProtocolAdapter.DomainAdministratorName,
                _samrProtocolAdapter.DomainUserPassword,
                false,
                true);
            Site.Log.Add(LogEntryKind.TestStep,
                string.Format("SamrConnect5, Server:{0}, Desired Access: READ_CONTROL.", _samrProtocolAdapter.PDCNetbiosName));
            hResult = (HRESULT)_samrProtocolAdapter.SamrConnect5(
                _samrProtocolAdapter.PDCNetbiosName,
                (uint)COMMON_ACCESS_MASK.READ_CONTROL,
                out _serverHandle);
            PtfAssert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrConnect5 must return STATUS_SUCCESS.");

            Site.Log.Add(LogEntryKind.TestStep,
                "SamrQuerySecurityObject, SecurityInformation: DACL_SECURITY_INFORMATION.");
            _SAMPR_SR_SECURITY_DESCRIPTOR? securityDescriptor;
            hResult = (HRESULT)SAMRProtocolAdapter.RpcAdapter.SamrQuerySecurityObject(
                _serverHandle,
                SamrQuerySecurityObject_SecurityInformation_Values.DACL_SECURITY_INFORMATION,
                out securityDescriptor);
            PtfAssert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrQuerySecurityObject must return STATUS_SUCCESS.");
            PtfAssert.IsNotNull(securityDescriptor, "The SecurityDescriptor returned by SamrQuerySecurityObject is not null.");
            _SECURITY_DESCRIPTOR sd = DtypUtility.DecodeSecurityDescriptor(securityDescriptor.Value.SecurityDescriptor);

            PtfAssert.IsNotNull(sd.Dacl,
                "[MS-SAMR] 3.1.5.12.2 If this bit(DACL_SECURITY_INFORMATION) is set, the client requests that the DACL be returned.");
            PtfAssert.IsNull(sd.OwnerSid,
                "[MS-SAMR] 3.1.5.12.2 The field(OwnerSid) of the security descriptor is not returned and is set to zero.");
            PtfAssert.IsNull(sd.GroupSid,
                "[MS-SAMR] 3.1.5.12.2 The field(GroupSid) of the security descriptor is not returned and is set to zero.");
            PtfAssert.IsNull(sd.Sacl,
                "[MS-SAMR] 3.1.5.12.2 The field(SACL) of the security descriptor is not returned and is set to zero.");

            Site.Log.Add(LogEntryKind.TestStep,
                "Verifies that the DACL returned from the server contains the following ACEs: WorldSid(SAM_SERVER_EXECUTE | SAM_SERVER_READ), AdministratorSid(SAM_SERVER_ALL_ACCESS");
            bool worldSidFound = false;
            uint worldSidMask = 0;
            bool adminSidFound = false;
            uint adminSidMask = 0;
            foreach (var o in sd.Dacl.Value.Aces)
            {
                if (!(o is Microsoft.Protocols.TestTools.StackSdk.Dtyp._ACCESS_ALLOWED_ACE)) continue;
                Microsoft.Protocols.TestTools.StackSdk.Dtyp._ACCESS_ALLOWED_ACE ace = (Microsoft.Protocols.TestTools.StackSdk.Dtyp._ACCESS_ALLOWED_ACE)o;
                switch (DtypUtility.ToSddlString(ace.Sid))
                {
                    case AdministratorSid:
                        adminSidFound = true;
                        adminSidMask = ace.Mask;
                        break;
                    case WorldSid:
                        worldSidFound = true;
                        worldSidMask = ace.Mask;
                        break;
                }

            }
            PtfAssert.IsTrue(worldSidFound,
                "[MS-SAMR] 3.1.5.12.2 If ObjectHandle.Object refers to the server object, the DACL MUST contain the following ACE. WorldSid");
            PtfAssert.AreEqual(SERVER_ACCESS_MASK.SAM_SERVER_EXECUTE | SERVER_ACCESS_MASK.SAM_SERVER_READ, (SERVER_ACCESS_MASK)worldSidMask,
                             "The access mask of WorldSid is SAM_SERVER_EXECUTE|SAM_SERVER_READ.");
            PtfAssert.IsTrue(adminSidFound,
                "[MS-SAMR] 3.1.5.12.2 If ObjectHandle.Object refers to the server object, the DACL MUST contain the following ACE. AdministratorSid");
            PtfAssert.AreEqual(SERVER_ACCESS_MASK.SAM_SERVER_ALL_ACCESS, (SERVER_ACCESS_MASK)adminSidMask,
                 "The access mask of AdministratorSid is SAM_SERVER_ALL_ACCESS.");
        
        }
    }
}
