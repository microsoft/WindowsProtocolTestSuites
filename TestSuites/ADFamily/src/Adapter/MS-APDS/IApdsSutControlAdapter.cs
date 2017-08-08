// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Apds
{
    /// <summary>
    /// Server Control Adapter interface that is exposed to the test suite, it is used to modify
    /// environment of protocol server.
    /// </summary>
    public interface IApdsSutControlAdapter : IAdapter
    {
        /// <summary>
        /// Config Netlogon paramerter to allow NTLM.
        /// </summary>
        [MethodHelp("Config Netlogon paramerter to allow NTLM.")]
        void ConfigNTLMRegistryKey();

        /// <summary>
        /// Restore Netlogon paramerter for NTLM.
        /// </summary>
        [MethodHelp("Restore Netlogon paramerter for NTLM.")]
        void RestoreNTLMRegistryKey();

        /// <summary>
        /// Get DC block value for target DC.
        /// </summary>
        /// <param name="targetDCName">The name of target DC</param>
        /// <returns>The default value for target DC block setting.</returns>
        [MethodHelp("Get DC block value from registry for target DC.")]
        int GetDCBlockValue(string targetDCName);

        /// <summary>
        /// Get block exception server form target DC.
        /// </summary>
        /// <param name="targetDCName">The name of target DC</param>
        /// <returns>The default string of block exception servers.</returns>
        [MethodHelp("Get block exception server form target DC.")]
        string GetDCException(string targetDCName);

        /// <summary>
        /// Set DC blocker value for target DC.
        /// </summary>
        /// <param name="blockKey">The value for target DC block setting.</param>
        /// <param name="targetDCName">The name of target DC</param>
        [MethodHelp("Set DC blocker value in registry for target DC.")]
        void SetDCBlockValue(int blockKey, string targetDCName);

        /// <summary>
        /// Set block exception server form target DC.
        /// </summary>
        /// <param name="exceptionServer">The name of block exception servers.</param>
        /// <param name="targetDCName">The name of target DC</param>
        [MethodHelp("Set block exception server form target DC.")]
        void SetDCException(string exceptionServer, string targetDCName);

        /// <summary>
        /// Sets the UserAllowedToAuthenticateFrom value on the target DC.
        /// This value is set on the trust DC and applies to our test user.
        /// </summary>
        /// <param name="restrictedPrinciple">
        /// The computer which will be restricted to be authenticated from. 
        /// Pass null to clear the A2AF setting.
        /// This value restricts the computer from which the test user i.e. 
        /// trust\apdsuser is allowed to logon.
        /// </param>
        [MethodHelp("Set the UserAllowedToAuthenticateFrom value on the target DC.")]
        void SetA2AF(string restrictedPrinciple);

        /// <summary>
        /// Sets the ComputerAllowedToAuthenticateTo value on the target DC.
        /// This value is set on this domain DC and applies to the driver 
        /// computer.
        /// </summary>
        /// <param name="restrictedPrinciple">
        /// The user allowed to authenticate to the computer. Pass null to 
        /// clear the A2A2 setting.
        /// This value sets the user who will be blocked from authenticating
        /// to the current computer i.e. Endpoint01.
        /// </param>
        /// <remarks>
        /// The value must be set on ComputerAllowedToAuthenticateTo, not 
        /// UserAllowedToAuthenticateTo; or it won't work.
        /// </remarks>
        [MethodHelp("Set the ComputerAllowedToAuthenticateTo value on the target DC.")]
        void SetA2A2(string restrictedPrinciple);

        /// <summary>
        /// Set the user to be a member of Protected Users Group."
        /// </summary>
        /// <param name="userName">
        /// The user to be added to the Protected Users Group. Pass null
        /// to clear the Protected Users Group members.
        /// </param>
        [MethodHelp("Set the user to be a member of Protected Users Group.")]
        void SetProtectedUser(string userName);

        /// <summary>
        /// Sets the UserAllowedToAuthenticateFrom value on the target DC.
        /// This value is set on the primary DC and applies to our managed service account.
        /// </summary>
        /// <param name="restrictedPrinciple">
        /// The computer which will be restricted to be authenticated from. 
        /// Pass null to clear the A2AF setting.
        /// This value restricts the computer from which the managed service account i.e. 
        /// contoso\msa01 is allowed to logon.
        /// </param>
        [MethodHelp("Set the UserAllowedToAuthenticateFrom value on the target DC.")]
        void SetA2AFServiceAccount(string restrictedPrinciple);
    }
}
