// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocol.TestSuites.Kerberos.Adapter
{
    public interface ISutControlAdapter : IAdapter
    {
        /// <summary>
        /// This method is to help enable the compound identity feature on the computer account in the specific domain.
        /// </summary>
        /// <param name="domainName">The domain name of the service principal.</param>
        /// <param name="computerName">The host name of the service principal.</param>
        /// <param name="adminName">Need administrator's credential to modify active directory account.</param>
        /// <param name="adminPwd">Need administrator's credential to modify active directory account.</param>
        [MethodHelp("This method is to help enable the compound identity feature on the computer account in the specific domain.")]
        void enableCompId(string domainName, string computerName, string adminName, string adminPwd);

        /// <summary>
        /// This method is used to enable or disable selective authentication for an inbound trust from the localForest to the targetForest
        /// </summary>
        /// <param name="localForest">Local Forest Name</param>
        /// <param name="userName">Username for Local Forest</param>
        /// <param name="password">Password for Local Forest</param>
        /// <param name="targetForest">Target Forest Name</param>
        /// <param name="enable">true or false</param>
        [MethodHelp("This method is used to enable or disable selective authentication for an inbound trust from the localForest to the targetForest.")]
        void setSelectiveAuth(string localForest, string userName, string password, string targetForest, bool enable);

        /// <summary>
        /// This method is used to get an authentication policy TGT life time by policy name and attribute name
        /// </summary>
        /// <param name="domainName">Domain Name</param>
        /// <param name="policyname">authentication policy name</param>
        /// <param name="tgtlifetimeattributename">the lifetime attribute name, such as msds-ComputerTGTLifetime, msds-UserTGTLifetime or msds-ServiceTGTLifetime</param>
        /// <param name="adminName">Admin user Name</param>
        /// <param name="adminPwd">Admin password</param>
        [MethodHelp("This method is used to get an authentication policy TGT life time by policy name and attribute name.")]
        double? getAuthPolicyTGTLifeTime(string domainName, string policyname, string tgtlifetimeattributename, string adminName, string adminPwd);
        
        /// <summary>
        /// This method is used to get attribute display name of an account
        /// </summary>
        /// <param name="domainName">Local domain Name</param>
        /// <param name="accountname">account name, user name or computer name</param>
        /// <param name="accounttype">users, or computers</param>
        /// <param name="attributename">The attribute of account to query</param>
        /// <param name="adminName">Admin user Name</param>
        /// <param name="adminPwd">Admin password</param>
        [MethodHelp("This method is used to get attribute display name of an account.")]
        string getAccountAttributeDN(string domainName, string accountname, string accounttype, string attributename, string adminName, string adminPwd);
    }
}
