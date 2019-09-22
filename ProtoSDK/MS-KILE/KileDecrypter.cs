// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Kile
{
    public class KileDecrypter
    {
        //fale endpoint to communicate between client and server
        private static readonly IPEndPoint FAKE_ENDPOINT = new IPEndPoint(IPAddress.Loopback, 0);

        //core decoder for negotiate AS and TGS
        private KileDecoder kileDecoder;


        /// <summary>
        /// Create a KileDecrypter instance.
        /// User should call following methods in sequence to initialize: AsExchange, TgsExchange and ApExchange.
        /// After exchanges are done, call DecryptRequest or DecryptResponse from first encrypted message to last. 
        /// Do not skip any request or response.
        /// </summary>
        /// <param name="domain">
        /// The realm part of the client's principal identifier.
        /// This argument cannot be null.
        /// </param>
        /// <param name="cName">
        /// The account to logon the remote machine. Either user account or computer account.
        /// This argument cannot be null.
        /// </param>
        /// <param name="password">
        /// The password of the user. 
        /// This argument cannot be null.
        /// </param>
        /// <param name="accountType">
        /// The type of the logon account. User or Computer.
        /// </param>
        /// <param name="connectionType">
        /// The connection type, TCP or UDP.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public KileDecrypter(
            string domain, 
            string cName, 
            string password, 
            KileAccountType accountType, 
            KileConnectionType connectionType)
        {
            if (domain == null)
            {
                throw new ArgumentNullException(nameof(domain));
            }
            if (cName == null)
            {
                throw new ArgumentNullException(nameof(cName));
            }
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            kileDecoder = new KileDecoder();
            kileDecoder.connectionType = connectionType;

            string salt = KileRole.GenerateSalt(domain, cName, accountType);

            kileDecoder.clientContext = new KileClientContext();
            kileDecoder.clientContext.Password = password;
            kileDecoder.clientContext.TransportType = connectionType;
            kileDecoder.clientContext.Salt = salt;

            kileDecoder.serverContext = new KileServerContext();
            kileDecoder.serverContext.TransportType = connectionType;
            kileDecoder.serverContext.Salt = salt;
            kileDecoder.serverContextList = new Dictionary<KileConnection, KileServerContext>(new KileServerContextComparer());
            kileDecoder.serverContextList.Add(new KileConnection(FAKE_ENDPOINT), kileDecoder.serverContext);
        }


        /// <summary>
        /// Exchange AS or TGS token with KDC.
        /// </summary>
        /// <param name="token">A token</param>
        /// <param name="sentFromClient">True if the token is a request, false is a response</param>
        /// <returns>Kerberos PDU</returns>
        private KilePdu ExchangeLegWithKdc(byte[] token, bool sentFromClient)
        {
            kileDecoder.isClientRole = !sentFromClient;

            int consumedLength;
            int expectedLength;

            KilePdu[] pduList = kileDecoder.DecodePacketCallback(
                FAKE_ENDPOINT,
                token,
                out consumedLength,
                out expectedLength);

            if (pduList == null || pduList.Length != 1 || consumedLength == 0)
            {
                throw new InvalidOperationException("Unable to decode data buffer");
            }

            KilePdu pdu = pduList[0];

            if (sentFromClient)
            {
                kileDecoder.clientContext.UpdateContext(pdu);
            }
            else
            {
                kileDecoder.serverContext.UpdateContext(pdu);
            }

            return pdu;
        }


        /// <summary>
        /// Decrypt a request or response
        /// </summary>
        /// <param name="token">A token containing encrypted data.</param>
        /// <param name="sentFromClient">True if the token is a request, false is a response</param>
        /// <returns>Plain-text data.</returns>
        private byte[] Decrypt(byte[] token, bool sentFromClient)
        {
            KileContext context = sentFromClient ? (KileContext)kileDecoder.serverContext : (KileContext)kileDecoder.clientContext;
            KilePdu pdu = KileRole.GssUnWrap(context, token);

            Token4121 token4121Pdu = pdu as Token4121;
            if (token4121Pdu != null)
            {
                return token4121Pdu.Data;
            }

            Token1964_4757 token1964or4757Pdu = pdu as Token1964_4757;
            if (token1964or4757Pdu != null)
            {
                return token1964or4757Pdu.Data;
            }

            throw new InvalidOperationException("Token type is not supported.");
        }


        /// <summary>
        /// Decrypt a request or response
        /// </summary>
        /// <param name="securityBuffers">Security buffers containing encrypted data.</param>
        /// <param name="sentFromClient">True if the token is a request, false is a response</param>
        /// <returns>Plain-text data.</returns>
        private byte[] Decrypt(SecurityBuffer[] securityBuffers, bool sentFromClient)
        {
            KileContext context = sentFromClient ? (KileContext)kileDecoder.serverContext : (KileContext)kileDecoder.clientContext;
            KileRole.GssUnWrapEx(context, securityBuffers);
            return SspiUtility.ConcatenateReadWriteSecurityBuffers(securityBuffers, SecurityBufferType.Data);
        }


        /// <summary>
        /// Verify a request or response
        /// </summary>
        /// <param name="securityBuffers">Security buffers containing encrypted data.</param>
        /// <param name="sentFromClient">True if the token is a request, false is a response</param>
        /// <returns>True if verify succeed; otherwise, false.</returns>
        private bool Verify(SecurityBuffer[] securityBuffers, bool sentFromClient)
        {
            KileContext context = sentFromClient ? (KileContext)kileDecoder.serverContext : (KileContext)kileDecoder.clientContext;
            KilePdu pdu;
            return KileRole.GssVerifyMicEx(context, securityBuffers, out pdu);
        }


        /// <summary>
        /// AS exchange.
        /// Typically AS request PDU and AS response PDU are heading with 4 bytes length for TCP.
        /// </summary>
        /// <param name="asReq">AS request</param>
        /// <param name="asRep">AS response</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when asReq or asRep is null.
        /// </exception>
        public void AsExchange(byte[] asReq, byte[] asRep)
        {
            if (asReq == null)
            {
                throw new ArgumentNullException(nameof(asReq));
            }
            if (asRep == null)
            {
                throw new ArgumentNullException(nameof(asRep));
            }

            ExchangeLegWithKdc(asReq, true);
            ExchangeLegWithKdc(asRep, false);
        }


        /// <summary>
        /// TGS exchange.
        /// Typically TGS request PDU and TGS response PDU are heading with 4 bytes length for TCP.
        /// </summary>
        /// <param name="tgsReq">TGS request</param>
        /// <param name="tgsRep">TGS response</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when tgsReq or tgsRep is null.
        /// </exception>
        public void TgsExchange(byte[] tgsReq, byte[] tgsRep)
        {
            if (tgsReq == null)
            {
                throw new ArgumentNullException(nameof(tgsReq));
            }
            if (tgsRep == null)
            {
                throw new ArgumentNullException(nameof(tgsRep));
            }

            ExchangeLegWithKdc(tgsReq, true);
            ExchangeLegWithKdc(tgsRep, false);
        }


        /// <summary>
        /// AP exchange.
        /// Typically AP request and AP response start with 
        /// GSSAPI token (asn.1 header + 1.2.840.113554.1.2.2), 
        /// or a TokId (Krb5ApReq:0x100)
        /// </summary>
        /// <param name="apReq">AP request</param>
        /// <param name="apRep">AP response</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when apReq or apRep is null.
        /// </exception>
        public void ApExchange(byte[] apReq, byte[] apRep)
        {
            if (apReq == null)
            {
                throw new ArgumentNullException(nameof(apReq));
            }
            if (apRep == null)
            {
                throw new ArgumentNullException(nameof(apRep));
            }

            var apReqPdu = new KileApRequest(kileDecoder.serverContext);
            apReqPdu.FromBytes(apReq);
            kileDecoder.clientContext.UpdateContext(apReqPdu);

            var apRepPdu = new KileApResponse(kileDecoder.clientContext);
            apRepPdu.FromBytes(apRep);
            kileDecoder.serverContext.UpdateContext(apRepPdu);
        }


        /// <summary>
        /// AP exchange, 3rd leg.
        /// Typically AP request and AP response start with 
        /// GSSAPI token (asn.1 header + 1.2.840.113554.1.2.2), 
        /// or a TokId (Krb5ApReq:0x100)
        /// </summary>
        /// <param name="apRep">AP response, sent from client</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when apRep is null.
        /// </exception>
        public void ApExchange(byte[] apRep)
        {
            if (apRep == null)
            {
                throw new ArgumentNullException(nameof(apRep));
            }

            var apRepPdu = new KileApResponse(kileDecoder.serverContext);
            apRepPdu.FromBytes(apRep);
            kileDecoder.clientContext.UpdateContext(apRepPdu);
        }


        /// <summary>
        /// Decrypt a request.
        /// AS, TGS and AP exchanges should be done before calling decrypt.
        /// User should call decrypt one by one, do not skip any request.
        /// </summary>
        /// <param name="token">A Kerberos token containing encrypted data.</param>
        /// <returns>Plain-text data.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when token is null.
        /// </exception>
        public byte[] DecryptRequest(byte[] token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            return Decrypt(token, true);
        }


        /// <summary>
        /// Decrypt a response.
        /// AS, TGS and AP exchanges should be done before calling decrypt.
        /// User should call decrypt one by one, do not skip any response.
        /// </summary>
        /// <param name="token">A Kerberos token containing encrypted data.</param>
        /// <returns>Plain-text data.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when token is null.
        /// </exception>
        public byte[] DecryptResponse(byte[] token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            return Decrypt(token, false);
        }


        /// <summary>
        /// Decrypt a request.
        /// AS, TGS and AP exchanges should be done before calling decrypt.
        /// User should call decrypt one by one, do not skip any request.
        /// </summary>
        /// <param name="securityBuffers">Security buffers</param>
        /// <returns>Plain-text data (including padding).</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when securityBuffers is null.
        /// </exception>
        public byte[] DecryptRequest(params SecurityBuffer[] securityBuffers)
        {
            if (securityBuffers == null)
            {
                throw new ArgumentNullException(nameof(securityBuffers));
            }

            return Decrypt(securityBuffers, true);
        }


        /// <summary>
        /// Decrypt a response.
        /// AS, TGS and AP exchanges should be done before calling decrypt.
        /// User should call decrypt one by one, do not skip any response.
        /// </summary>
        /// <param name="securityBuffers">Security buffers</param>
        /// <returns>Plain-text data (including padding).</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when securityBuffers is null.
        /// </exception>
        public byte[] DecryptResponse(params SecurityBuffer[] securityBuffers)
        {
            if (securityBuffers == null)
            {
                throw new ArgumentNullException(nameof(securityBuffers));
            }

            return Decrypt(securityBuffers, false);
        }


        /// <summary>
        /// Verify a request.
        /// AS, TGS and AP exchanges should be done before calling verify.
        /// User should call decrypt one by one, do not skip any request.
        /// </summary>
        /// <param name="securityBuffers">Security buffers</param>
        /// <returns>True if verify succeed; otherwise, false</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when securityBuffers is null.
        /// </exception>
        public bool VerifyRequest(params SecurityBuffer[] securityBuffers)
        {
            if (securityBuffers == null)
            {
                throw new ArgumentNullException(nameof(securityBuffers));
            }

            return Verify(securityBuffers, true);
        }


        /// <summary>
        /// Verify a response.
        /// AS, TGS and AP exchanges should be done before calling verify.
        /// User should call decrypt one by one, do not skip any response.
        /// </summary>
        /// <param name="securityBuffers">Security buffers</param>
        /// <returns>True if verify succeed; otherwise, false</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when securityBuffers is null.
        /// </exception>
        public bool VerifyResponse(params SecurityBuffer[] securityBuffers)
        {
            if (securityBuffers == null)
            {
                throw new ArgumentNullException(nameof(securityBuffers));
            }

            return Verify(securityBuffers, false);
        }
    }
}
