// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Kile
{
    /// <summary>
    /// A derived class of ServerSecurityContext which is a SSPI wrapped class. 
    /// Provide Kerberos authentication for upper-layer protocol. 
    /// This class only supports single realm transport and single service principle request.
    /// </summary>
    public class KerberosServerSecurityContext : ServerSecurityContext, IDisposable
    {
        #region Private Members

        /// <summary>
        /// Represents whether this object has been disposed.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// The KILE server.
        /// </summary>
        private KileServer server;

        /// <summary>
        /// Specify whether needs to continue authentication
        /// </summary>
        private bool continueProcess;

        /// <summary>
        /// The token returned after authentication.
        /// </summary>
        private byte[] token;

        /// <summary>
        /// Encrypt key of the service principle. This parameter cannot be null.
        /// </summary>
        private EncryptionKey ticketEncryptKey;

        /// <summary>
        /// Specify whether current token to accept is the initial one.
        /// </summary>
        private bool isInitialToken;

        /// <summary>
        /// Queries the sizes of the structures used in the per-message functions.
        /// </summary>
        private SecurityPackageContextSizes contextSizes;

        #endregion


        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ticketEncryptKey">Encrypt key of the service principle. This parameter cannot be null.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the input parameter is null.</exception>
        public KerberosServerSecurityContext(EncryptionKey ticketEncryptKey)
        {
            if (ticketEncryptKey == null)
            {
                throw new ArgumentNullException("ticketEncryptKey");
            }

            server = new KileServer("");
            server.context = new KileServerContext();
            this.ticketEncryptKey = ticketEncryptKey;
            isInitialToken = true;
            contextSizes = new SecurityPackageContextSizes();
            contextSizes.MaxTokenSize = ConstValue.MAX_TOKEN_SIZE;
            contextSizes.MaxSignatureSize = ConstValue.MAX_SIGNATURE_SIZE;
            contextSizes.BlockSize = ConstValue.BLOCK_SIZE;
            contextSizes.SecurityTrailerSize = ConstValue.SECURITY_TRAILER_SIZE;
        }


        /// <summary>
        /// Constructor.
        /// Use principle's password with RC4_HMAC encryption type to generate encryption key of the ticket.
        /// </summary>
        /// <param name="ticketEncryptKey">Password of the service principle. This parameter cannot be null.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the input parameter is null.</exception>
        public KerberosServerSecurityContext(
            string ticketEncryptPassword)
            : this(new EncryptionKey((int)EncryptionType.RC4_HMAC,
                KeyGenerator.MakeKey(EncryptionType.RC4_HMAC, ticketEncryptPassword, null)))
        {
        }

        #endregion


        #region Override Methods

        /// <summary>
        /// Accept client token.
        /// </summary>
        /// <param name="inToken">The client's token.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the input parameter is null.</exception>
        /// <exception cref="System.FormatException">Thrown when the token format is invalid.</exception>
        public override void Accept(byte[] inToken)
        {
            if (inToken == null)
            {
                throw new ArgumentNullException("inToken");
            }
            if (isInitialToken)
            {
                KileApRequest apRequest = new KileApRequest(server.Context);
                apRequest.FromBytes(inToken, ticketEncryptKey);
                bool isMutualAuth = (apRequest.Request.ap_options.mValue[0] << 24 & (int)ApOptions.MutualRequired)
                    == (int)ApOptions.MutualRequired;
                bool isDceStyle = inToken[0] != ConstValue.KERBEROS_TAG;

                if (isMutualAuth || isDceStyle)
                {
                    EncryptionKey apSubKey = new EncryptionKey((int)EncryptionType.RC4_HMAC,
                        Guid.NewGuid().ToByteArray());
                    KileApResponse apResponse = server.CreateApResponse(apSubKey);

                    // Set a random sequence number
                    Random randomNumber = new Random();
                    apResponse.ApEncPart.seq_number = new UInt32(randomNumber.Next());
                    server.context.currentLocalSequenceNumber = (ulong)apResponse.ApEncPart.seq_number.mValue;
                    token = apResponse.ToBytes();
                }
                isInitialToken = false;

                if (inToken[0] != ConstValue.KERBEROS_TAG)
                {
                    // SEC_I_CONTINUE_NEEDED;
                    continueProcess = true;
                }
                else
                {
                    // SEC_E_OK;
                    continueProcess = false;
                }
            }
            else
            {
                KileApResponse apResponse = new KileApResponse(server.Context);
                apResponse.FromBytes(inToken);

                if (server.Context.CurrentLocalSequenceNumber != (ulong)apResponse.ApEncPart.seq_number.mValue)
                {
                    throw new FormatException("Sequence number does not match.");
                }

                // SEC_E_OK;
                continueProcess = false;
                token = null;
            }
        }


        /// <summary>
        /// This takes the given SecurityBuffer array, signs data part, and update signature into token part
        /// </summary>
        /// <param name="securityBuffers">Data to sign and token to update.</param>
        /// <exception cref="System.NotSupportedException">Thrown when the input parameter is null.</exception>
        /// <exception cref="System.ArgumentException">Thrown when the input parameter is null.</exception>
        public override void Sign(params SecurityBuffer[] securityBuffers)
        {
            KileUtility.Sign(server, securityBuffers);
        }


        /// <summary>
        /// This takes the given byte array and verifies it using the SSPI VerifySignature method.
        /// </summary>
        /// <param name="securityBuffers">Data and token to verify</param>
        /// <returns>Success if true, Fail if false</returns>
        public override bool Verify(params SecurityBuffer[] securityBuffers)
        {
            return KileUtility.Verify(server, securityBuffers);
        }


        /// <summary>
        /// Encrypts Message. User decides what SecBuffers are used.
        /// </summary>
        /// <param name="securityBuffers">The security buffers to encrypt.</param>
        public override void Encrypt(params SecurityBuffer[] securityBuffers)
        {
            KileUtility.Encrypt(server, securityBuffers);
        }


        /// <summary>
        /// This takes the given byte array, decrypts it, and returns
        /// the original, unencrypted byte array.
        /// </summary>
        /// <param name="securityBuffers">The security buffers to decrypt.</param>
        public override bool Decrypt(params SecurityBuffer[] securityBuffers)
        {
            return KileUtility.Decrypt(server, securityBuffers);
        }

        #endregion


        #region Override Properties

        /// <summary>
        /// Package type
        /// </summary>
        public override SecurityPackageType PackageType
        {
            get
            {
                return SecurityPackageType.Kerberos;
            }
        }


        /// <summary>
        /// The token returned after authentication.
        /// </summary>
        public override byte[] Token
        {
            get
            {
                return token;
            }
        }


        /// <summary>
        /// Whether to continue process.
        /// </summary>
        public override bool NeedContinueProcessing
        {
            get
            {
                return continueProcess;
            }
        }


        /// <summary>
        /// Currently local sequence number
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public override uint SequenceNumber
        {
            get
            {
                return (uint)server.Context.CurrentLocalSequenceNumber;
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        /// <summary>
        /// This returns the session key to be used in the security context, for both client and server side.
        /// </summary>
        /// <exception cref="System.ArgumentException">Thrown when the key is not valid.</exception>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public override byte[] SessionKey
        {
            get
            {
                EncryptionKey key = server.Context.ContextKey;

                if (key == null || key.keytype == null || key.keyvalue == null || key.keyvalue.mValue == null)
                {
                    throw new ArgumentException("Session key is not valid.");
                }
                return key.keyvalue.mValue;
            }
        }


        /// <summary>
        /// Queries the sizes of the structures used in the per-message functions.
        /// </summary>
        public override SecurityPackageContextSizes ContextSizes
        {
            get
            {
                return contextSizes;
            }
        }

        #endregion


        #region IDisposable

        /// <summary>
        /// Release resources.
        /// </summary>
        /// <param name="disposing">If disposing equals true, Managed and unmanaged resources are disposed.
        /// if false, Only unmanaged resources can be disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //Release managed resource.
                    if (server != null)
                    {
                        server.Dispose();
                        server = null;
                    }
                }

                //Note disposing has been done.
                disposed = true;
            }
        }


        /// <summary>
        /// Release resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            // Take this object out of the finalization queue of the GC:
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Destructor
        /// </summary>
        ~KerberosServerSecurityContext()
        {
            Dispose(false);
        }

        #endregion
    }
}
