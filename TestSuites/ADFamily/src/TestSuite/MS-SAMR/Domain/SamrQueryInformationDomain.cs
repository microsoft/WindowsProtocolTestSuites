// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
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
        [TestCategory("Domain")]
        [TestCategory("DC")]
        [TestCategory("BVT")]
        [Description("This is to test SamrQueryInformationDomain with DomainGeneralInformation.")]
        public void SamrQueryInformationDomain_GeneralInformation()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName,
                out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationDomain: obtain attributes from a domain object.");
            _SAMPR_DOMAIN_INFO_BUFFER? domainInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationDomain(_domainHandle, _DOMAIN_INFORMATION_CLASS.DomainGeneralInformation, out domainInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationDomain returns:{0}.", result);
            Site.Assert.IsNotNull(domainInfo, "The returned domainInfo should not be null.");

            _samrProtocolAdapter.VerifyDomainGeneralInformationFields(domainInfo.Value.General);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("Domain")]
        [TestCategory("DC")]
        [TestCategory("BVT")]
        [Description("This is to test SamrQueryInformationDomain with DomainServerRoleInformation.")]
        public void SamrQueryInformationDomain_ServerRoleInformation()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName,
                out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationDomain: obtain attributes from a domain object.");
            _SAMPR_DOMAIN_INFO_BUFFER? domainInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationDomain(_domainHandle, _DOMAIN_INFORMATION_CLASS.DomainServerRoleInformation, out domainInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationDomain returns:{0}.", result);
            Site.Assert.IsNotNull(domainInfo, "The returned domainInfo should not be null.");

            _samrProtocolAdapter.VerifyDomainServerRoleInformationFields(domainInfo.Value.Role);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("Domain")]
        [TestCategory("DC")]
        [TestCategory("BVT")]
        [Description("This is to test SamrQueryInformationDomain with DomainStateInformation.")]
        public void SamrQueryInformationDomain_StateInformation()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName,
                out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationDomain: obtain attributes from a domain object.");
            _SAMPR_DOMAIN_INFO_BUFFER? domainInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationDomain(_domainHandle, _DOMAIN_INFORMATION_CLASS.DomainStateInformation, out domainInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationDomain returns:{0}.", result);
            Site.Assert.IsNotNull(domainInfo, "The returned domainInfo should not be null.");

            Site.Assert.AreEqual(_DOMAIN_SERVER_ENABLE_STATE.DomainServerEnabled, domainInfo.Value.State.DomainServerState, "3.1.5.5.1.3 The server MUST set Buffer.State.DomainServerState to DomainServerEnabled.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("Domain")]
        [TestCategory("DC")]
        [TestCategory("BVT")]
        [Description("This is to test SamrQueryInformationDomain with DomainGeneralInformation2.")]
        public void SamrQueryInformationDomain_GeneralInformation2()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName,
                out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationDomain: obtain attributes from a domain object.");
            _SAMPR_DOMAIN_INFO_BUFFER? domainInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationDomain(_domainHandle, _DOMAIN_INFORMATION_CLASS.DomainGeneralInformation2, out domainInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationDomain returns:{0}.", result);
            Site.Assert.IsNotNull(domainInfo, "The returned domainInfo should not be null.");
                        
            _samrProtocolAdapter.VerifyDomainGeneralInformationFields(domainInfo.Value.General2.I1);

            _SAMPR_DOMAIN_INFO_BUFFER? lockoutInfo;
            result = _samrProtocolAdapter.SamrQueryInformationDomain(_domainHandle, _DOMAIN_INFORMATION_CLASS.DomainLockoutInformation, out lockoutInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationDomain returns:{0}.", result);
            Site.Assert.IsNotNull(lockoutInfo, "The returned domainInfo should not be null.");

            Site.Assert.AreEqual(lockoutInfo.Value.Lockout.LockoutDuration, domainInfo.Value.General2.LockoutDuration,
                "lockoutDuration: 3.1.5.5.1.4 The server MUST process this call as two calls to SamrQueryInformationDomain with the information levels of DomainGeneralInformation and DomainLockoutTime, but all in the same transaction.");
            Site.Assert.AreEqual(lockoutInfo.Value.Lockout.LockoutObservationWindow, domainInfo.Value.General2.LockoutObservationWindow,
                "lockoutObservationWindow: 3.1.5.5.1.4 The server MUST process this call as two calls to SamrQueryInformationDomain with the information levels of DomainGeneralInformation and DomainLockoutTime, but all in the same transaction.");
            Site.Assert.AreEqual(lockoutInfo.Value.Lockout.LockoutThreshold, domainInfo.Value.General2.LockoutThreshold,
                "lockoutThreshold: 3.1.5.5.1.4 The server MUST process this call as two calls to SamrQueryInformationDomain with the information levels of DomainGeneralInformation and DomainLockoutTime, but all in the same transaction.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("Domain")]
        [TestCategory("DC")]
        [TestCategory("BVT")]
        [Description("This is to test SamrQueryInformationDomain with DomainLockoutInformation.")]
        public void SamrQueryInformationDomain_LockoutInformation()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName,
                out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationDomain: obtain attributes from a domain object.");
            _SAMPR_DOMAIN_INFO_BUFFER? domainInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationDomain(_domainHandle, _DOMAIN_INFORMATION_CLASS.DomainLockoutInformation, out domainInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationDomain returns:{0}.", result);
            Site.Assert.IsNotNull(domainInfo, "The returned domainInfo should not be null.");

            _samrProtocolAdapter.VerifyDomainLockoutInformationFields(domainInfo.Value.Lockout);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("Domain")]
        [TestCategory("DC")]
        [Description("This is to test SamrQueryInformationDomain with non-domain handle.")]
        public void SamrQueryInformationDomain_NonDomainHandle()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Initialize: Create Samr Bind to the server.");
            _samrProtocolAdapter.SamrBind(
                _samrProtocolAdapter.pdcNetBIOSName,
                _samrProtocolAdapter.primaryDomainFqdn,
                _samrProtocolAdapter.DomainAdministratorName,
                _samrProtocolAdapter.DomainUserPassword,
                false,
                true);

            HRESULT methodStatus;

            Site.Log.Add(LogEntryKind.TestStep, "SamrConnect5: obtain a handle to the server:{0}.", _samrProtocolAdapter.pdcFqdn);
            methodStatus = _samrProtocolAdapter.SamrConnect5(
                _samrProtocolAdapter.pdcFqdn,
                Utilities.SAM_SERVER_ALL_ACCESS,
                out _serverHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, methodStatus, "SamrConnect5 returns:{0}.", methodStatus);
            Site.Assert.IsNotNull(_serverHandle, "The returned server handle is: {0}.", _serverHandle);
                        
            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationDomain: with a server handle.");
            _SAMPR_DOMAIN_INFO_BUFFER? domainInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationDomain(_serverHandle, _DOMAIN_INFORMATION_CLASS.DomainGeneralInformation, out domainInfo);
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "3.1.5.5.1 The server MUST return an error if DomainHandle.HandleType is not equal to 'Domain'.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("Domain")]
        [TestCategory("DC")]
        [Description("This is to test SamrQueryInformationDomain with DomainPasswordInformation with no required access.")]
        public void SamrQueryInformationDomain_PasswordInformation_NoAccess()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName,
                out _serverHandle, out _domainHandle, (uint)DOMAIN_ACCESS_MASK.DOMAIN_READ_OTHER_PARAMETERS);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationDomain: obtain password information from a domain object with no required access.");
            _SAMPR_DOMAIN_INFO_BUFFER? domainInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationDomain(_domainHandle, _DOMAIN_INFORMATION_CLASS.DomainPasswordInformation, out domainInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "3.1.5.5.1 DomainHandle.GrantedAccess MUST have the required access specified in section 3.1.2.1. Otherwise, the server MUST return STATUS_ACCESS_DENIED.");
        }
    }
}
