using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib
{
    public interface IDtlsServerSecurityContext : IDisposable
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

        void Accept(byte[] clientToken);

        void Sign(params SecurityBuffer[] securityBuffers);

        bool Verify(params SecurityBuffer[] securityBuffers);

        void Encrypt(params SecurityBuffer[] securityBuffers);

        bool Decrypt(params SecurityBuffer[] securityBuffers);
    }
}
