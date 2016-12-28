// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Security.Samr;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.DirectoryServices;

using System.Security.AccessControl;
using System.Security.Principal;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using System.Linq;

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
        [TestCategory("SAMR-Server")]
        [Description("Non-DC Test: Calls SamrConnect with administrator account of DM server and SAM_SERVER_READ access. Expects a successful return.")]
        public void SamrConnect_SUCCESS_NonDC()
        {
            IChecker PtfAssert = TestClassBase.BaseTestSite.Assert;
            Site.Log.Add(LogEntryKind.TestStep,
                string.Format("SamrBind, Server:{0}, Domain:{1}, User:{2}, Password{3}.",
                _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.domainMemberNetBIOSName,
                _samrProtocolAdapter.DMAdminName,
                _samrProtocolAdapter.DMAdminPassword));
            _samrProtocolAdapter.SamrBind(
                _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.domainMemberNetBIOSName,
                _samrProtocolAdapter.DMAdminName,
                _samrProtocolAdapter.DMAdminPassword,
                false,
                false);

            Site.Log.Add(LogEntryKind.TestStep,
                string.Format("SamrConnect, Server:{0}, DesiredAccess: SAM_SERVER_READ.",
                _samrProtocolAdapter.domainMemberFqdn));

            HRESULT methodStatus = (HRESULT)SAMRProtocolAdapter.RpcAdapter.SamrConnect(
                _samrProtocolAdapter.domainMemberFqdn,
                out _serverHandle,
                (uint)SERVER_ACCESS_MASK.SAM_SERVER_READ);
            PtfAssert.AreEqual(HRESULT.STATUS_SUCCESS, methodStatus, "SamrConnect returns STATUS_SUCCESS");
            PtfAssert.AreNotEqual(IntPtr.Zero, _serverHandle, "SamrConnect returns a non-NULL handle.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Server")]
        [Description("Non-DC Test: Calls SamrConnect2 with administrator account of DM server and SAM_SERVER_READ access. Expects a successful return.")]
        public void SamrConnect2_SUCCESS_NonDC()
        {
            IChecker PtfAssert = TestClassBase.BaseTestSite.Assert;
            Site.Log.Add(LogEntryKind.TestStep,
               string.Format("SamrBind, Server:{0}, Domain:{1}, User:{2}, Password{3}.",
               _samrProtocolAdapter.domainMemberFqdn,
               _samrProtocolAdapter.domainMemberNetBIOSName,
               _samrProtocolAdapter.DMAdminName,
               _samrProtocolAdapter.DMAdminPassword));
            _samrProtocolAdapter.SamrBind(
                _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.domainMemberNetBIOSName,
                _samrProtocolAdapter.DMAdminName,
                _samrProtocolAdapter.DMAdminPassword,
                false,
                false);

            Site.Log.Add(LogEntryKind.TestStep,
                string.Format("SamrConnect, Server:{0}, DesiredAccess: SAM_SERVER_READ.",
                _samrProtocolAdapter.domainMemberFqdn));

            HRESULT methodStatus = (HRESULT)SAMRProtocolAdapter.RpcAdapter.SamrConnect2(
                _samrProtocolAdapter.domainMemberFqdn,
                out _serverHandle,
                (uint)SERVER_ACCESS_MASK.SAM_SERVER_READ);
            PtfAssert.AreEqual(HRESULT.STATUS_SUCCESS, methodStatus, "SamrConnect2 returns STATUS_SUCCESS");
            PtfAssert.AreNotEqual(IntPtr.Zero, _serverHandle, "SamrConnect2 returns a non-NULL handle.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Server")]
        [Description("Non-DC Test: Calls SamrConnect4 with administrator account of DM server and SAM_SERVER_READ access. Expects a successful return.")]
        public void SamrConnect4_SUCCESS_NonDC()
        {
            IChecker PtfAssert = TestClassBase.BaseTestSite.Assert;
            Site.Log.Add(LogEntryKind.TestStep,
               string.Format("SamrBind, Server:{0}, Domain:{1}, User:{2}, Password{3}.",
               _samrProtocolAdapter.domainMemberFqdn,
               _samrProtocolAdapter.domainMemberNetBIOSName,
               _samrProtocolAdapter.DMAdminName,
               _samrProtocolAdapter.DMAdminPassword));
            _samrProtocolAdapter.SamrBind(
                _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.domainMemberNetBIOSName,
                _samrProtocolAdapter.DMAdminName,
                _samrProtocolAdapter.DMAdminPassword,
                false,
                false);

            Site.Log.Add(LogEntryKind.TestStep,
                string.Format("SamrConnect, Server:{0}, DesiredAccess: SAM_SERVER_READ.",
                _samrProtocolAdapter.domainMemberFqdn));

            HRESULT methodStatus = (HRESULT)SAMRProtocolAdapter.RpcAdapter.SamrConnect4(
                _samrProtocolAdapter.domainMemberFqdn,
                out _serverHandle,
                0x02u,
                (uint)SERVER_ACCESS_MASK.SAM_SERVER_READ);
            PtfAssert.AreEqual(HRESULT.STATUS_SUCCESS, methodStatus, "SamrConnect4 returns STATUS_SUCCESS");
            PtfAssert.AreNotEqual(IntPtr.Zero, _serverHandle, "SamrConnect4 returns a non-NULL handle.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Server")]
        [Description("Non-DC Test: Calls SamrConnect5 with administrator account of DM server and SAM_SERVER_READ access. Expects a successful return.")]
        public void SamrConnect5_SUCCESS_NonDC()
        {
            IChecker PtfAssert = TestClassBase.BaseTestSite.Assert;
            Site.Log.Add(LogEntryKind.TestStep,
                string.Format("SamrBind, Server:{0}, Domain:{1}, User:{2}, Password{3}.",
                _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.domainMemberNetBIOSName,
                _samrProtocolAdapter.DMAdminName,
                _samrProtocolAdapter.DMAdminPassword));
            _samrProtocolAdapter.SamrBind(
                _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.domainMemberNetBIOSName,
                _samrProtocolAdapter.DMAdminName,
                _samrProtocolAdapter.DMAdminPassword,
                false,
                false);

            Site.Log.Add(LogEntryKind.TestStep,
                string.Format("SamrConnect, Server:{0}, DesiredAccess: SAM_SERVER_READ.",
                _samrProtocolAdapter.domainMemberFqdn));

            SAMPR_REVISION_INFO[] inRevisionInfo = new SAMPR_REVISION_INFO[1];
            inRevisionInfo[0] = new SAMPR_REVISION_INFO();
            inRevisionInfo[0].V1.Revision = _SAMPR_REVISION_INFO_V1_Revision_Values.V3;
            inRevisionInfo[0].V1.SupportedFeatures = SupportedFeatures_Values.V1;

            uint outVersion;
            SAMPR_REVISION_INFO outRevisionInfo;
            HRESULT methodStatus = (HRESULT)SAMRProtocolAdapter.RpcAdapter.SamrConnect5(
                _samrProtocolAdapter.domainMemberFqdn,
                (uint)SERVER_ACCESS_MASK.SAM_SERVER_READ,
                0x01u,
                inRevisionInfo[0],
                out outVersion,
                out outRevisionInfo,
                out _serverHandle);
            PtfAssert.AreEqual(HRESULT.STATUS_SUCCESS, methodStatus, "[MS-SAMR] 3.1.5.1.1 Otherwise, the server MUST return STATUS_SUCCESS.");
            PtfAssert.AreEqual(1u, outVersion,
                "[MS-SAMR] 3.1.5.1.1 The server MUST set OutVersion to 1 and OutRevisionInfo.Revision to 3.");
            PtfAssert.AreEqual(3u, (uint)outRevisionInfo.V1.Revision,
                "[MS-SAMR] 3.1.5.1.1 The server MUST set OutVersion to 1 and OutRevisionInfo.Revision to 3.");
            PtfAssert.AreEqual(0u, (uint)outRevisionInfo.V1.SupportedFeatures, "[MS-SAMR] 3.1.5.1.1 The remaining fields of OutRevisionInfo MUST be set to zero.");
            PtfAssert.AreNotEqual(IntPtr.Zero, _serverHandle, "SamrConnect5 returns a non-NULL handle.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Server")]
        [Description("Non-DC Test: Calls SamrCloseHandle to close a server handle. Expects a successful return.")]
        public void SamrCloseHandle_Server_NonDC()
        {
            IChecker PtfAssert = TestClassBase.BaseTestSite.Assert;
            Site.Log.Add(LogEntryKind.TestStep,
               string.Format("SamrBind, Server:{0}, Domain:{1}, User:{2}, Password{3}.",
               _samrProtocolAdapter.domainMemberFqdn,
               _samrProtocolAdapter.domainMemberNetBIOSName,
               _samrProtocolAdapter.DMAdminName,
               _samrProtocolAdapter.DMAdminPassword));
            _samrProtocolAdapter.SamrBind(
                _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.domainMemberNetBIOSName,
                _samrProtocolAdapter.DMAdminName,
                _samrProtocolAdapter.DMAdminPassword,
                false,
                false);

            Site.Log.Add(LogEntryKind.TestStep,
                string.Format("SamrConnect, Server:{0}, DesiredAccess: SAM_SERVER_READ.",
                _samrProtocolAdapter.domainMemberFqdn));

            HRESULT hResult = (HRESULT)SAMRProtocolAdapter.RpcAdapter.SamrConnect(
                _samrProtocolAdapter.domainMemberFqdn,
                out _serverHandle,
                (uint)SERVER_ACCESS_MASK.SAM_SERVER_READ);
            PtfAssert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrConnect returns STATUS_SUCCESS");
            Site.Log.Add(LogEntryKind.TestStep, "SamrCloseHandle");
            hResult = (HRESULT)SAMRProtocolAdapter.RpcAdapter.SamrCloseHandle(
                ref _serverHandle);
            PtfAssert.AreEqual<IntPtr>(IntPtr.Zero, _serverHandle, "[MS-SAMR] 3.1.5.13.1 ... MUST return 0 for the value SamHandle.");
            PtfAssert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrCloseHandle returns STATUS_SUCCESS");

        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Server")]
        [Description("Non-DC Test: Calls SamrLookupDomainInSamServer to find the buildin domain in the DM server and expects a successful return.")]
        public void SamrLookupDomainInSamServer_NonDC()
        {
            HRESULT hResult;
            IChecker PtfAssert = TestClassBase.BaseTestSite.Assert;
            Site.Log.Add(LogEntryKind.TestStep,
                string.Format("SamrBind, Server:{0}, Domain:{1}, User:{2}, Password{3}.",
                _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.domainMemberNetBIOSName,
                _samrProtocolAdapter.DMAdminName,
                _samrProtocolAdapter.DMAdminPassword));
            _samrProtocolAdapter.SamrBind(
                _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.domainMemberNetBIOSName,
                _samrProtocolAdapter.DMAdminName,
                _samrProtocolAdapter.DMAdminPassword,
                false,
                false);
            Site.Log.Add(LogEntryKind.TestStep,
                string.Format("SamrConnect5, Server:{0}, Desired Access: SAM_SERVER_LOOKUP_DOMAIN.", _samrProtocolAdapter.domainMemberFqdn));
            hResult = (HRESULT)_samrProtocolAdapter.SamrConnect5(
                _samrProtocolAdapter.domainMemberFqdn,
                (uint)SERVER_ACCESS_MASK.SAM_SERVER_LOOKUP_DOMAIN,
                out _serverHandle);
            PtfAssert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrConnect5 must return STATUS_SUCCESS.");

            Site.Log.Add(LogEntryKind.TestStep,
                string.Format("SamrLookupDomainInSamServer, Name: {0}.", _samrProtocolAdapter.domainMemberNetBIOSName));
            _RPC_SID? domainID;
            hResult = (HRESULT)SAMRProtocolAdapter.RpcAdapter.SamrLookupDomainInSamServer(
                _serverHandle,
                DtypUtility.ToRpcUnicodeString("Builtin"),
                out domainID);
            PtfAssert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrLookupDomainInSamServer must return STATUS_SUCCESS.");
            PtfAssert.IsNotNull(domainID, "DomainId is not null.");
            string domainSid = DtypUtility.ToSddlString(domainID.Value);
            PtfAssert.AreEqual(
                "S-1-5-32",
                domainSid,
                "The objectSid of the builtin domain must be returend.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Server")]
        [Description("Non-DC Test: Calls SamrEnumerateDomainsInSamServer and expects a successful return.")]
        public void SamrEnumerateDomainsInSamServer_SUCCESS_NonDC()
        {
            HRESULT hResult;
            IChecker PtfAssert = TestClassBase.BaseTestSite.Assert;
            Site.Log.Add(LogEntryKind.TestStep,
                string.Format("SamrBind, Server:{0}, Domain:{1}, User:{2}, Password{3}.",
                _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.domainMemberNetBIOSName,
                _samrProtocolAdapter.DMAdminName,
                _samrProtocolAdapter.DMAdminPassword));
            _samrProtocolAdapter.SamrBind(
                _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.domainMemberNetBIOSName,
                _samrProtocolAdapter.DMAdminName,
                _samrProtocolAdapter.DMAdminPassword,
                false,
                false);
            Site.Log.Add(LogEntryKind.TestStep,
                string.Format("SamrConnect5, Server:{0}, Desired Access: SAM_SERVER_ENUMERATE_DOMAINS.", _samrProtocolAdapter.domainMemberFqdn));
            hResult = (HRESULT)_samrProtocolAdapter.SamrConnect5(
                _samrProtocolAdapter.domainMemberFqdn,
                (uint)SERVER_ACCESS_MASK.SAM_SERVER_ENUMERATE_DOMAINS,
                out _serverHandle);
            PtfAssert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrConnect5 must return STATUS_SUCCESS.");
            uint? enumerationContext = 0;
            uint countReturned;
            _SAMPR_ENUMERATION_BUFFER? enumerationBuffer;
            Site.Log.Add(LogEntryKind.TestStep, "SamrEnumerateDomainsInSamServer, PreferedMaximumLength: 1024.");
            hResult = (HRESULT)SAMRProtocolAdapter.RpcAdapter.SamrEnumerateDomainsInSamServer(
                _serverHandle,
                ref enumerationContext,
                out enumerationBuffer,
                1024,
                out countReturned);
            PtfAssert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrEnumerateDomainsInSamServer must return STATUS_SUCCESS.");
            PtfAssert.AreNotEqual<uint>(0, countReturned, "The CountReturned is not zero.");
            PtfAssert.IsNotNull(enumerationBuffer, "EnumerationBuffer is not null.");
            PtfAssert.AreEqual<uint>(countReturned, enumerationBuffer.Value.EntriesRead, "Verify the EntriesRead property.");

            bool builtInDomainFound = false;
            foreach (var entry in enumerationBuffer.Value.Buffer)
            {
                string name = DtypUtility.ToString(entry.Name);
                if (string.Compare(name, "BUILTIN", true) == 0) builtInDomainFound = true;
                PtfAssert.AreEqual<uint>(0, entry.RelativeId, "[MS-SAMR]3.1.5.2.1 Buffer.Buffer.RelativeId is 0.");
            }
            PtfAssert.IsTrue(builtInDomainFound,
                "Client obtains a listing, without duplicates, of the name attribute of the built-in domain object.");
        }

        //[TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Server")]
        [Description("Non-DC Test: SamrSetSecurityObject, Sets the Owner of DM using WRITE_OWNER access.")]
        public void SamrSetSecurityObject_Server_Owner_NonDC()
        {
            HRESULT hResult;
            IChecker PtfAssert = TestClassBase.BaseTestSite.Assert;
            Site.Log.Add(LogEntryKind.TestStep,
                string.Format("SamrBind, Server:{0}, Domain:{1}, User:{2}, Password{3}.",
                _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.domainMemberNetBIOSName,
                _samrProtocolAdapter.DMAdminName,
                _samrProtocolAdapter.DMAdminPassword));
            _samrProtocolAdapter.SamrBind(
                _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.domainMemberNetBIOSName,
                _samrProtocolAdapter.DMAdminName,
                _samrProtocolAdapter.DMAdminPassword,
                false,
                false);
            Site.Log.Add(LogEntryKind.TestStep,
                string.Format("SamrConnect5, Server:{0}, Desired Access: WRITE_OWNER.", _samrProtocolAdapter.domainMemberFqdn));
            hResult = (HRESULT)_samrProtocolAdapter.SamrConnect5(
                _samrProtocolAdapter.domainMemberFqdn,
                (uint)Common_ACCESS_MASK.WRITE_OWNER,
                out _serverHandle);
            PtfAssert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrConnect5 must return STATUS_SUCCESS.");
            CommonSecurityDescriptor commonsd = new CommonSecurityDescriptor(false, true, "O:BAG:BA");
            byte[] buffer = new byte[commonsd.BinaryLength];
            commonsd.GetBinaryForm(buffer, 0);
            _SAMPR_SR_SECURITY_DESCRIPTOR sd = new _SAMPR_SR_SECURITY_DESCRIPTOR()
            {
                SecurityDescriptor = buffer,
                Length = (uint)buffer.Length
            };
            Site.Log.Add(LogEntryKind.TestStep,
                "SamrSetSecurityObject, SecurityInformation: OWNER_SECURITY_INFORMATION.");
            hResult = (HRESULT)SAMRProtocolAdapter.RpcAdapter.SamrSetSecurityObject(
                _serverHandle,
                SecurityInformation_Values.OWNER_SECURITY_INFORMATION,
                sd
                );
            PtfAssert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrSetSecurityObject must return STATUS_SUCCESS.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Server")]
        [Description("Non-DC Test: SamrQuerySecurityObject, Query the Owner of DM using SAM_SERVER_READ access.")]
        public void SamrQuerySecurityObject_Server_Owner_NonDC()
        {
            HRESULT hResult;
            IChecker PtfAssert = TestClassBase.BaseTestSite.Assert;
            Site.Log.Add(LogEntryKind.TestStep,
                string.Format("SamrBind, Server:{0}, Domain:{1}, User:{2}, Password{3}.",
                _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.domainMemberNetBIOSName,
                _samrProtocolAdapter.DMAdminName,
                _samrProtocolAdapter.DMAdminPassword));
            _samrProtocolAdapter.SamrBind(
                _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.domainMemberNetBIOSName,
                _samrProtocolAdapter.DMAdminName,
                _samrProtocolAdapter.DMAdminPassword,
                false,
                false);
            Site.Log.Add(LogEntryKind.TestStep,
                string.Format("SamrConnect5, Server:{0}, Desired Access: READ_CONTROL.", _samrProtocolAdapter.domainMemberFqdn));
            hResult = (HRESULT)_samrProtocolAdapter.SamrConnect5(
                _samrProtocolAdapter.domainMemberFqdn,
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
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Server")]
        [Description("Non-DC Test: SamrQuerySecurityObject, Query the Group of DM using SAM_SERVER_READ access.")]
        public void SamrQuerySecurityObject_Server_Group_NonDC()
        {
            HRESULT hResult;
            IChecker PtfAssert = TestClassBase.BaseTestSite.Assert;
            Site.Log.Add(LogEntryKind.TestStep,
                string.Format("SamrBind, Server:{0}, Domain:{1}, User:{2}, Password{3}.",
                _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.domainMemberNetBIOSName,
                _samrProtocolAdapter.DMAdminName,
                _samrProtocolAdapter.DMAdminPassword));
            _samrProtocolAdapter.SamrBind(
                _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.domainMemberNetBIOSName,
                _samrProtocolAdapter.DMAdminName,
                _samrProtocolAdapter.DMAdminPassword,
                false,
                false);
            Site.Log.Add(LogEntryKind.TestStep,
                string.Format("SamrConnect5, Server:{0}, Desired Access: READ_CONTROL.", _samrProtocolAdapter.domainMemberFqdn));
            hResult = (HRESULT)_samrProtocolAdapter.SamrConnect5(
                _samrProtocolAdapter.domainMemberFqdn,
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
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Server")]
        [Description("Non-DC Test: SamrQuerySecurityObject, Query the SACL of DM using ACCESS_SYSTEM_SECURITY access.")]
        public void SamrQuerySecurityObject_Server_SACL_NonDC()
        {
            HRESULT hResult;
            IChecker PtfAssert = TestClassBase.BaseTestSite.Assert;
            Site.Log.Add(LogEntryKind.TestStep,
                string.Format("SamrBind, Server:{0}, Domain:{1}, User:{2}, Password{3}.",
                _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.domainMemberNetBIOSName,
                _samrProtocolAdapter.DMAdminName,
                _samrProtocolAdapter.DMAdminPassword));
            _samrProtocolAdapter.SamrBind(
                _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.domainMemberNetBIOSName,
                _samrProtocolAdapter.DMAdminName,
                _samrProtocolAdapter.DMAdminPassword,
                false,
                false);
            Site.Log.Add(LogEntryKind.TestStep,
                string.Format("SamrConnect5, Server:{0}, Desired Access: ACCESS_SYSTEM_SECURITY.", _samrProtocolAdapter.domainMemberFqdn));
            hResult = (HRESULT)_samrProtocolAdapter.SamrConnect5(
                _samrProtocolAdapter.domainMemberFqdn,
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
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Server")]
        [Description("Non-DC Test: SamrQuerySecurityObject, Query the DACL of DM using READ_CONTROL access.")]
        public void SamrQuerySecurityObject_Server_DACL_NonDC()
        {
            HRESULT hResult;
            IChecker PtfAssert = TestClassBase.BaseTestSite.Assert;
            Site.Log.Add(LogEntryKind.TestStep,
                string.Format("SamrBind, Server:{0}, Domain:{1}, User:{2}, Password{3}.",
                _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.domainMemberNetBIOSName,
                _samrProtocolAdapter.DMAdminName,
                _samrProtocolAdapter.DMAdminPassword));
            _samrProtocolAdapter.SamrBind(
                _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.domainMemberNetBIOSName,
                _samrProtocolAdapter.DMAdminName,
                _samrProtocolAdapter.DMAdminPassword,
                false,
                false);
            Site.Log.Add(LogEntryKind.TestStep,
                string.Format("SamrConnect5, Server:{0}, Desired Access: READ_CONTROL.", _samrProtocolAdapter.domainMemberFqdn));
            hResult = (HRESULT)_samrProtocolAdapter.SamrConnect5(
                _samrProtocolAdapter.domainMemberFqdn,
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
