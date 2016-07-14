// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc
{
    /// <summary>
    /// NRPC compute session key algorithms.
    /// </summary>
    public enum NrpcComputeSessionKeyAlgorithm
    {
        /// <summary>
        /// If AES support is negotiated between the client and the server, 
        /// the strong-key support flag is ignored and the session key is 
        /// computed with the HMAC-SHA256 algorithm.
        /// </summary>
        HMACSHA256,


        /// <summary>
        /// If AES is not negotiated and strong-key support is one of the flags 
        /// in the NegotiateFlags between the client and the server, 
        /// the session key is computed with the MD5 message-digest algorithm.
        /// </summary>
        MD5,


        /// <summary>
        /// If neither AES nor strong-key support is negotiated between the client 
        /// and the server, the session key is computed by using the DES 
        /// encryption algorithm in ECB mode.
        /// </summary>
        DES
    }


    /// <summary>
    /// NRPC compute netlogon credential algorithms.
    /// </summary>
    public enum NrpcComputeNetlogonCredentialAlgorithm
    {
        /// <summary>
        /// If AES support is negotiated between the client and the server, 
        /// the Netlogon credentials are computed using the AES-128 encryption 
        /// algorithm in 8-bit CFB mode with a zero initialization vector.
        /// </summary>
        AES128,

        /// <summary>
        /// If AES is not supported, the Netlogon credentials are computed using 
        /// DES encryption algorithm in ECB mode.
        /// </summary>
        DESECB
    }


    /// <summary>
    /// NRPC signature algorithms.
    /// </summary>
    public enum NrpcSignatureAlgorithm
    {
        /// <summary>
        /// If AES is negotiated, a client generates an NL_AUTH_SHA2_SIGNATURE token 
        /// that contains an HMAC-SHA256 checksum [RFC4634], a sequence number, 
        /// and a Confounder (if confidentiality has been requested), 
        /// to send data protected on the wire. 
        /// </summary>
        HMACSHA256,

        /// <summary>
        /// If AES is not negotiated, a client generates an Netlogon Signature token 
        /// that contains an HMAC-MD5 checksum ([RFC2104]), a sequence number, 
        /// and a Confounder (if confidentiality has been requested), 
        /// to send data protected on the wire.
        /// </summary>
        HMACMD5,
    }


    /// <summary>
    /// NRPC seal algorithms.
    /// </summary>
    public enum NrpcSealAlgorithm
    {
        /// <summary>
        /// If AES is negotiated, the data is encrypted using the AES algorithm.
        /// </summary>
        AES128,

        /// <summary>
        /// If AES is not negotiated, the data is encrypted using the RC4 algorithm.
        /// </summary>
        RC4
    }
}
