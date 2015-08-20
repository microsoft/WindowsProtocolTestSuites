// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Sspi
{
    /// <summary>
    /// The CREDSSP_SUBMIT_TYPE enumeration specifies the type of credentials specified by a CREDSSP_CRED structure.
    /// http://msdn.microsoft.com/en-us/library/aa965488(v=VS.85).aspx
    /// </summary>
    internal enum CredSspSubmitType
    {
        /// <summary>
        /// The credentials are a user name and password.
        /// </summary>
        CredsspPasswordCreds = 2,

        /// <summary>
        /// The credentials are Schannel credentials.
        /// </summary>
        CredsspSchannelCreds = 4,

        /// <summary>
        /// The credentials are in a certificate.
        /// </summary>
        CredsspCertificateCreds = 13,

        /// <summary>
        /// CredsspSubmitBufferBoth
        /// </summary>
        CredsspSubmitBufferBoth = 50,

        /// <summary>
        /// CredsspSubmitBufferBothOld
        /// </summary>
        CredsspSubmitBufferBothOld = 51,
    }

    /// <summary>
    /// The CREDSSP_CRED structure specifies authentication data for both Schannel and Negotiate security packages.
    /// http://msdn.microsoft.com/en-us/library/aa965581(VS.85).aspx
    /// </summary>
    internal struct CredSspCred
    {
        /// <summary>
        /// The CREDSPP_SUBMIT_TYPE enumeration value that specifies the type of credentials contained in this 
        /// structure.
        /// </summary>
        internal CredSspSubmitType Type;

        /// <summary>
        /// A pointer to a set of Schannel credentials.
        /// </summary>
        internal IntPtr pSchannelCred;

        /// <summary>
        /// A pointer to a set of Negotiate credentials.
        /// </summary>
        internal IntPtr pSpnegoCred;
    }
}
