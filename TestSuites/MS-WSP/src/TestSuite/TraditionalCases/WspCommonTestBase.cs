// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP;

namespace Microsoft.Protocols.TestSuites.WspTS
{
    public abstract partial class WspCommonTestBase : TestClassBase
    {
        protected void EnsureSuccessfulCPMConnectOut(uint errorCode)
        {
            Site.Assert.AreEqual((uint)WspErrorCode.SUCCESS, errorCode, "CPMConnectIn should succeed.");
        }

        protected void EnsureSuccessfulCPMCreateQueryOut(uint errorCode)
        {
            Site.Assert.AreEqual((uint)WspErrorCode.SUCCESS, errorCode, "CPMCreateQueryIn should succeed.");
        }

        protected void EnsureSuccessfulCPMSetBindingsOut(uint errorCode)
        {
            Site.Assert.AreEqual((uint)WspErrorCode.SUCCESS, errorCode, "CPMSetBindingsIn should succeed.");
        }

        protected void EnsureSuccessfulCPMGetRowsOut(uint errorCode)
        {
            bool succeed = errorCode == (uint)WspErrorCode.SUCCESS || errorCode == (uint)WspErrorCode.DB_S_ENDOFROWSET ? true : false;
            Site.Assert.IsTrue(succeed, "Server should return SUCCESS or DB_S_ENDOFROWSET for CPMGetRowsIn.");
        }

        protected void EnsureSuccessfulCPMFreeCursorOut(uint errorCode)
        {
            Site.Assert.AreEqual((uint)WspErrorCode.SUCCESS, errorCode, "CPMFreeCursorIn should succeed.");
        }

        protected void EnsureSuccessfulCPMGetQueryStatusOut(uint errorCode)
        {
            Site.Assert.AreEqual((uint)WspErrorCode.SUCCESS, errorCode, "CPMGetQueryStatusIn should succeed.");
        }

        protected void EnsureSuccessfulCPMGetQueryStatusExOut(uint errorCode)
        {
            Site.Assert.AreEqual((uint)WspErrorCode.SUCCESS, errorCode, "CPMGetQueryStatusExIn should succeed.");
        }

        protected void EnsureSuccessfulCPMRestartPositionIn(uint errorCode)
        {
            Site.Assert.AreEqual((uint)WspErrorCode.SUCCESS, errorCode, "CPMRestartPositionIn should succeed.");
        }
    }
}
