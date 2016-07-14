// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc
{
    /// <summary>
    /// A set of bit flags that specify properties that MUST be true for 
    /// a domain trust to be part of the returned domain name list. 
    /// A flag is TRUE (or set) if its value is equal to 1. 
    /// Flags MUST contain one or more of the following bits.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum NrpcDsrEnumerateDomainTrustsFlags : uint
    {
        /// <summary>
        /// No flag is set.
        /// </summary>
        None = 0,

        /// <summary>
        /// A: Domain is a member of the forest.
        /// </summary>
        MemberOfTheForest = 1,

        /// <summary>
        /// B: Domain is directly trusted by this domain.
        /// </summary>
        DirectlyTrusted = 2,

        /// <summary>
        /// C: Domain is the root of a tree in the forest.
        /// </summary>
        RootOfTreeInForest = 4, 

        /// <summary>
        /// D: Domain is the primary domain of the queried server.
        /// </summary>
        PrimaryDomain = 8, 

        /// <summary>
        /// E: Primary domain is running in native mode.
        /// </summary>
        PrimaryDomainIsRunningInNativeMode = 0x10, 

        /// <summary>
        /// F: Domain is directly trusting this domain.
        /// </summary>
        DirectlyTrusting = 0x20, 

        /// <summary>
        /// G: Domain is MIT Kerberos realm, trusted with RC4 encryption.
        /// </summary>
        MitKerberosRealm = 0x80, 

        /// <summary>
        /// H: Kerberos uses AES keys to encrypt Kerberos TGTs.
        /// </summary>
        KerberosTgtsEncryptedByAesKeys = 0x100, 
    }
}
