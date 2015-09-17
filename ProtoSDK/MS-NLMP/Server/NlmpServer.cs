// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nlmp
{
    /// <summary>
    /// The nlmp server side. This class is used to create the server side packets.
    /// </summary>
    public class NlmpServer
    {
        /// <summary>
        /// the context of nlmp server.
        /// </summary>
        private NlmpServerContext context;

        /// <summary>
        /// the context of nlmp server.
        /// </summary>
        public NlmpServerContext Context
        {
            get
            {
                return this.context;
            }
            set
            {
                this.context = value;
            }
        }


        /// <summary>
        /// constructor
        /// </summary>
        public NlmpServer()
        {
            this.context = new NlmpServerContext();
        }


        /// <summary>
        /// create a challenge packet. This packet contains CHALLENGE_MESSAGE. The CHALLENGE_MESSAGE defines an NTLM 
        /// challenge message that is sent from the server to the client. The CHALLENGE_MESSAGE is used by the server  
        /// to challenge the client to prove its identity.
        /// </summary>
        /// <param name="negotiateFlags">The client sets flags to indicate options it supports.</param>
        /// <param name="version">
        /// This structure is used for debugging purposes only. In normal (non-debugging) protocol messages, it is
        /// ignored and does not affect the NTLM message processing.
        /// </param>
        /// <param name="serverChallenge">contains the NTLM challenge. The challenge is a 64-bit nonce.</param>
        /// <param name="targetName">
        /// TargetName contains the name of the server authentication realm, and MUST be expressed in the negotiated 
        /// character set. A server that is a member of a domain returns the domain of which it is a member, and a 
        /// server that is not a member of a domain returns the server name. This param can be null.
        /// </param>
        /// <param name="targetInfo">
        /// TargetInfo contains a sequence of AV_PAIR structures.This param can be null.
        /// </param>
        /// <returns>the NlmpChallengePacket</returns>
        /// <noException></noException>
        public NlmpChallengePacket CreateChallengePacket(
            NegotiateTypes negotiateFlags,
            VERSION version,
            ulong serverChallenge,
            string targetName,
            ICollection<AV_PAIR> targetInfo
            )
        {
            NlmpChallengePacket packet = new NlmpChallengePacket();

            packet.SetNegotiateFlags(negotiateFlags);

            if (NlmpUtility.IsVersionRequired(negotiateFlags))
            {
                packet.SetVersion(version);
            }

            packet.SetServerChallenge(serverChallenge);

            if (NlmpUtility.IsDomainType(negotiateFlags) || NlmpUtility.IsServerType(negotiateFlags))
            {
                packet.SetTargetName(targetName);
            }

            // generate bytes of targetinfo
            byte[] targetInfoBytes = NlmpUtility.AvPairCollectionGetBytes(targetInfo);

            // initialize targetinfofield
            packet.SetTargetInfo(targetInfoBytes);

            return packet;
        }
    }
}
