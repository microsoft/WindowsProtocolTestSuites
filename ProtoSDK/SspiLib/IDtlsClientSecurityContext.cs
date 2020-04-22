// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib
{
    public interface IDtlsClientSecurityContext : IDisposable
    {
        bool HasMoreFragments { get; }

        SecurityPackageContextStreamSizes StreamSizes { get; }

        bool NeedContinueProcessing { get; }

        /// <summary>
        /// Gets or sets sequence number for Verify, Encrypt and Decrypt message.
        /// For Digest SSP, it must be 0.
        /// </summary>
        uint SequenceNumber { get; }

        /// <summary>
        /// The session Key
        /// </summary>
        byte[] SessionKey { get; }

        /// <summary>
        /// The token returned by Sspi.
        /// </summary>
        byte[] Token { get; }

        /// <summary>
        /// Queries the sizes of the structures used in the per-message functions.
        /// </summary>
        SecurityPackageContextSizes ContextSizes { get; }

        /// <summary>
        /// Package-specific flags that indicate the quality of protection. A security package can use this parameter
        /// to enable the selection of cryptographic algorithms.
        /// </summary>
        SECQOP_WRAP QualityOfProtection { get; }

        /// <summary>
        /// Initialize SecurityContext with a server token.
        /// </summary>
        /// <param name="serverToken">Server Token</param>
        /// <exception cref="SspiException">If Initialize fail, this exception will be thrown.</exception>
        void Initialize(byte[] serverToken);

        /// <summary>
        /// Sign data according SecBuffers.
        /// </summary>
        /// <param name="securityBuffers">
        /// the security buffer array to sign.<para/>
        /// it can contain none or some data security buffer, that are combine to one message to sign.<para/>
        /// it must contain token security buffer, in which the signature will be stored.
        /// </param>
        /// <exception cref="ArgumentNullException">the securityBuffers must not be null</exception>
        /// <exception cref="ArgumentException">securityBuffers must contain signature to store signature</exception>
        void Sign(params SecurityBuffer[] securityBuffers);

        /// <summary>
        /// Verify Message.
        /// </summary>
        /// <param name="securityBuffers">
        /// the security buffer array to verify.<para/>
        /// it can contain none or some data security buffer, that are combine to one message to verify.<para/>
        /// it must contain token security buffer, in which the signature is stored.
        /// </param>
        /// <exception cref="ArgumentNullException">the securityBuffers must not be null</exception>
        /// <exception cref="ArgumentException">securityBuffers must contain signature to verify</exception>
        bool Verify(params SecurityBuffer[] securityBuffers);

        /// <summary>
        /// Encrypts Message. User decides what SecBuffers are used.
        /// </summary>
        /// <param name="securityBuffers">
        /// the security buffer array to encrypt.<para/>
        /// it can contain none or some data security buffer, that are combine to one message to encrypt.<para/>
        /// it can contain none or some token security buffer, in which the signature will be stored.
        /// </param>
        /// <exception cref="ArgumentNullException">the securityBuffers must not be null</exception>
        void Encrypt(params SecurityBuffer[] securityBuffers);

        /// <summary>
        /// Decrypt Message
        /// </summary>
        /// <param name="securityBuffers">
        /// the security buffer array to decrypt.<para/>
        /// it can contain none or some data security buffer, that are combine to one message to decrypt.<para/>
        /// it can contain none or some token security buffer, in which the signature is stored.
        /// </param>
        /// <returns>the encrypt result, if verify, it's the verify result.</returns>
        /// <exception cref="ArgumentNullException">the securityBuffers must not be null</exception>
        bool Decrypt(params SecurityBuffer[] securityBuffers);
    }
}
