// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Transport;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.DirectoryServices.Protocols;
using System.Net;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts
{
    /// <summary>
    /// LDAP server. Note that one server instance listens on one port only.
    /// </summary>
    public partial class AdtsLdapServer : IDisposable
    {
        #region Private members

        /// <summary>
        /// Default buffer size.
        /// </summary>
        private const int DefaultBufferSize = 1024 * 8;

        /// <summary>
        /// The port that server listens on.
        /// </summary>
        private ushort listenPort;

        /// <summary>
        /// Specifies the connection is TCP(including SSL/TLS) or UDP.
        /// </summary>
        private bool isTcp;

        /// <summary>
        /// The transport stack instance.
        /// </summary>
        private TransportStack transportStack;

        /// <summary>
        /// The context manager instance.
        /// </summary>
        private AdtsLdapContextManager contextManager;

        /// <summary>
        /// The encoder that creates LDAP v2 messages.
        /// </summary>
        private AdtsLdapV2Encoder encoderv2;

        /// <summary>
        /// The encoder that creates LDAP v3 messages.
        /// </summary>
        private AdtsLdapV3Encoder encoderv3;

        /// <summary>
        /// The decoder that decodes messages(both v2 and v3).
        /// </summary>
        private AdtsLdapServerDecoder decoder;

        #endregion Private members

        #region Public methods
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="port">The port to be connected</param>
        public AdtsLdapServer(ushort port, AdtsLdapConnectionType connectionType)
        {
            this.listenPort = port;
            if (connectionType == AdtsLdapConnectionType.Tcp
                || connectionType == AdtsLdapConnectionType.TlsOrSsl)
            {
                this.isTcp = true;
            }
            else
            {
                this.isTcp = false;
            }

            this.encoderv2 = new AdtsLdapV2Encoder();
            this.encoderv3 = new AdtsLdapV3Encoder();
            this.contextManager = new AdtsLdapContextManager();
            this.decoder = new AdtsLdapServerDecoder(this);
        }


        /// <summary>
        /// Starts the server to listen on specified port.
        /// </summary>
        public void Start()
        {
            if (transportStack == null)
            {
                SocketTransportConfig transportConfig = new SocketTransportConfig();
                transportConfig.LocalIpAddress = IPAddress.Any;
                transportConfig.LocalIpPort = this.listenPort;
                transportConfig.Role = Role.Server;
                transportConfig.BufferSize = DefaultBufferSize;
                transportConfig.MaxConnections = int.MaxValue;
                transportConfig.Type = isTcp ? StackTransportType.Tcp : StackTransportType.Udp;

                this.transportStack = new TransportStack(transportConfig, decoder.DecodeLdapPacketCallBack);
            }

            this.transportStack.Start();
        }


        /// <summary>
        /// Stops the server.
        /// </summary>
        public void Stop()
        {
            if (!this.disposed)
            {
                if (this.transportStack != null)
                {
                    this.transportStack.Dispose();
                    this.transportStack = null;
                }
                if (this.contextManager != null)
                {
                    this.contextManager.Clear();
                }
            }
        }


        /// <summary>
        /// Disconnects with specified client. The corresponding context will be removed.
        /// </summary>
        /// <param name="context">The context that contains the client information.</param>
        public void Disconnect(AdtsLdapContext context)
        {
            if (this.isTcp)
            {
                this.transportStack.Disconnect(context.RemoteAddress);
            }
            this.contextManager.RemoveContext(context.RemoteAddress, this.isTcp);
        }


        /// <summary>
        /// Sends a packet.
        /// </summary>
        /// <param name="context">The context that contains related info about client.</param>
        /// <param name="packet">The packet to be sent.</param>
        public void SendPacket(AdtsLdapContext context, AdtsLdapPacket packet)
        {
            transportStack.SendPacket(context.RemoteAddress, packet);
        }


        /// <summary>
        /// Sends packet data to client (to keep compliant with Stack SDK conventions).
        /// </summary>
        /// <param name="context">The context that contains related info about client.</param>
        /// <param name="packetData">The packet data.</param>
        public void SendBytes(AdtsLdapContext context, byte[] packetData)
        {
            transportStack.SendBytes(context.RemoteAddress, packetData);
        }


        /// <summary>
        /// to receive bytes from connection.<para/>
        /// the transport must be TcpServer or NetbiosServer.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that specifies the timeout for this operation.
        /// </param>
        /// <param name="maxCount">
        /// an int value that specifies the maximum count of expect bytes.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that indicates the connection to received data from.<para/>
        /// if Tcp, it's an IPEndPoint that specifies the target endpoint.<para/>
        /// if Netbios, it's an int value that specifies the client session id.
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
        public virtual byte[] ExpectBytes(TimeSpan timeout, int maxCount, AdtsLdapContext remoteEndPoint)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TransportStack");
            }

            if (maxCount < 0)
            {
                throw new ArgumentException("max count must not be negative", "maxCount");
            }

            return this.transportStack.ExpectBytes(timeout, maxCount, remoteEndPoint.RemoteAddress);
        }


        /// <summary>
        /// Expects a packet.
        /// </summary>
        /// <param name="timeout">The timeout to be waited.</param>
        /// <param name="context">The context to be returned.</param>
        /// <returns>The packet received.</returns>
        public AdtsLdapPacket ExpectPacket(TimeSpan timeout, out AdtsLdapContext context)
        {
            while (true)
            {
                TransportEvent transportEvent = this.transportStack.ExpectTransportEvent(timeout);
                if (transportEvent.EventType == EventType.ReceivedPacket)
                {
                    AdtsLdapPacket packet = (AdtsLdapPacket)transportEvent.EventObject;
                    context = this.contextManager.GetContext((IPEndPoint)transportEvent.EndPoint, this.isTcp);

                    return packet;
                }
                else if (transportEvent.EventType == EventType.Disconnected)
                {
                    // Remove context if disconnected.
                    this.transportStack.Disconnect((IPEndPoint)transportEvent.EndPoint);
                    context = this.contextManager.GetContext((IPEndPoint)transportEvent.EndPoint, this.isTcp);
                    this.contextManager.RemoveContext((IPEndPoint)transportEvent.EndPoint, this.isTcp);

                    return null;
                }
                else if (transportEvent.EventType == EventType.Exception)
                {
                    throw (Exception)transportEvent.EventObject;
                }
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
                    if (this.contextManager != null)
                    {
                        this.contextManager.Dispose();
                        this.contextManager = null;
                    }
                    this.encoderv2 = null;
                    this.encoderv3 = null;
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
        ~AdtsLdapServer()
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
        public void AddDirectoryControls(AdtsLdapPacket packet, params DirectoryControl[] controls)
        {
            if (packet.ldapMessagev2 != null)
            {
                throw new NotSupportedException("Extended controls are only supported for LDAP v3 packets.");
            }
            this.encoderv3.AddDirectoryControls(packet, controls);
        }


        /// <summary>
        /// Creates a BindResponse for normal bindings, SASL bindings and sicily bindings.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="resultCode">Result code of previous request, as specified in RFC 2251.</param>
        /// <param name="matchedDn">
        /// Matched DN. Required for normal bindings, SASL bindings; but optional for sicily bind.</param>
        /// <param name="errorMessage">Error message for result code. Required.</param>
        /// <param name="referral">Referral. Optional and for LDAP v3 only.</param>
        /// <param name="serverCredentials">Server credentials, optional for normal and sicily bind.</param>
        /// <returns>The packet that contains the response.</returns>
        public AdtsBindResponsePacket CreateBindResponse(
            AdtsLdapContext context,
            ResultCode resultCode,
            string matchedDn,
            string errorMessage,
            string[] referral,
            byte[] serverCredentials)
        {
            if (context.Security == null)
            {
                context.Security = new AdtsLdapSimpleSecurityLayer();
            }

            if (context.ServerVersion == AdtsLdapVersion.V2)
            {
                return this.encoderv2.CreateBindResponse(
                    context,
                    resultCode,
                    matchedDn,
                    errorMessage,
                    referral,
                    serverCredentials);
            }
            else
            {
                return this.encoderv3.CreateBindResponse(
                    context,
                    resultCode,
                    matchedDn,
                    errorMessage,
                    referral,
                    serverCredentials);
            }
        }


        /// <summary>
        /// Creates a SicilyBindResponse packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="resultCode">Result code of previous request, as specified in RFC 2251.</param>
        /// <param name="serverCredentials">Server credentials, optional for normal and sicily bind.</param>
        /// <param name="errorMessage">Error message for result code. Required.</param>
        /// <returns>The packet that contains the response.</returns>
        public AdtsSicilyBindResponsePacket CreateSicilyBindResponse(
            AdtsLdapContext context,
            ResultCode resultCode,
            byte[] serverCredentials,
            string errorMessage)
        {
            if (context.ServerVersion == AdtsLdapVersion.V2)
            {
                return this.encoderv2.CreateSicilyBindResponse(
                    context,
                    resultCode,
                    serverCredentials,
                    errorMessage);
            }
            else
            {
                return this.encoderv3.CreateSicilyBindResponse(
                    context,
                    resultCode,
                    serverCredentials,
                    errorMessage);
            }
        }


        /// <summary>
        /// Creates an AddResponse packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="resultCode">Result code of previous request, as specified in RFC 2251.</param>
        /// <param name="matchedDn">Matched DN.</param>
        /// <param name="errorMessage">Error message for result code. Required.</param>
        /// <param name="referral">Referral. Optional. Used for LDAP v3 only.</param>
        /// <returns>The packet that contains the response.</returns>
        public AdtsAddResponsePacket CreateAddResponse(
            AdtsLdapContext context,
            ResultCode resultCode,
            string matchedDn,
            string errorMessage,
            string[] referral)
        {
            if (context.ServerVersion == AdtsLdapVersion.V2)
            {
                return this.encoderv2.CreateAddResponse(
                    context,
                    resultCode,
                    matchedDn,
                    errorMessage,
                    referral);
            }
            else
            {
                return this.encoderv3.CreateAddResponse(
                    context,
                    resultCode,
                    matchedDn,
                    errorMessage,
                    referral);
            }
        }


        /// <summary>
        /// Creates a CompareResponse packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="resultCode">Result code of previous request, as specified in RFC 2251.</param>
        /// <param name="matchedDn">Matched DN.</param>
        /// <param name="errorMessage">Error message for result code. Required.</param>
        /// <param name="referral">Referral. Optional. Used for LDAP v3 only.</param>
        /// <returns>The packet that contains the response.</returns>
        public AdtsCompareResponsePacket CreateCompareResponse(
            AdtsLdapContext context,
            ResultCode resultCode,
            string matchedDn,
            string errorMessage,
            string[] referral)
        {
            if (context.ServerVersion == AdtsLdapVersion.V2)
            {
                return this.encoderv2.CreateCompareResponse(
                    context,
                    resultCode,
                    matchedDn,
                    errorMessage,
                    referral);
            }
            else
            {
                return this.encoderv3.CreateCompareResponse(
                    context,
                    resultCode,
                    matchedDn,
                    errorMessage,
                    referral);
            }
        }


        /// <summary>
        /// Creates a DelResponse packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="resultCode">Result code of previous request, as specified in RFC 2251.</param>
        /// <param name="matchedDn">Matched DN.</param>
        /// <param name="errorMessage">Error message for result code. Required.</param>
        /// <param name="referral">Referral. Optional. Used for LDAP v3 only.</param>
        /// <returns>The packet that contains the response.</returns>
        public AdtsDelResponsePacket CreateDelResponse(
            AdtsLdapContext context,
            ResultCode resultCode,
            string matchedDn,
            string errorMessage,
            string[] referral)
        {
            if (context.ServerVersion == AdtsLdapVersion.V2)
            {
                return this.encoderv2.CreateDelResponse(
                    context,
                    resultCode,
                    matchedDn,
                    errorMessage,
                    referral);
            }
            else
            {
                return this.encoderv3.CreateDelResponse(
                    context,
                    resultCode,
                    matchedDn,
                    errorMessage,
                    referral);
            }
        }


        /// <summary>
        /// Creates a ModifyResponse packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="resultCode">Result code of previous request, as specified in RFC 2251.</param>
        /// <param name="matchedDn">Matched DN.</param>
        /// <param name="errorMessage">Error message for result code. Required.</param>
        /// <param name="referral">Referral. Optional. Used for LDAP v3 only.</param>
        /// <returns>The packet that contains the response.</returns>
        public AdtsModifyResponsePacket CreateModifyResponse(
            AdtsLdapContext context,
            ResultCode resultCode,
            string matchedDn,
            string errorMessage,
            string[] referral)
        {
            if (context.ServerVersion == AdtsLdapVersion.V2)
            {
                return this.encoderv2.CreateModifyResponse(
                    context,
                    resultCode,
                    matchedDn,
                    errorMessage,
                    referral);
            }
            else
            {
                return this.encoderv3.CreateModifyResponse(
                    context,
                    resultCode,
                    matchedDn,
                    errorMessage,
                    referral);
            }
        }


        /// <summary>
        /// Creates a ModifyDnResponse packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="resultCode">Result code of previous request, as specified in RFC 2251.</param>
        /// <param name="matchedDn">Matched DN.</param>
        /// <param name="errorMessage">Error message for result code. Required.</param>
        /// <param name="referral">Referral. Optional. Used for LDAP v3 only.</param>
        /// <returns>The packet that contains the response.</returns>
        public AdtsModifyDnResponsePacket CreateModifyDnResponse(
            AdtsLdapContext context,
            ResultCode resultCode,
            string matchedDn,
            string errorMessage,
            string[] referral)
        {
            if (context.ServerVersion == AdtsLdapVersion.V2)
            {
                return this.encoderv2.CreateModifyDnResponse(
                    context,
                    resultCode,
                    matchedDn,
                    errorMessage,
                    referral);
            }
            else
            {
                return this.encoderv3.CreateModifyDnResponse(
                    context,
                    resultCode,
                    matchedDn,
                    errorMessage,
                    referral);
            }
        }


        /// <summary>
        /// Creates an ExtendedResponse packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="resultCode">Result code of previous request, as specified in RFC 2251.</param>
        /// <param name="matchedDn">Matched DN.</param>
        /// <param name="errorMessage">Error message for result code. Required.</param>
        /// <param name="referral">Referral. Optional. Used for LDAP v3 only.</param>
        /// <returns>The packet that contains the response.</returns>
        /// <exception cref="NotSupportedException">
        /// Thrown when trying to create an LDAP v2 ExtendedResponse packet.
        /// </exception>
        public AdtsExtendedResponsePacket CreateExtendedResponse(
            AdtsLdapContext context,
            ResultCode resultCode,
            string matchedDn,
            string errorMessage,
            string[] referral)
        {
            if (context.ServerVersion == AdtsLdapVersion.V2)
            {
                throw new NotSupportedException();
            }
            else
            {
                return this.encoderv3.CreateExtendedResponse(
                    context,
                    resultCode,
                    matchedDn,
                    errorMessage,
                    referral);
            }
        }


        /// <summary>
        /// Creates a SearchResultEntry packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="matchedDn">Matched DN.</param>
        /// <param name="attributes">The attributes and values that are contained in the entry.</param>
        /// <returns>The packet that contains the response.</returns>
        public AdtsSearchResultEntryPacket CreateSearchedResultEntry(
            AdtsLdapContext context,
            string matchedDn,
            params KeyValuePair<string, string[]>[] attributes)
        {
            if (context.ServerVersion == AdtsLdapVersion.V2)
            {
                return this.encoderv2.CreateSearchedResultEntry(context, matchedDn, attributes);
            }
            else
            {
                return this.encoderv3.CreateSearchedResultEntry(context, matchedDn, attributes);
            }
        }


        /// <summary>
        /// Creates a SearchResultReference. For LDAP v3 only.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="referenceUrl">The referenced URL.</param>
        /// <returns>The packet that contains the response.</returns>
        /// <exception cref="NotSupportedException">
        /// Thrown when trying to create an LDAP v2 SearchResultReference packet.
        /// </exception>
        /// Disabled fxcop rule because user may pass an invalid URL. A string will be simply ok.
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings")]
        public AdtsSearchResultReferencePacket CreateSearchResultReference(
            AdtsLdapContext context,
            string[] referenceUrls)
        {
            if (context.ServerVersion == AdtsLdapVersion.V2)
            {
                throw new NotSupportedException();
            }
            else
            {
                return this.encoderv3.CreateSearchResultReference(context, referenceUrls);
            }
        }


        /// <summary>
        /// Creates a SearchResultDone packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="resultCode">Result code of previous request, as specified in RFC 2251.</param>
        /// <param name="matchedDn">Matched DN.</param>
        /// <param name="errorMessage">Error message for result code. Required.</param>
        /// <param name="referral">Referral. Optional.</param>
        /// <returns>The packet that contains the response.</returns>
        /// <exception cref="NotSupportedException">
        /// Thrown when trying to create an LDAP v2 SearchResultDone packet.
        /// </exception>
        public AdtsSearchResultDonePacket CreateSearchResultDone(
            AdtsLdapContext context,
            ResultCode resultCode,
            string matchedDn,
            string errorMessage,
            string[] referral)
        {
            if (context.ServerVersion == AdtsLdapVersion.V2)
            {
                throw new NotSupportedException();
            }
            else
            {
                return this.encoderv3.CreateSearchResultDone(
                    context,
                    resultCode,
                    matchedDn,
                    errorMessage,
                    referral);
            }
        }
        #endregion Codec
    }
}