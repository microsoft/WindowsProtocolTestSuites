using Microsoft.Protocols.TestSuites.Rdp;
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestSuites.Rdpemt
{
    public partial class RdpemtTestSuite : RdpTestClassBase
    {

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP10.0")]
        [TestCategory("RDPBCGR")]
        [TestCategory("StaticVirtualChannel")]
        [Description("This test case is used to verify client could tunneling static virtual channel traffic over UDP.")]
        public void S3_Tunneling_StaticVirtualChannel_PositiveTest()
        {
            this.TestSite.Assert.IsTrue(isClientSupportTunnelingStaticVCTraffic, "To execute test cases of S10, RDP client should support tunneling of static virtual channel traffic over UDP.");
            this.TestSite.Assert.IsTrue(isClientSupportRDPEFS, "To execut this case, RDP client should support [MS-RDPEFS]: Remote Desktop Protocol: File System Virtual Channel Extension.");
            EstablishTunnelingStaticVCTrafficConnection();
        }
    }

}

