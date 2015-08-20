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
    /// LspUnblockRequest message
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct LspUnblockRequestMsg
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
    /// LspUnblockRequest, sent by LspConsole.
    /// </summary>
    public class LspUnblockRequest : LspMessage
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
        /// Unblock a blocking rule.
        /// </summary>
        /// <param name="session">session info</param>
        public LspUnblockRequest(LspSession session)
            : base(LspMessageType.UnblockRequest)
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
            LspUnblockRequestMsg msg;
            msg.header.messageType = LspMessageType.UnblockRequest;
            msg.header.messageLen = Marshal.SizeOf(typeof(LspUnblockRequestMsg));
            msg.sessionId = this.session.SessionId;

            return TypeMarshal.ToBytes<LspUnblockRequestMsg>(msg);
        }


        /// <summary>
        /// Decode the raw data into the strong type LspMessage.
        /// </summary>
        /// <param name="rawData">Raw data(byte array) to be decoded.</param>
        /// <returns>Strong type LspMessage.</returns>
        [SecurityPermission(SecurityAction.Demand)]
        public static LspUnblockRequest Decode(byte[] rawData)
        {
            if (rawData == null || rawData.Length != Marshal.SizeOf(typeof(LspUnblockRequestMsg)))
            {
                return null;
            }

            LspUnblockRequestMsg msg = TypeMarshal.ToStruct<LspUnblockRequestMsg>(rawData);
            return new LspUnblockRequest(new LspSession(msg.sessionId));
        }
    }
}
