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
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrQueryInformationUser with UserAllInformation.")]
        public void SamrQueryInformationUser_UserAllInformation_NonDC()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Connect and open a user handle.");
            ConnectAndOpenUser_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.DMAdminName, out _userHandle, Utilities.USER_READ_GENERAL | Utilities.USER_READ_LOGON | Utilities.USER_READ_ACCOUNT | Utilities.USER_READ_PREFERENCES);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser(_userHandle, _USER_INFORMATION_CLASS.UserAllInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser returns:{0}.", result);
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");
        }

    }
}
