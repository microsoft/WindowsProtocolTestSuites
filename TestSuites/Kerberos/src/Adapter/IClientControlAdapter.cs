// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocol.TestSuites.Kerberos.Adapter
{
    public interface IClientControlAdapter : IAdapter
    {
        /// <summary>
        /// This method is to uncheck "The other domain supports Kerberos AES Encryption" option on local realm.
        /// </summary>
        [MethodHelp("This method is to uncheck \"The other domain supports Kerberos AES Encryption\" option on local realm.")]
        void ClearTrustRealmEncType();

        /// <summary>
        /// This method is to recheck "The other domain supports Kerberos AES Encryption" option on local realm.
        /// </summary>
        [MethodHelp("This method is to recheck \"The other domain supports Kerberos AES Encryption\" option on local realm.")]
        void SetTrustRealmEncTypeAsAes();

        /// <summary>
        /// This method is to set the SupportedEncryptionTypes of the remote computer to use RC4-HMAC.
        /// </summary>
        [MethodHelp("This method is to set the SupportedEncryptionTypes of the remote computer to use RC4-HMAC.")]
        void SetSupportedEncryptionTypesAsRc4(string remoteComputerName, string remoteUsername, string remotePassword);
       
        /// <summary>
        /// This method is to restore the SupportedEncryptionTypes settings of the remote computer.
        /// </summary>
        [MethodHelp("This method is to restore the SupportedEncryptionTypes settings of the remote computer.")]
        void RestoreSupportedEncryptionTypes();

        /// <summary>
        /// This method is to Enable Compound Identity.
        /// </summary>
        [MethodHelp("This method is to Enable Compound Identity.")]
        void EnableCompoundIdentity();
    }
}
