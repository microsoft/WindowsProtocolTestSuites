// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;

using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Transport;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts
{
    /// <summary>
    /// Ldap client class.
    /// </summary>
    public partial class AdtsLdapClient : IDisposable
    {
        #region Private members

        /// <summary>
        /// Specifies whether the instance has been disposed.
        /// </summary>
#pragma warning disable  0169
        private bool isDisposed;
#pragma warning restore  0169

        /// <summary>
        /// Specifies whether the client is a TCP or UDP connection.
        /// </summary>
        private bool isTcp;

        /// <summary>
        /// The context instance.
        /// </summary>
        private AdtsLdapContext context;

        /// <summary>
        /// The encoder used to create messages.
        /// </summary>
        private AdtsLdapEncoder encoder;

        /// <summary>
        /// The decoder used to decode messages.
        /// </summary>
        private AdtsLdapClientDecoder decoder;

        /// <summary>
        /// The transport stack instance.
        /// </summary>
        private TransportStack transportStack;

        /// <summary>
        /// An AdtsLdapVersion enum that stores the versio of LDAP.
        /// </summary>
        private AdtsLdapVersion ldapVersion;

        /// <summary>
        /// A TransportConfig object that stores the config for TransportStack.
        /// </summary>
        private TransportConfig config;

        #endregion Private members

        #region Properties

        /// <summary>
        /// Gets client context.
        /// </summary>
        public AdtsLdapContext Context
        {
            get
            {
                return this.context;
            }
        }

        #endregion Properties

        #region Public methods

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ldapVersion">The LDAP version that client uses.</param>
        /// <param name="config">
        /// The transport stack configuration. Note that for ADTS-LDAP, only socket transport is supported.
        /// </param>
        /// <exception cref="StackException">Thrown when the TransportConfig is not SocketTransportConfig</exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the stack transport type is neither TCP nor UDP.
        /// </exception>
        public AdtsLdapClient(AdtsLdapVersion ldapVersion, TransportConfig config)
        {
            this.ldapVersion = ldapVersion;
            this.config = config;

            if (ldapVersion == AdtsLdapVersion.V2)
            {
                this.encoder = new AdtsLdapV2Encoder();
            }
            else
            {
                this.encoder = new AdtsLdapV3Encoder();
            }
        }


        /// <summary>
        /// Connects to server.
        /// </summary>
        /// <param name="config">Transport configurations.</param>
        /// <exception cref="InvalidOperationException">
        /// thrown when TransportConifg is null!
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// thrown when TransportConfig is not SocketTransportConfig,
        /// ADTS-LDAP supports socket transport only!
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// thrown when Type of TransportConfig is not TCP/UDP.
        /// Only TCP and UDP are supported for StackTransportType
        /// </exception>
        public virtual void Connect()
        {
            if (this.config == null)
            {
                throw new InvalidOperationException("TransportConfig is null!");
            }

            SocketTransportConfig socketConfig = this.config as SocketTransportConfig;
            if (socketConfig == null)
            {
                throw new NotSupportedException("ADTS-LDAP supports socket transport only!");
            }

            // initialize IsTcp.
            if (socketConfig.Type == StackTransportType.Tcp)
            {
                this.isTcp = true;
            }
            else if (socketConfig.Type == StackTransportType.Udp)
            {
                this.isTcp = false;
            }
            else
            {
                throw new NotSupportedException("Only TCP and UDP are supported for StackTransportType");
            }

            // initialize context.
            if (this.context == null)
            {
                IPEndPoint remoteAddress = new IPEndPoint(socketConfig.RemoteIpAddress, socketConfig.RemoteIpPort);
                this.context = new AdtsLdapContext(ldapVersion, remoteAddress);
            }

            // initialize decorder.
            if (this.decoder == null)
            {
                this.decoder = new AdtsLdapClientDecoder(this);
            }

            // initialize transport stack.
            if (this.transportStack == null)
            {
                this.transportStack = new TransportStack(config, this.decoder.DecodeLdapPacketCallBack);
            }

            #region Transport Connect

            // TCP and UDP differs here. Connect method cannot be used for UDP connections.
            if (this.isTcp)
            {
                this.transportStack.Connect();
            }
            else
            {
                this.transportStack.Start();
            }

            #endregion
        }


        /// <summary>
        /// Disconnects with server.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// thrown when Transport is null! Please invoke Connect() first!
        /// </exception>
        public virtual void Disconnect()
        {
            if (this.transportStack == null)
            {
                throw new InvalidOperationException("Transport is null! Please invoke Connect() first!");
            }

            this.transportStack.Disconnect();
        }


        /// <summary>
        /// Sends a packet to server.
        /// </summary>
        /// <param name="packet">The packet to be sent.</param>
        /// <exception cref="InvalidOperationException">
        /// thrown when Transport is null! Please invoke Connect() first!
        /// </exception>
        public virtual void SendPacket(AdtsLdapPacket packet)
        {
            if (this.transportStack == null)
            {
                throw new InvalidOperationException("Transport is null! Please invoke Connect() first!");
            }

            this.transportStack.SendPacket(packet);
            this.context.MessageId++;
        }


        /// <summary>
        /// Sends packet data to server(to keep compliant with Stack SDK conventions).
        /// Note that if send bytes directly, no check is performed, client context will not be updated, either.
        /// </summary>
        /// <param name="packetData">The packet data.</param>
        /// <exception cref="InvalidOperationException">
        /// thrown when Transport is null! Please invoke Connect() first!
        /// </exception>
        public virtual void SendBytes(byte[] packetData)
        {
            if (this.transportStack == null)
            {
                throw new InvalidOperationException("Transport is null! Please invoke Connect() first!");
            }

            this.transportStack.SendBytes(packetData);
            this.context.MessageId++;
        }


        /// <summary>
        /// to receive bytes from connection.<para/>
        /// the underlayer transport must be TcpClient, Stream or NetbiosClient.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that specifies the timeout for this operation.
        /// </param>
        /// <param name="maxCount">
        /// an int value that specifies the maximum count of expect bytes.
        /// </param>
        /// <returns>
        /// a bytes array that contains the received bytes.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// thrown when maxCount is negative.
        /// </exception>
        public virtual byte[] ExpectBytes(TimeSpan timeout, int maxCount)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("AdtsLdapClient");
            }

            if (maxCount < 0)
            {
                throw new ArgumentException("max count must not be negative", "maxCount");
            }

            return this.transportStack.ExpectBytes(timeout, maxCount);
        }


        /// <summary>
        /// Expects a packet.
        /// </summary>
        /// <param name="timeout">The timeout to be waited.</param>
        /// <returns>The packet received.</returns>
        /// <exception cref="InvalidOperationException">
        /// thrown when Transport is null! Please invoke Connect() first!
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when expect event type from transport is invalid.
        /// </exception>
        public virtual AdtsLdapPacket ExpectPacket(TimeSpan timeout)
        {
            if (this.transportStack == null)
            {
                throw new InvalidOperationException("Transport is null! Please invoke Connect() first!");
            }

            TransportEvent transportEvent = this.transportStack.ExpectTransportEvent(timeout);
            if (transportEvent.EventType == EventType.ReceivedPacket)
            {
                return (AdtsLdapPacket)transportEvent.EventObject;
            }
            else if (transportEvent.EventType == EventType.Exception)
            {
                throw (Exception)transportEvent.EventObject;
            }
            else
            {
                throw new InvalidOperationException("Invalid event type");
            }
        }

        #endregion Public methods

        #region IDisposable Members

        /// <summary>
        /// the dispose flags 
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Release the managed and unmanaged resources. 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            // Take this object out of the finalization queue of the GC:
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Release resources. 
        /// </summary>
        /// <param name = "disposing">
        /// If disposing equals true, Managed and unmanaged resources are disposed. if false, Only unmanaged resources 
        /// can be disposed. 
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed and unmanaged resources.
                if (disposing)
                {
                    // Free managed resources & other reference types:
                    if (this.transportStack != null)
                    {
                        this.transportStack.Dispose();
                        this.transportStack = null;
                    }
                    if (this.security != null)
                    {
                        this.security.Dispose();
                        this.security = null;
                    }
                    this.config = null;
                    this.encoder = null;
                    this.decoder = null;
                }

                // Call the appropriate methods to clean up unmanaged resources.
                // If disposing is false, only the following code is executed:

                this.disposed = true;
            }
        }


        /// <summary>
        /// finalizer 
        /// </summary>
        ~AdtsLdapClient()
        {
            Dispose(false);
        }

        #endregion

        #region Codec methods

        /// <summary>
        /// Adds extended controls to a packet.
        /// </summary>
        /// <param name="packet">The packet to which the controls are added.</param>
        /// <param name="controls">The controls.</param>
        /// <exception cref="NotSupportedException">
        /// Thrown when trying to add controls to an LDAP v2 packet.
        /// </exception>
        public virtual void AddDirectoryControls(AdtsLdapPacket packet, params DirectoryControl[] controls)
        {
            if (this.encoder is AdtsLdapV2Encoder)
            {
                throw new NotSupportedException("Extended controls are only supported for LDAP v3 packets.");
            }
            (this.encoder as AdtsLdapV3Encoder).AddDirectoryControls(packet, controls);
        }


        /// <summary>
        /// Creates a BindRequest with simple bind for Active Directory Domain Services(AD DS).
        /// </summary>
        /// <param name="username">User name which doesn't include domain prefix.</param>
        /// <param name="password">Password of user.</param>
        /// <param name="domainNetbiosName">NetBIOS domain name(with suffix like ".com" removed).</param>
        /// <returns>The packet that contains the response.</returns>
        public virtual AdtsBindRequestPacket CreateSimpleBindRequest(
            string username,
            string password,
            string domainNetbiosName)
        {
            if (security == null)
            {
                this.security = new AdtsLdapSimpleSecurityLayer();
            }

            this.context.IsSicily = false;

            return this.encoder.CreateSimpleBindRequest(this.context, username, password, domainNetbiosName);
        }


        /// <summary>
        /// Creates a BindRequest with simple bind.
        /// </summary>
        /// <param name="name">
        /// The name field of BindRequest, see TD Section 5.1.1.1.1 for full list of legal names.
        /// </param>
        /// <param name="password">The password credential of the object.</param>
        /// <returns>The packet that contains the response.</returns>
        public virtual AdtsBindRequestPacket CreateSimpleBindRequest(
            string name,
            string password)
        {
            if (security == null)
            {
                this.security = new AdtsLdapSimpleSecurityLayer();
            }

            return this.encoder.CreateSimpleBindRequest(this.context, name, password);
        }


        /// <summary>
        /// Creates a BindRequest with SASL bind. This method is for LDAP v3 only.
        /// Note that for GSS-SPNEGO with NTLM, two rounds of bind requests is required.
        /// </summary>
        /// <param name="mechanism">Authentication mechanism used, e.g., GSS-SPNEGO, </param>
        /// <param name="credential">The credential to be sent, it can be calculated with SSPI.</param>
        /// <returns>The packet that contains the response.</returns>
        public virtual AdtsBindRequestPacket CreateSaslBindRequest(string mechanism, byte[] credential)
        {
            this.context.IsSicily = false;

            return this.encoder.CreateSaslBindRequest(this.context, mechanism, credential);
        }


        /// <summary>
        /// Creates a sicily package discovery BindRequest packet.
        /// </summary>
        /// <returns>The sicily package discovery BindRequest.</returns>
        public virtual AdtsBindRequestPacket CreateSicilyPackageDiscoveryBindRequest()
        {
            this.context.IsSicily = true;

            return this.encoder.CreateSicilyPackageDiscoveryBindRequest(this.context);
        }


        /// <summary>
        /// Creates a sicily negotiate bind request packet.
        /// </summary>
        /// <param name="authName">Authentication name, such as "NTLM", etc.</param>
        /// <param name="credential">The credential to be sent, it can be calculated with SSPI.</param>
        /// <returns>The sicily bind request packet.</returns>
        public virtual AdtsBindRequestPacket CreateSicilyNegotiateBindRequest(string authName, byte[] credential)
        {
            // set the IsSicily flag for decorder to decode sicily response packet.
            this.context.IsSicily = true;

            return this.encoder.CreateSicilyNegotiateBindRequest(this.context, authName, credential);
        }


        /// <summary>
        /// Creates a sicily response bind request packet.
        /// </summary>
        /// <param name="credential">The credential to be sent, it can be calculated with SSPI.</param>
        /// <returns>The packet that contains the response.</returns>
        public virtual AdtsBindRequestPacket CreateSicilyResponseBindRequest(byte[] credential)
        {
            // set the IsSicily flag for decorder to decode sicily response packet.
            this.context.IsSicily = true;

            return this.encoder.CreateSicilyResponseBindRequest(this.context, credential);
        }


        /// <summary>
        /// Creates an AddRequest packet.
        /// </summary>
        /// <param name="objectDn">The DN of the object to be added.</param>
        /// <param name="attributes">Attributes to be set.</param>
        /// <returns>The packet that contains the response.</returns>
        public virtual AdtsAddRequestPacket CreateAddRequest(
            string objectDn,
            params KeyValuePair<string, string[]>[] attributes)
        {
            return this.encoder.CreateAddRequest(this.context, objectDn, attributes);
        }


        /// <summary>
        /// Creates a DelRequest packet.
        /// </summary>
        /// <param name="objectDn">The DN of the object to be deleted.</param>
        /// <returns>The packet that contains the response.</returns>
        public virtual AdtsDelRequestPacket CreateDelRequest(string objectDn)
        {
            return this.encoder.CreateDelRequest(this.context, objectDn);
        }


        /// <summary>
        /// Creates an AbandonRequest packet.
        /// </summary>
        /// <param name="messageId">The ID of message to be abandoned.</param>
        /// <returns>The packet that contains the response.</returns>
        public virtual AdtsAbandonRequestPacket CreateAbandonRequest(long messageId)
        {
            return this.encoder.CreateAbandonRequest(this.context, messageId);
        }


        /// <summary>
        /// Creates a CompareRequest packet.
        /// </summary>
        /// <param name="objectDn">The DN of the object to be compared.</param>
        /// <param name="attributeName">The name of the attribute.</param>
        /// <param name="attributeValue">The value of the attribute.</param>
        /// <returns>The packet that contains the response.</returns>
        public virtual AdtsCompareRequestPacket CreateCompareRequest(
            string objectDn,
            string attributeName,
            string attributeValue)
        {
            return this.encoder.CreateCompareRequest(this.context, objectDn, attributeName, attributeValue);
        }


        /// <summary>
        /// Creates an ExtendedRequest packet.
        /// </summary>
        /// <param name="requestName">The request name of the extended operation.</param>
        /// <param name="requestValue">The request value of the extended operation.</param>
        /// <returns>The packet that contains the response.</returns>
        public virtual AdtsExtendedRequestPacket CreateExtendedRequest(string requestName, byte[] requestValue)
        {
            return this.encoder.CreateExtendedRequest(this.context, requestName, requestValue);
        }


        /// <summary>
        /// Creates an UnbindRequest packet.
        /// </summary>
        /// <returns>The packet that contains the response.</returns>
        public virtual AdtsUnbindRequestPacket CreateUnbindRequest()
        {
            return this.encoder.CreateUnbindRequest(this.context);
        }


        /// <summary>
        /// Creates a ModifyDNRequest packet.
        /// </summary>
        /// <param name="oldDn">The original DN to be modified.</param>
        /// <param name="newRdn">The new relative DN.</param>
        /// <param name="newParentDn">
        /// The new parent DN. For LDAP v3 only. Ignored when creating LDAP v2 requests.
        /// </param>
        /// <param name="delOldRdn">
        /// Whether to delete old RDN. For LDAP v3 only. Ignored when creating LDAP v2 requests.
        /// </param>
        /// <returns>The packet that contains the response.</returns>
        public virtual AdtsModifyDnRequestPacket CreateModifyDnRequest(
            string oldDn,
            string newRdn,
            string newParentDn,
            bool delOldRdn)
        {
            return this.encoder.CreateModifyDnRequest(this.context, oldDn, newRdn, newParentDn, delOldRdn);
        }


        /// <summary>
        /// Creates a ModifyRequest packet.
        /// </summary>
        /// <param name="objectDn">The DN of object to be modified.</param>
        /// <param name="modificationList">Modification list of attributes.</param>
        /// <returns>The packet that contains the response.</returns>
        public virtual AdtsModifyRequestPacket CreateModifyRequest(
            string objectDn,
            params DirectoryAttributeModification[] modificationList)
        {
            return this.encoder.CreateModifyRequest(this.context, objectDn, modificationList);
        }


        /// <summary>
        /// Creates a SearchRequest packet.
        /// </summary>
        /// <param name="dn">The DN to be searched.</param>
        /// <param name="sizeLimit">Size limit for search response.</param>
        /// <param name="timeLimit">
        /// Time limit of search, in seconds, before DC returns an timeLimitExceeded error.
        /// </param>
        /// <param name="scope">Search scope. Base, single level, or subtree.</param>
        /// <param name="dereferenceAliases">Dereference alias options.</param>
        /// <param name="filter">Search filter.</param>
        /// <param name="typesOnly">
        /// Specifies whether the search returns only the attribute names without the attribute values.
        /// </param>
        /// <param name="attributes">The attributes to be retrieved.</param>
        /// <returns>The packet that contains the response.</returns>
        public virtual AdtsSearchRequestPacket CreateSearchRequest(
            string dn,
            long sizeLimit,
            long timeLimit,
            SearchScope scope,
            DereferenceAlias dereferenceAliases,
            Asn1Choice filter,
            bool typesOnly,
            params string[] attributes)
        {
            return this.encoder.CreateSearchRequest(
                this.context,
                dn,
                sizeLimit,
                timeLimit,
                scope,
                dereferenceAliases,
                filter,
                typesOnly,
                attributes);
        }

        #endregion Codec
    }
}