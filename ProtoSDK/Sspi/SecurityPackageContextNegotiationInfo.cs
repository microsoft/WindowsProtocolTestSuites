// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Sspi
{
    /// <summary>
    /// The SecPkgContext_NegotiationInfo structure contains information on the security package that is being set up 
    /// or has been set up, and also gives the status on the negotiation to set up the security package.
    /// http://msdn.microsoft.com/en-us/library/aa380091(v=VS.85).aspx
    /// </summary>
    public struct SecurityPackageContextNegotiationInfo
    {
        /// <summary>
        /// A SecPkgInfo structure that provides general information about the security package chosen in
        /// the negotiate process, such as the name and capabilities of the package.
        /// </summary>
        public SecurityPackageInformation PackageInfo;

        /// <summary>
        /// Indicator of the state of the negotiation for the security package identified in the PackageInfo member.
        /// This attribute can be queried from the context handle before the setup is complete, such as when ISC 
        /// returns SEC_I_CONTINUE_NEEDED.
        /// </summary>
        public uint NegotiationState;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="negotiationInfo">NegotiationInfo defined by SSPI</param>
        internal SecurityPackageContextNegotiationInfo(SspiSecurityPackageContextNegotiationInfo negotiationInfo)
        {
            SspiSecurityPackageInformation sspiNegotiationInfo;

            sspiNegotiationInfo = (SspiSecurityPackageInformation)Marshal.PtrToStructure(
                negotiationInfo.PackageInfo,
                typeof(SspiSecurityPackageInformation));
            this.PackageInfo = new SecurityPackageInformation(sspiNegotiationInfo);
            this.NegotiationState = negotiationInfo.NegotiationState;
        }
    }
}
