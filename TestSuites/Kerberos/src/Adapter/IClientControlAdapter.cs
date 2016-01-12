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
    }
}
