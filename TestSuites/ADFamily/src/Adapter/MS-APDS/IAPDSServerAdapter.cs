// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Security.Apds;
using Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Apds
{
    /// <summary>
    /// Message Adapter interface that is exposed to the test suite, which is used for APDS message generation
    /// from protocol client to protocol server.
    /// </summary>
    public interface IApdsServerAdapter : IAdapter
    {
        #region methods
        /// <summary>
        /// Send NTLM request message to DC, and validate the credentials.
        /// </summary>
        /// <param name="logonlevel">Indicates the logon level.</param>
        /// <param name="accountInfo">Indicates whether this account is valid in DC validation.</param>
        /// <param name="isAccessCheckSuccess">Indicates whether the access checked success.</param>
        /// <param name="validationLevel">Indicates the validation level.</param>
        /// <returns>Indicates the result status of DC response.</returns>
        [MethodHelp(" Sending NTLM Request to Protocol Server")]
        Status NTLMLogon(
            _NETLOGON_LOGON_INFO_CLASS logonlevel,
            AccountInformation accountInfo,
            Boolean isAccessCheckSuccess,
            _NETLOGON_VALIDATION_INFO_CLASS validationLevel);

        /// <summary>
        /// Send Digest request message to DC, and validate the credentials.
        /// </summary>
        /// <param name="isValidationSuccess">Indicates whether the validation from server will be success.</param>
        /// <param name="accountInfo">Indicates whether this account is valid in DC validation.</param>
        /// <param name="digestType">Indicates the DigestType field of the request.</param>
        /// <param name="algType">Indicates the AlgType field of the request.</param>
        /// <param name="ignoredFields">Indicates the field should be ignored by the server.</param>
        /// <returns>Indicates the result status of DC response.</returns>
        [MethodHelp(" Sending Digest Request to Protocol Server")]
        Status GenerateDigestRequest(
            Boolean isValidationSuccess,
            AccountInformation accountInfo,
            DigestType_Values digestType,
            AlgType_Values algType,
            IgnoredFields ignoredFields);

        #endregion
    }
}
