// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nlmp
{
    /// <summary>
    /// the context of nlmp.
    /// </summary>
    public abstract class NlmpContext
    {
        #region Fields From Sdk

        /// <summary>
        /// SessionBaseKey is used to generate other keys, such as ClientSigningKey, ServerSealingKey and so on.<para/>
        /// This key also is exported to application to sign/seal. e.g. smb uses this key to generate the signature in
        /// SmbHeader<para/>
        /// This key is defined in TD, and Gss Api named it SessionKey also.
        /// </summary>
        private byte[] sessionBaseKey;

        #endregion

        #region Fields From TD

        /// <summary>
        /// A 128-bit (16-byte) session key used to derive ClientSigningKey, ClientSealingKey, ServerSealingKey, and
        /// ServerSigningKey.
        /// </summary>
        private byte[] exportedSessionKey;

        /// <summary>
        /// The set of configuration flags (section 2.2.2.5) that specifies the negotiated capabilities of the client
        /// and server for the current NTLM session.
        /// </summary>
        private NegotiateTypes negFlg;

        /// <summary>
        /// A string that indicates the name of the user.
        /// </summary>
        private string user;

        /// <summary>
        /// A string that indicates the name of the user's domain.
        /// </summary>
        private string userDom;

        /// <summary>
        /// A Boolean setting that controls using the NTLM response for the LM response to the server challenge when
        /// NTLMv1 authentication is used. The default value of this state variable is TRUE. Windows NT Server 4.0 SP3
        /// does not support providing NTLM instead of LM responses.
        /// </summary>
        private bool noLMResponseNTLMv1;

        /// <summary>
        /// A Boolean setting that disables the client from sending NTLM_AUTHENTICATE messages.
        /// </summary>
        private bool clientBlocked;

        /// <summary>
        /// A list of server names that can use NTLM authentication.
        /// </summary>
        private string[] clientBlockExceptions;

        /// <summary>
        /// A Boolean setting that requires the client to use 128-bit encryption.
        /// </summary>
        private bool clientRequire128bitEncryption;

        /// <summary>
        /// An integer that indicates the maximum lifetime for challenge/response pairs.
        /// </summary>
        private int maxLifetime;

        /// <summary>
        /// The signing key used by the client to sign messages and used by the server to verify signed client 
        /// messages. It is generated after the client is authenticated by the server and is not passed over the wire.
        /// </summary>
        private byte[] clientSigningKey;

        /// <summary>
        /// The sealing key used by the client to seal messages and used by the server to unseal client messages. It 
        /// is generated after the client is authenticated by the server and is not passed over the wire.
        /// </summary>
        private byte[] clientSealingKey;

        /// <summary>
        /// A 4-byte sequence number (section 3.4.4).
        /// </summary>
        private int seqNum;

        /// <summary>
        /// The sealing key used by the server to seal messages and used by the client to unseal server messages. It 
        /// is generated after the client is authenticated by the server and is not passed over the wire.
        /// </summary>
        private byte[] serverSigningKey;

        /// <summary>
        /// The signing key used by the server to sign messages and used by the client to verify signed server 
        /// messages. It is generated after the client is authenticated by the server and is not passed over the wire.
        /// </summary>
        private byte[] serverSealingKey;

        /// <summary>
        /// Service principal name (SPN) of the service that the client wishes to authenticate to. This value is
        /// optional.
        /// </summary>
        private string clientSuppliedTargetName;

        /// <summary>
        /// the client handle of this context.<para/>
        /// ClientHandle holds the security context that build by NlmpUtility.RC4Init(Handle, Key)
        /// </summary>
        private int clientHandle;

        /// <summary>
        /// the server handle of this context<para/>
        /// ServerHandle holds the security context that build by NlmpUtility.RC4Init(Handle, Key)
        /// </summary>
        private int serverHandle;

        #endregion

        #region Properties From Sdk

        /// <summary>
        /// the client handle of this context.<para/>
        /// ClientHandle holds the security context that build by NlmpUtility.RC4Init(Handle, Key)
        /// </summary>
        public int ClientHandle
        {
            get
            {
                if (clientHandle == 0)
                {
                    clientHandle = NlmpUtility.NextHandle;
                }
                return clientHandle;
            }
        }


        /// <summary>
        /// the server handle of this context<para/>
        /// ServerHandle holds the security context that build by NlmpUtility.RC4Init(Handle, Key)
        /// </summary>
        public int ServerHandle
        {
            get
            {
                if (serverHandle == 0)
                {
                    serverHandle = NlmpUtility.NextHandle;
                }
                return serverHandle;
            }
        }


        /// <summary>
        /// SessionBaseKey is used to generate other keys, such as ClientSigningKey, ServerSealingKey and so on.<para/>
        /// This key also is exported to application to sign/seal. e.g. smb uses this key to generate the signature in
        /// SmbHeader<para/>
        /// This key is defined in TD, and Gss Api named it SessionKey also.
        /// </summary>
        public byte[] SessionBaseKey
        {
            get
            {
                return this.sessionBaseKey;
            }
            set
            {
                this.sessionBaseKey = value;
            }
        }


        #endregion

        #region Properties From TD, Internal to Protocol

        /// <summary>
        /// A 128-bit (16-byte) session key used to derive ClientSigningKey, ClientSealingKey, ServerSealingKey, and
        /// ServerSigningKey.
        /// </summary>
        public byte[] ExportedSessionKey
        {
            get
            {
                return this.exportedSessionKey;
            }
            set
            {
                this.exportedSessionKey = value;
            }
        }


        /// <summary>
        /// The set of configuration flags (section 2.2.2.5) that specifies the negotiated capabilities of the client
        /// and server for the current NTLM session.
        /// </summary>
        public NegotiateTypes NegFlg
        {
            get
            {
                return this.negFlg;
            }
            set
            {
                this.negFlg = value;
            }
        }


        /// <summary>
        /// A string that indicates the name of the user.
        /// </summary>
        public string User
        {
            get
            {
                return this.user;
            }
            set
            {
                this.user = value;
            }
        }


        /// <summary>
        /// A string that indicates the name of the user's domain.
        /// </summary>
        public string UserDom
        {
            get
            {
                return this.userDom;
            }
            set
            {
                this.userDom = value;
            }
        }


        /// <summary>
        /// A Boolean setting that controls using the NTLM response for the LM response to the server challenge when
        /// NTLMv1 authentication is used.
        /// </summary>
        public bool NoLMResponseNTLMv1
        {
            get
            {
                return this.noLMResponseNTLMv1;
            }
            set
            {
                this.noLMResponseNTLMv1 = value;
            }
        }


        /// <summary>
        /// A Boolean setting that disables the client from sending NTLM_AUTHENTICATE messages.
        /// </summary>
        public bool ClientBlocked
        {
            get
            {
                return this.clientBlocked;
            }
            set
            {
                this.clientBlocked = value;
            }
        }


        /// <summary>
        /// A list of server names that can use NTLM authentication.
        /// </summary>
        public string[] ClientBlockExceptions
        {
            get
            {
                return this.clientBlockExceptions;
            }
            set
            {
                this.clientBlockExceptions = value;
            }
        }


        /// <summary>
        /// A Boolean setting that requires the client to use 128-bit encryption.
        /// </summary>
        public bool ClientRequire128bitEncryption
        {
            get
            {
                return this.clientRequire128bitEncryption;
            }
            set
            {
                this.clientRequire128bitEncryption = value;
            }
        }


        /// <summary>
        /// An integer that indicates the maximum lifetime for challenge/response pairs.
        /// </summary>
        public int MaxLifetime
        {
            get
            {
                return this.maxLifetime;
            }
            set
            {
                this.maxLifetime = value;
            }
        }


        /// <summary>
        /// The signing key used by the client to sign messages and used by the server to verify signed client 
        /// messages. It is generated after the client is authenticated by the server and is not passed over the wire.
        /// </summary>
        public byte[] ClientSigningKey
        {
            get
            {
                return this.clientSigningKey;
            }
            set
            {
                this.clientSigningKey = value;
            }
        }


        /// <summary>
        /// The sealing key used by the client to seal messages and used by the server to unseal client messages. It 
        /// is generated after the client is authenticated by the server and is not passed over the wire.
        /// </summary>
        public byte[] ClientSealingKey
        {
            get
            {
                return this.clientSealingKey;
            }
            set
            {
                this.clientSealingKey = value;
            }
        }


        /// <summary>
        /// A 4-byte sequence number (section 3.4.4).
        /// </summary>
        public int SeqNum
        {
            get
            {
                return this.seqNum;
            }
            set
            {
                this.seqNum = value;
            }
        }


        /// <summary>
        /// The signing key used by the server to sign messages and used by the client to verify signed server 
        /// messages. It is generated after the client is authenticated by the server and is not passed over the wire.
        /// </summary>
        public byte[] ServerSigningKey
        {
            get
            {
                return this.serverSigningKey;
            }
            set
            {
                this.serverSigningKey = value;
            }
        }


        /// <summary>
        /// The sealing key used by the server to seal messages and used by the client to unseal server messages. It 
        /// is generated after the client is authenticated by the server and is not passed over the wire.
        /// </summary>
        public byte[] ServerSealingKey
        {
            get
            {
                return this.serverSealingKey;
            }
            set
            {
                this.serverSealingKey = value;
            }
        }


        #endregion

        #region Properties From TD, Exposed to Application

        /// <summary>
        /// Indicates that the connectionless mode of NTLM is to be selected. If the Datagram option is selected by the
        /// client, then connectionless mode is used and NTLM performs a bitwise OR operation with the following NTLM
        /// Negotiate Flag into the ClientConfigFlags.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly")]
        public abstract bool Datagram
        {
            set;
        }


        /// <summary>
        /// Service principal name (SPN) of the service that the client wishes to authenticate to. This value is
        /// optional.
        /// </summary>
        public string ClientSuppliedTargetName
        {
            get
            {
                return this.clientSuppliedTargetName;
            }
            set
            {
                this.clientSuppliedTargetName = value;
            }
        }


        #endregion

        #region Constructor

        /// <summary>
        /// the constructor, set the default values
        /// </summary>
        protected NlmpContext()
        {
            this.NoLMResponseNTLMv1 = true;
        }


        #endregion
    }
}
