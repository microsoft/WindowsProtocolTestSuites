// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;

using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Kile
{
    /// <summary>
    /// A derived class of ClientSecurityContext which is a SSPI wrapped class. 
    /// Provide Kerberos authentication for upper-layer protocol. 
    /// This class only supports single realm transport.
    /// </summary>
    [CLSCompliant(false)]
    public class KerberosClientSecurityContext : ClientSecurityContext, IDisposable
    {
        #region private members

        /// <summary>
        /// Represents whether this object has been disposed.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// The KILE client.
        /// </summary>
        private KileClient client;

        /// <summary>
        /// The service to request.
        /// </summary>
        private string service;

        /// <summary>
        /// The realm part of the client's principal identifier.
        /// </summary>
        private string domain;

        /// <summary>
        /// The user logon name to generate salt.
        /// </summary>
        private string userLogonName;

        /// <summary>
        /// Context attribute flags.
        /// </summary>
        private ClientSecurityContextAttribute contextAttribute;

        /// <summary>
        /// Specify whether needs to continue authentication
        /// </summary>
        private bool continueProcess;

        /// <summary>
        /// The token returned after authentication.
        /// </summary>
        private byte[] token;

        /// <summary>
        /// Queries the sizes of the structures used in the per-message functions.
        /// </summary>
        private SecurityPackageContextSizes contextSizes;

        #endregion private members


        #region constructor
        /// <summary>
        /// Construct an instance of Kerberos Client Context to do Kerberos Authentication.
        /// </summary>
        /// <param name="clientCredential">Client's credential</param>
        /// <param name="logonName">The user logon name used to generate salt. This argument cannot be null.</param>
        /// <param name="serviceName">The name of the service to request. This argument cannot be null.</param>
        /// <param name="kdcIpAddress">KDC IP address</param>
        /// <param name="contextAttributes">Context attribute flags. 
        /// This is the fContextReq parameter of method InitializeSecurityContext. Delegate is not supported.</param>
        /// <param name="transportType">Whether the transport is TCP or UDP transport.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the input parameter is null</exception>
        /// <exception cref="System.InvalidOperationException">Thrown when fail to resolve host name.</exception>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
        [CLSCompliant(false)]
        public KerberosClientSecurityContext(AccountCredential clientCredential,
                                             string logonName,
                                             string serviceName,
                                             IPAddress kdcIpAddress,
                                             ClientSecurityContextAttribute contextAttributes,
                                             KileConnectionType transportType)
        {
            if (clientCredential.DomainName == null)
            {
                throw new ArgumentNullException(nameof(clientCredential.DomainName));
            }
            if (clientCredential.AccountName == null)
            {
                throw new ArgumentNullException(nameof(clientCredential.AccountName));
            }
            if (clientCredential.Password == null)
            {
                throw new ArgumentNullException(nameof(clientCredential.Password));
            }
            if (kdcIpAddress == null)
            {
                throw new ArgumentNullException(nameof(kdcIpAddress));
            }
            if (logonName == null)
            {
                throw new ArgumentNullException(nameof(logonName));
            }
            if (serviceName == null)
            {
                throw new ArgumentNullException(nameof(serviceName));
            }

            client = new KileClient(clientCredential.DomainName, clientCredential.AccountName, clientCredential.Password,
                KileAccountType.User);
            service = serviceName;
            domain = clientCredential.DomainName;
            userLogonName = logonName;
            contextAttribute = contextAttributes;
            client.Connect(kdcIpAddress.ToString(), ConstValue.KDC_PORT, transportType);
            contextSizes = new SecurityPackageContextSizes();
            contextSizes.MaxTokenSize = ConstValue.MAX_TOKEN_SIZE;
            contextSizes.MaxSignatureSize = ConstValue.MAX_SIGNATURE_SIZE;
            contextSizes.BlockSize = ConstValue.BLOCK_SIZE;
            contextSizes.SecurityTrailerSize = ConstValue.SECURITY_TRAILER_SIZE;
        }

        #endregion constructor


        #region override methods

        /// <summary>
        /// Initialize the context from a token.
        /// </summary>
        /// <param name="inToken">The token used to initialize.</param>
        /// <exception cref="System.NotSupportedException">Thrown when the ContextAttribute contains a flag that not be
        /// supported.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown when an error is returned.</exception>
        public override void Initialize(byte[] inToken)
        {
            if (inToken == null || inToken.Length == 0)
            {
                // a new connection
                KilePdu response = null;
                string sname = ConstValue.KERBEROS_SNAME;
                KRBFlags flags = KRBFlags.FORWARDABLE | KRBFlags.RENEWABLE
                               | KRBFlags.CANONICALIZE | KRBFlags.RENEWABLEOK;
                PaEncTimeStamp timestamp = client.ConstructPaEncTimeStamp(EncryptionType.RC4_HMAC);
                PaPacRequest pacRequest = client.ConstructPaPacRequest(true);
                Asn1SequenceOf<PA_DATA> paData = client.ConstructPaData(timestamp, pacRequest);
                KileAsRequest asRequest;
                EncryptionKey subkey = null;

                if ((contextAttribute & ClientSecurityContextAttribute.DceStyle)
                    == ClientSecurityContextAttribute.DceStyle)
                {
                    asRequest = client.CreateAsRequest(sname, flags, paData, EncryptionType.AES256_CTS_HMAC_SHA1_96,
                        EncryptionType.RC4_HMAC);
                    //subkey = new EncryptionKey((int)EncryptionType.AES256_CTS_HMAC_SHA1_96,
                    //    KileUtility.GenerateRandomBytes(ConstValue.AES_KEY_LENGTH));

                    var key = KeyGenerator.MakeKey(EncryptionType.AES256_CTS_HMAC_SHA1_96, client.ClientContext.Password, client.ClientContext.Salt);
                    subkey = new EncryptionKey(new KerbInt32((long)EncryptionType.AES256_CTS_HMAC_SHA1_96), new Asn1OctetString(key));
                }
                else
                {
                    asRequest = client.CreateAsRequest(sname, flags, paData, EncryptionType.RC4_HMAC);
                }
                client.SendPdu(asRequest);
                response = client.ExpectPdu(ConstValue.TIMEOUT_DEFAULT);

                if (response.GetType() == typeof(KileKrbError))
                {
                    throw new InvalidOperationException("Received Kerberos Error response: " + ((KileKrbError)response).ErrorCode);
                }
                KileAsResponse asResponse = (KileAsResponse)response;
                sname = service;
                // for example: "KERB.COMldapsut02.kerb.com"
                client.ClientContext.Salt = domain.ToUpper();
                string[] nameList = userLogonName.Split('/');
                foreach (string name in nameList)
                {
                    client.ClientContext.Salt += name;
                }
                KileTgsRequest tgsRequest = client.CreateTgsRequest(sname,
                                                              flags,
                                                              new KerbUInt32((long)Math.Abs((long)DateTime.Now.Ticks)),
                                                              null,
                                                              ChecksumType.hmac_md5_string,
                                                              null,
                                                              null);
                client.SendPdu(tgsRequest);
                response = client.ExpectPdu(ConstValue.TIMEOUT_DEFAULT);

                if (response.GetType() == typeof(KileKrbError))
                {
                    throw new InvalidOperationException("Received Kerberos Error response: " + ((KileKrbError)response).ErrorCode);
                }
                KileTgsResponse tgsResponse = (KileTgsResponse)response;
                ApOptions apOption;
                ChecksumFlags checksumFlag;
                GetFlagsByContextAttribute(out apOption, out checksumFlag);

                KerbAuthDataTokenRestrictions adRestriction =
                    client.ConstructKerbAuthDataTokenRestrictions(0,
                    (uint)LSAP_TOKEN_INFO_INTEGRITY_Flags.FULL_TOKEN,
                    (uint)LSAP_TOKEN_INFO_INTEGRITY_TokenIL.Medium,
                    new Guid().ToString());
                AdAuthDataApOptions adApOptions = client.ConstructAdAuthDataApOptions(ConstValue.KERB_AP_OPTIONS_CBT);
                AuthorizationData authData = client.ConstructAuthorizationData(adRestriction, adApOptions);
                KileApRequest apRequest = client.CreateApRequest(apOption,
                                                             ChecksumType.ap_authenticator_8003,
                                                             ConstValue.SEQUENCE_NUMBER_DEFAULT,
                                                             checksumFlag,
                                                             subkey,
                                                             authData);
                token = apRequest.ToBytes();
                bool isMutualAuth = (contextAttribute & ClientSecurityContextAttribute.MutualAuth)
                    == ClientSecurityContextAttribute.MutualAuth;
                bool isDceStyle = (contextAttribute & ClientSecurityContextAttribute.DceStyle)
                    == ClientSecurityContextAttribute.DceStyle;

                if (isMutualAuth || isDceStyle)
                {
                    continueProcess = true;   // SEC_I_CONTINUE_NEEDED;
                }
                else
                {
                    continueProcess = false;  // SEC_E_OK;
                }
            }
            else  // mutual authentication
            {
                KileApResponse apResponse = client.ParseApResponse(inToken);
                token = null;

                if ((contextAttribute & ClientSecurityContextAttribute.DceStyle)
                    == ClientSecurityContextAttribute.DceStyle)
                {
                    KileApResponse apResponseSend = client.CreateApResponse(null);
                    token = apResponseSend.ToBytes();
                }

                continueProcess = false;      // SEC_E_OK;
            }
        }


        /// <summary>
        /// This takes the given SecurityBuffer array, signs data part, and update signature into token part
        /// </summary>
        /// <param name="securityBuffers">Data to sign and token to update.</param>
        [CLSCompliant(false)]
        public override void Sign(params SecurityBuffer[] securityBuffers)
        {
            KileUtility.Sign(client, securityBuffers);
        }


        /// <summary>
        /// This takes the given byte array and verifies it using the SSPI VerifySignature method.
        /// </summary>
        /// <param name="securityBuffers">Data and token to verify</param>
        /// <returns>Success if true, Fail if false</returns>
        [CLSCompliant(false)]
        public override bool Verify(params SecurityBuffer[] securityBuffers)
        {
            return KileUtility.Verify(client, securityBuffers);
        }


        /// <summary>
        /// Encrypts Message. User decides what SecBuffers are used.
        /// </summary>
        /// <param name="securityBuffers">The security buffers to encrypt.</param>
        [CLSCompliant(false)]
        public override void Encrypt(params SecurityBuffer[] securityBuffers)
        {
            KileUtility.Encrypt(client, securityBuffers);
        }


        /// <summary>
        /// This takes the given byte array, decrypts it, and returns
        /// the original, unencrypted byte array.
        /// </summary>
        /// <param name="securityBuffers">The security buffers to decrypt.</param>
        [CLSCompliant(false)]
        public override bool Decrypt(params SecurityBuffer[] securityBuffers)
        {
            return KileUtility.Decrypt(client, securityBuffers);
        }

        #endregion


        #region override properties

        /// <summary>
        /// Package type
        /// </summary>
        [CLSCompliant(false)]
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
        [CLSCompliant(false)]
        public override uint SequenceNumber
        {
            get
            {
                return (uint)client.ClientContext.CurrentLocalSequenceNumber;
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
                EncryptionKey key = client.Context.ContextKey;

                if (key == null || key.keytype == null || key.keyvalue == null || key.keyvalue.Value == null)
                {
                    throw new ArgumentException("Session key is not valid.");
                }
                //return key.keyvalue.mValue;
                return key.keyvalue.ByteArrayValue;
            }
        }


        /// <summary>
        /// Queries the sizes of the structures used in the per-message functions.
        /// </summary>
        [CLSCompliant(false)]
        public override SecurityPackageContextSizes ContextSizes
        {
            get
            {
                return contextSizes;
            }
        }

        #endregion


        #region Private Methods

        /// <summary>
        /// Get the ApOptions flag and Checksum flag by the context attribute
        /// </summary>
        /// <param name="apOption">The apOptions flag</param>
        /// <param name="checksumFlags">The checksum flag</param>
        private void GetFlagsByContextAttribute(out ApOptions apOptions, out ChecksumFlags checksumFlags)
        {
            apOptions = ApOptions.None;
            checksumFlags = ChecksumFlags.None;

            if ((contextAttribute & ClientSecurityContextAttribute.Delegate) == ClientSecurityContextAttribute.Delegate)
            {
                throw new NotSupportedException("ContextAttribute.Delegate is not supported currently!");
            }
            if ((contextAttribute & ClientSecurityContextAttribute.UseSessionKey)
                == ClientSecurityContextAttribute.UseSessionKey)
            {
                throw new NotSupportedException("ContextAttribute.UseSessionKey is not supported currently!");
            }
            if ((contextAttribute & ClientSecurityContextAttribute.MutualAuth)
                == ClientSecurityContextAttribute.MutualAuth)
            {
                checksumFlags |= ChecksumFlags.GSS_C_MUTUAL_FLAG;
                apOptions |= ApOptions.MutualRequired;
            }
            if ((contextAttribute & ClientSecurityContextAttribute.ReplayDetect)
                == ClientSecurityContextAttribute.ReplayDetect)
            {
                checksumFlags |= ChecksumFlags.GSS_C_REPLAY_FLAG;
            }
            if ((contextAttribute & ClientSecurityContextAttribute.SequenceDetect)
                == ClientSecurityContextAttribute.SequenceDetect)
            {
                checksumFlags |= ChecksumFlags.GSS_C_SEQUENCE_FLAG;
            }
            if ((contextAttribute & ClientSecurityContextAttribute.Confidentiality)
                == ClientSecurityContextAttribute.Confidentiality)
            {
                checksumFlags |= ChecksumFlags.GSS_C_CONF_FLAG;
            }
            if ((contextAttribute & ClientSecurityContextAttribute.DceStyle) == ClientSecurityContextAttribute.DceStyle)
            {
                checksumFlags |= ChecksumFlags.GSS_C_DCE_STYLE;
            }
            if ((contextAttribute & ClientSecurityContextAttribute.ExtendedError)
                == ClientSecurityContextAttribute.ExtendedError)
            {
                checksumFlags |= ChecksumFlags.GSS_C_EXTENDED_ERROR_FLAG;
            }
            if ((contextAttribute & ClientSecurityContextAttribute.Integrity)
                == ClientSecurityContextAttribute.Integrity)
            {
                checksumFlags |= ChecksumFlags.GSS_C_INTEG_FLAG;
            }
            if ((contextAttribute & ClientSecurityContextAttribute.Identify) == ClientSecurityContextAttribute.Identify)
            {
                checksumFlags |= ChecksumFlags.GSS_C_IDENTIFY_FLAG;
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
                    if (client != null)
                    {
                        client.Dispose();
                        client = null;
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
        ~KerberosClientSecurityContext()
        {
            Dispose(false);
        }

        #endregion
    }
}
