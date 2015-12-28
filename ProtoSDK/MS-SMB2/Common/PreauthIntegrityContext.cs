// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// Preauth Integrity context class
    /// </summary>
    public class PreauthIntegrityContext
    {
        // The negotiated preauthentication integrity hash algorithm
        private PreauthIntegrityHashID preauthIntegrityHashID;

        // Preauth Integrity Hash Value computed from Negotiate Request and Response
        private byte[] connectionPreauthIntegrityHashValue;

        // Session Preauth Integrity Hash Value table
        private Dictionary<ulong, byte[]> sessionPreauthIntegrityHashValueTable;

        /// Stores the compeleted sessions
        HashSet<ulong> completedSessionSet;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="piHashId">The context hash ID.</param>
        public PreauthIntegrityContext(PreauthIntegrityHashID piHashId)
        {
            preauthIntegrityHashID = piHashId;
            connectionPreauthIntegrityHashValue = null;
            sessionPreauthIntegrityHashValueTable = new Dictionary<ulong, byte[]>();
            completedSessionSet = new HashSet<ulong>();
        }

        /// <summary>
        /// Update the context from a Negotiate Request or Response messages.
        /// The caller should ensure the messages are input in the right order.
        /// </summary>
        /// <param name="packet">The packet used to update this context. It must be the one of the following types:
        /// Smb2NegotiateRequestPacket, Smb2NegotiateResponsePacket</param>
        public void UpdateConnectionState(Smb2SinglePacket packet)
        {
            //Check the packet type
            if (!(packet is Smb2NegotiateRequestPacket) &&
                !(packet is Smb2NegotiateResponsePacket))
            {
                throw new InvalidCastException("packet must be one of the types: Smb2NegotiateRequestPacket, Smb2NegotiateResponsePacket.");
            }

            //Refer to [MS-SMB2] 3.2.5.2	Receiving an SMB2 NEGOTIATE Response
            if (packet is Smb2NegotiateRequestPacket)
            {
                connectionPreauthIntegrityHashValue = ZeroHashValue;
                byte[] messageBytes = packet.ToBytes();
                byte[] hashData = connectionPreauthIntegrityHashValue.Concat(messageBytes).ToArray();
                connectionPreauthIntegrityHashValue = CreateHashAlgorithm().ComputeHash(hashData);
            }
            else if (packet is Smb2NegotiateResponsePacket)
            {
                byte[] messageBytes = packet.ToBytes();
                byte[] hashData = connectionPreauthIntegrityHashValue.Concat(messageBytes).ToArray();
                connectionPreauthIntegrityHashValue = CreateHashAlgorithm().ComputeHash(hashData);
            }
        }

       /// <summary>
        /// Update the context from a SessioinSetup request and response messages.
        /// The caller should ensure the messages are input in the right order.
        /// </summary>
        /// <param name="packet">The packet used to update this context. It must be the one of the following types:
        /// Smb2SessionSetupRequestPacket, Smb2SessionSetupResponsePacket</param>
        public void UpdateSessionState(ulong sessionId, Smb2SinglePacket packet)
        {
            //Check the packet type
            if (!(packet is Smb2SessionSetupRequestPacket) &&
                !(packet is Smb2SessionSetupResponsePacket))
            {
                throw new InvalidCastException("packet must be one of the types: Smb2SessionSetupRequestPacket, Smb2SessionSetupResponsePacket.");
            }

            if (completedSessionSet.Contains(sessionId))
            {
                return; // Don't update session hash value for reauthetication
            }

            //Refer to [MS-SMB2] 3.2.5.3	Receiving an SMB2 SESSION_SETUP Response
            if (packet is Smb2SessionSetupRequestPacket)
            {
                if (!sessionPreauthIntegrityHashValueTable.ContainsKey(sessionId))
                {
                    sessionPreauthIntegrityHashValueTable.Add(sessionId, connectionPreauthIntegrityHashValue);
                }
                byte[] messageBytes = packet.ToBytes();
                byte[] hashData = sessionPreauthIntegrityHashValueTable[sessionId].Concat(messageBytes).ToArray();
                sessionPreauthIntegrityHashValueTable[sessionId] = CreateHashAlgorithm().ComputeHash(hashData);
            }
            else if (packet is Smb2SessionSetupResponsePacket)
            {
                if (packet.Header.Status == Smb2Status.STATUS_MORE_PROCESSING_REQUIRED)
                {
                    byte[] messageBytes = packet.ToBytes();
                    byte[] hashData = sessionPreauthIntegrityHashValueTable[sessionId].Concat(messageBytes).ToArray();
                    sessionPreauthIntegrityHashValueTable[sessionId] = CreateHashAlgorithm().ComputeHash(hashData);
                }
                else if (packet.Header.Status == Smb2Status.STATUS_SUCCESS)
                {
                    completedSessionSet.Add(sessionId);
                }
            }
        }

        /// <summary>
        /// Gets the preauthentication integrity hash for the session specified by the sessionId
        /// </summary>
        /// <param name="sessionId">The session Id.</param>
        /// <returns>The preauthentication integrity hash value of the given session. 
        /// Return null if the specified session is not found.</returns>
        public byte[] GetSessionPreauthIntegrityHashValue(ulong sessionId)
        {
            if (sessionPreauthIntegrityHashValueTable.ContainsKey(sessionId) &&
                this.completedSessionSet.Contains(sessionId))
            {
                return sessionPreauthIntegrityHashValueTable[sessionId];
            }
            else
            {
                return null;
            }
        }

        // Generates a zero hash value for the context.
        private byte[] ZeroHashValue
        {
            get
            {
                if (preauthIntegrityHashID == Smb2.PreauthIntegrityHashID.SHA_512)
                {
                    return new byte[64];
                }
                else
                {
                    // for future compability
                    return new byte[64];
                }
            }
        }

        // Creates a HashAlgorithm instance for the context.
        private HashAlgorithm CreateHashAlgorithm()
        {
            if (preauthIntegrityHashID == PreauthIntegrityHashID.SHA_512)
            {
                return new SHA512Managed();
            }
            else
            {
                // for future compability
                return new SHA512Managed();
            }
        }
    }

    

}
