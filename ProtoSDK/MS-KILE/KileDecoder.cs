// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Net;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Kile
{
    /// <summary>
    /// Kile Decoder
    /// </summary>
    internal class KileDecoder
    {
        /// <summary>
        /// Specify whether currently act as a client or server.
        /// </summary>
        internal bool isClientRole;

        /// <summary>
        /// Kile Client Context
        /// (which maintains certain state variables for kile client)
        /// </summary>
        internal KileClientContext clientContext;

        /// <summary>
        /// Kile Server Context
        /// (which maintains certain state variables for kile server currently operated context)
        /// </summary>
        internal KileServerContext serverContext;

        /// <summary>
        /// Kile Server Context List
        /// </summary>
        internal Dictionary<KileConnection, KileServerContext> serverContextList;

        /// <summary>
        /// Transport connection type. TCP/UDP
        /// </summary>
        internal KileConnectionType connectionType;

        #region Constructor

        /// <summary>
        /// Create a KileDecoder instance, to support KileDecrypter.
        /// </summary>
        internal KileDecoder()
        {
        }


        /// <summary>
        /// Kile Decoder Constructor
        /// </summary>
        /// <param name="clientContext">Kile Client Context</param>
        internal KileDecoder(KileClientContext clientContext)
        {
            this.clientContext = clientContext;
            isClientRole = true;
        }


        /// <summary>
        /// Kile Decoder Constructor
        /// </summary>
        /// <param name="serverContextList">Kile Server Context List</param>
        /// <param name="transportType">Transport type, TCP or UDP.</param>
        internal KileDecoder(
            Dictionary<KileConnection, KileServerContext> serverContextList,
            KileConnectionType transportType)
        {
            this.serverContextList = serverContextList;
            connectionType = transportType;
            isClientRole = false;
        }

        #endregion constructor


        /// <summary>
        /// Decode KILE PDUs from received message bytes
        /// </summary>
        /// <param name="endPoint">An endpoint from which the message bytes are received</param>
        /// <param name="receivedBytes">The received bytes to be decoded</param>
        /// <param name="consumedLength">Length of message bytes consumed by decoder</param>
        /// <param name="expectedLength">Length of message bytes the decoder expects to receive</param>
        /// <returns>The decoded KILE PDUs</returns>
        /// <exception cref="System.FormatException">thrown when a kile message type is unsupported</exception>
        internal KilePdu[] DecodePacketCallback(object endPoint,
                                                byte[] receivedBytes,
                                                out int consumedLength,
                                                out int expectedLength)
        {
            // initialize lengths
            consumedLength = 0;
            expectedLength = 0;

            if (null == receivedBytes || 0 == receivedBytes.Length)
            {
                return null;
            }
            if (!isClientRole)
            {
                serverContext = null;

                if (serverContextList != null)
                {
                    var kileConnection = new KileConnection((IPEndPoint)endPoint);

                    if (!serverContextList.ContainsKey(kileConnection))
                    {
                        serverContext = new KileServerContext();
                        serverContext.TransportType = connectionType;
                        serverContextList.Add(kileConnection, serverContext);
                    }
                    else
                    {
                        serverContext = serverContextList[kileConnection];
                    }
                }
                if (serverContext == null)
                {
                    throw new InvalidOperationException("The kileConnection related context does not exist.");
                }
            }

            // TCP has a 4 bytes length header, while UDP has not
            byte[] pduBytes = receivedBytes;

            if ((isClientRole && clientContext.TransportType == KileConnectionType.TCP)
                || (!isClientRole && serverContext.TransportType == KileConnectionType.TCP))
            {
                // insufficient data, needs to receive more
                if (receivedBytes.Length < sizeof(int))
                {
                    return null;
                }

                // get pdu data length
                byte[] lengthBytes = ArrayUtility.SubArray(receivedBytes, 0, sizeof(int));
                Array.Reverse(lengthBytes);
                int pduLength = BitConverter.ToInt32(lengthBytes, 0);

                // insufficient data, needs to receive more
                expectedLength = sizeof(int) + pduLength;
                if (receivedBytes.Length < expectedLength)
                {
                    return null;
                }

                // check if it is a krb zero message
                if (pduLength == 0 && receivedBytes.Length == sizeof(int))
                {
                    consumedLength = sizeof(int);
                    var krbZeroPdu = new KrbZero(clientContext);
                    return new KilePdu[] { krbZeroPdu };
                }

                // remove length header from pdu bytes
                pduBytes = ArrayUtility.SubArray<byte>(receivedBytes, sizeof(int), pduLength);
            }
            else
            {
                // UDP has no length header
                expectedLength = pduBytes.Length;
            }

            // get message type
            // (the lower 5 bits indicates its kile message type)
            MsgType kileMessageType = (MsgType)(pduBytes[0] & 0x1f);

            // decode according to message type
            consumedLength = expectedLength;
            KilePdu pdu = null;

            switch (kileMessageType)
            {
                case MsgType.KRB_AS_REQ:
                    pdu = new KileAsRequest(serverContext);
                    break;

                case MsgType.KRB_AS_RESP:
                    pdu = new KileAsResponse(clientContext);
                    break;

                case MsgType.KRB_TGS_REQ:
                    pdu = new KileTgsRequest(serverContext);
                    break;

                case MsgType.KRB_TGS_RESP:
                    pdu = new KileTgsResponse(clientContext);
                    break;

                case MsgType.KRB_ERROR:
                    pdu = new KileKrbError();
                    break;

                default:
                    throw new FormatException(
                        "Unsupported Message Type: " + kileMessageType.ToString());
            }
            pdu.FromBytes(pduBytes);

            // update context
            if (isClientRole)
            {
                clientContext.UpdateContext(pdu);
            }
            else
            {
                serverContext.UpdateContext(pdu);
            }

            return new KilePdu[] { pdu };
        }
    }
}
