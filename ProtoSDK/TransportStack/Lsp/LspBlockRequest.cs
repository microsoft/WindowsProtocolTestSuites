// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// LspBlockRequest message
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct LspBlockRequestMsg
    {
        /// <summary>
        /// Header
        /// </summary>
        public LspMessageHeader header;

        /// <summary>
        /// Session id
        /// </summary>
        public int sessionId;
    }

    /// <summary>
    /// LspBlockRequest, sent by LspConsole.
    /// </summary>
    public class LspBlockRequest : LspMessage
    {
        private LspSession session;

        /// <summary>
        /// Property of LspSession.
        /// </summary>
        public LspSession Session
        {
            get
            {
                return this.session;
            }
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="session">LspSession</param>
        public LspBlockRequest(LspSession session)
            : base(LspMessageType.BlockRequest)
        {
            this.session = session;
        }


        /// <summary>
        /// Encode the Lsp message into byte array.
        /// </summary>
        /// <returns>Encoded byte array.</returns>
        [SecurityPermission(SecurityAction.Demand)]
        public override byte[] Encode()
        {
            LspBlockRequestMsg msg;
            msg.header.messageType = LspMessageType.BlockRequest;
            msg.header.messageLen = Marshal.SizeOf(typeof(LspBlockRequestMsg));
            msg.sessionId = this.session.SessionId;

            return TypeMarshal.ToBytes<LspBlockRequestMsg>(msg);
        }


        /// <summary>
        /// Decode the raw data into the strong type LspMessage.
        /// </summary>
        /// <param name="rawData">Raw data(byte array) to be decoded.</param>
        /// <returns>Strong type LspMessage.</returns>
        [SecurityPermission(SecurityAction.Demand)]
        public static LspBlockRequest Decode(byte[] rawData)
        {
            if (rawData == null || rawData.Length != Marshal.SizeOf(typeof(LspBlockRequestMsg)))
            {
                return null;
            }

            LspBlockRequestMsg msg = TypeMarshal.ToStruct<LspBlockRequestMsg>(rawData);
            return new LspBlockRequest(new LspSession(msg.sessionId));
        }
    }
}
