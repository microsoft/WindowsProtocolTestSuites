// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc
{
    /// <summary>
    /// NRPC context shared by client and server session.
    /// </summary>
    public abstract class NrpcContext
    {
        // A 64-bit integer value used for detecting out-of-order messages.
        private ulong sequenceNumber;

        // Session-Key.
        private byte[] sessionKey;

        // The credential that is maintained between client and server
        // that is used during computation and verification 
        // of the Netlogon authenticator (section 3.1.4.5).
        private byte[] storedCredential;

        // A byte array that contains the client challenge.        
        private byte[] clientChallenge;

        // A 32-bit set of bit flags in little-endian format that 
        // indicate features client and server supported.
        private NrpcNegotiateFlags negotiateFlags;

        // A byte array that contains the server challenge response.
        private byte[] serverChallenge;

        // An even-numbered sequence of bytes, with no embedded zero values, 
        // that is a plain-text secret (password) shared between the client and the server.
        private string sharedSecret;

        // An unsigned 32-bit integer which indicates the number 
        // of times that a trust password has changed.
        //The first trust password generated has TrustPasswordVersion equal to one
        private uint trustPasswordVersion = 1;

        // A Boolean setting that indicates whether the RPC message has 
        // to be encrypted or just integrity-protected. 
        // When TRUE, the message will be encrypted; 
        // otherwise, it will be integrity-protected.
        private bool sealSecureChannel;

        // A string that contains the name of the account 
        // whose password is being changed. 
        // In windows, all machine account names are the name of
        // the machine with a $ (dollar sign) appended.
        private string accountName;

        // The RID of the account specified by the AccountName parameter.
        private uint accountRid;

        // The NetBIOS name of the client computer.
        private string clientComputerName;

        // Indicates the type of the secure channel being established by this call.
        private _NETLOGON_SECURE_CHANNEL_TYPE secureChannelType;


        /// <summary>
        /// A 64-bit integer value used for detecting out-of-order messages.
        /// </summary>
        public ulong SequenceNumber
        {
            get
            {
                return sequenceNumber;
            }
            set
            {
                sequenceNumber = value;
            }
        }


        /// <summary>
        /// Session-Key
        /// </summary>
        public byte[] SessionKey
        {
            get
            {
                return sessionKey;
            }
            set
            {
                sessionKey = value;
            }
        }


        /// <summary>
        /// A NETLOGON_CREDENTIAL (section 2.2.1.3.4) structure containing 
        /// the credential that is maintained between client and server
        /// that is used during computation and verification 
        /// of the Netlogon authenticator (section 3.1.4.5).
        /// </summary>
        public byte[] StoredCredential
        {
            get
            {
                return storedCredential;
            }
            set
            {
                storedCredential = value;
            }
        }


        /// <summary>
        /// A pointer to a NETLOGON_CREDENTIAL structure that contains the client challenge.
        /// </summary>
        public byte[] ClientChallenge
        {
            get
            {
                return clientChallenge;
            }
            set
            {
                clientChallenge = value;
            }
        }


        /// <summary>
        /// A 32-bit set of bit flags in little-endian format that 
        /// indicate features client and server supported.
        /// </summary>
        public NrpcNegotiateFlags NegotiateFlags
        {
            get
            {
                return negotiateFlags;
            }
            set
            {
                negotiateFlags = value;
            }
        }


        /// <summary>
        /// A pointer to a NETLOGON_CREDENTIAL structure that 
        /// contains the server challenge response.
        /// </summary>
        public byte[] ServerChallenge
        {
            get
            {
                return serverChallenge;
            }
            set
            {
                serverChallenge = value;
            }
        }


        /// <summary>
        /// An even-numbered sequence of bytes, with no embedded zero values, 
        /// that is a plain-text secret (password) shared between the client and the server.
        /// </summary>
        public string SharedSecret
        {
            get
            {
                return sharedSecret;
            }
            set
            {
                sharedSecret = value;
            }
        }


        /// <summary>
        /// An unsigned 32-bit integer which indicates the number 
        /// of times that a trust password has changed.
        /// </summary>
        public uint TrustPasswordVersion
        {
            get
            {
                return trustPasswordVersion;
            }
            set
            {
                trustPasswordVersion = value;
            }
        }


        /// <summary>
        /// A Boolean setting that indicates whether the RPC message has 
        /// to be encrypted or just integrity-protected. 
        /// When TRUE, the message will be encrypted; 
        /// otherwise, it will be integrity-protected.
        /// </summary>
        public bool SealSecureChannel
        {
            get
            {
                return sealSecureChannel;
            }
            set
            {
                sealSecureChannel = value;
            }
        }


        /// <summary>
        /// A string that contains the name of the account 
        /// whose password is being changed. 
        /// In windows, all machine account names are the name of
        /// the machine with a $ (dollar sign) appended.
        /// </summary>
        public string AccountName
        {
            get
            {
                return accountName;
            }
            set
            {
                accountName = value;
            }
        }


        /// <summary>
        /// The RID of the account specified by the AccountName parameter.
        /// </summary>
        public uint AccountRid
        {
            get
            {
                return accountRid;
            }
            set
            {
                accountRid = value;
            }
        }


        /// <summary>
        /// The NetBIOS name of the client computer.
        /// </summary>
        public string ClientComputerName
        {
            get
            {
                return clientComputerName;
            }
            set
            {
                clientComputerName = value;
            }
        }


        /// <summary>
        /// Indicates the type of the secure channel being established by this call.
        /// </summary>
        public _NETLOGON_SECURE_CHANNEL_TYPE SecureChannelType
        {
            get
            {
                return secureChannelType;
            }
            set
            {
                secureChannelType = value;
            }
        }
    }
}
