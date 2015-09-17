// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Sspi
{
    /// <summary>
    /// The SecPkgInfo structure provides general information about a security package, such as its name 
    /// and capabilities.
    /// http://msdn.microsoft.com/en-us/library/aa380104(v=VS.85).aspx
    /// </summary>
    public struct SecurityPackageInformation
    {
        /// <summary>
        /// Set of bit flags that describes the capabilities of the security package. 
        /// </summary>
        public uint Capabilities;

        /// <summary>
        /// Specifies the version of the package protocol. Must be 1.
        /// </summary>
        public ushort Version;

        /// <summary>
        /// Specifies a DCE RPC identifier, if appropriate. If the package does not implement one of the DCE registered
        /// security systems, the reserved value SECPKG_ID_NONE is used.
        /// </summary>
        public ushort RpcId;

        /// <summary>
        /// Specifies the maximum size, in bytes, of the token.
        /// </summary>
        public uint MaxToken;

        /// <summary>
        /// Pointer to a null-terminated string that contains the name of the security package.
        /// </summary>
        public string Name;

        /// <summary>
        /// Pointer to a null-terminated string. This can be any additional string passed back by the package.
        /// </summary>
        public string Comment;

        /// <summary>
        /// Constructor. Convert SspiSecurityPackageInformation to SecurityPackageInformation.
        /// </summary>
        /// <param name="securityPackageInfo">Security package info</param>
        internal SecurityPackageInformation(SspiSecurityPackageInformation securityPackageInfo)
        {
            this.Capabilities = securityPackageInfo.fCapabilities;
            this.Version = securityPackageInfo.wVersion;
            this.RpcId = securityPackageInfo.wRpcId;
            this.MaxToken = securityPackageInfo.cbMaxToken;
            this.Name = Marshal.PtrToStringUni(securityPackageInfo.Name);
            this.Comment = Marshal.PtrToStringUni(securityPackageInfo.Comment);
        }
    }
}
