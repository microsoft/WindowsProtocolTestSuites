// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nlmp
{
    /// <summary>
    /// this class defines the client of nlmp sdk.
    /// it declares the packet api. it also holds the context and the config information.
    /// </summary>
    public class NlmpClient
    {
        #region Fields

        /// <summary>
        /// the config information of client. e.g. the version of nlmp.
        /// </summary>
        private NlmpClientConfig config;

        /// <summary>
        /// the client context which holds the runtime information of nlmp.
        /// </summary>
        private NlmpClientContext context;

        #endregion

        #region Properties

        /// <summary>
        /// the config information of client. e.g. the version of nlmp.
        /// </summary>
        public NlmpClientConfig Config
        {
            get
            {
                return this.config;
            }
            set
            {
                this.config = value;
            }
        }


        /// <summary>
        /// the client context which holds the runtime information of nlmp.
        /// </summary>
        public NlmpClientContext Context
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


        #endregion

        #region Constructor

        /// <summary>
        /// constructor of NlmpClient
        /// </summary>
        /// <param name="config">
        /// the config for client. provide the config information, such as the version of nlmp.
        /// </param>
        public NlmpClient(
            NlmpClientConfig config
            )
        {
            this.Config = config;
            this.Context = new NlmpClientContext();
        }


        #endregion

        #region PacketAPI

        /// <summary>
        /// this function create a negotiate packet.
        /// client send the negotiate packet to server to indicate the supported capabilities.
        /// in connection-oriented mode, this is the first packet that client send to server.
        /// </summary>
        /// <param name="negotiateFlags">this flags indicates the capabilities that client supports.</param>
        /// <param name="version">
        /// This structure is used for debugging purposes only. In normal (non-debugging) protocol messages, it is 
        /// ignored and does not affect the NTLM message processing.
        /// </param>
        /// <param name="domainName">
        /// DomainName contains the name of the client authentication domain that MUST be encoded using the OEM 
        /// character set. This param can not be null.
        /// </param>
        /// <param name="workstationName">
        /// WorkstationName contains the name of the client machine that MUST be encoded using the OEM character 
        /// set. Otherwise, this data is not present. This param can not be null.
        /// </param>
        /// <returns>the negotiate packet</returns>
        /// <exception cref="ArgumentNullException">domainName must not be null</exception>
        /// <exception cref="ArgumentNullException">workstationName must not be null</exception>
        /// <exception cref="ArgumentException">
        /// when version is required, the domainName and workstationName must be string.Empty
        /// </exception>
        public NlmpNegotiatePacket CreateNegotiatePacket(
            NegotiateTypes negotiateFlags,
            VERSION version,
            string domainName,
            string workstationName
            )
        {
            if (NlmpUtility.IsConnectionless(negotiateFlags))
            {
                throw new NotSupportedException("NEGOTIATE message is not supported under Connectionless mode.");
            }

            if (domainName == null)
            {
                throw new ArgumentNullException("domainName");
            }

            if (workstationName == null)
            {
                throw new ArgumentNullException("workstationName");
            }

            #region Parameter validation

            if (NlmpUtility.IsVersionRequired(negotiateFlags))
            {
                if (domainName.Length != 0)
                {
                    throw new ArgumentException(
                        "when version is required, the domainName should be string.Empty!", "domainName");
                }

                if (workstationName.Length != 0)
                {
                    throw new ArgumentException(
                        "when version is required, the workstationName should be string.Empty!", "workstationName");
                }
            }
            else
            {
                if (NlmpUtility.IsDomainNameSupplied(negotiateFlags) && domainName.Length == 0)
                {
                    throw new ArgumentException(
                        "when version is not required, the domainName should not be string.Empty!", "domainName");
                }

                if (NlmpUtility.IsWorkstationSupplied(negotiateFlags) && workstationName.Length == 0)
                {
                    throw new ArgumentException(
                        "when version is not required, the workstationName should not be string.Empty!", 
                        "workstationName");
                }
            }

            #endregion

            NlmpNegotiatePacket packet = new NlmpNegotiatePacket();

            packet.SetNegotiateFlags(negotiateFlags);

            if (NlmpUtility.IsVersionRequired(negotiateFlags))
            {
                packet.SetVersion(version);
            }
            else
            {
                if (NlmpUtility.IsDomainNameSupplied(negotiateFlags))
                {
                    packet.SetDomainName(domainName);
                }

                if (NlmpUtility.IsWorkstationSupplied(negotiateFlags))
                {
                    packet.SetWorkstationName(workstationName);
                }
            }

            return packet;
        }


        /// <summary>
        /// this function create an authenticate packet.
        /// after client received the challenge packet from server, client create an authenticate packet to server.
        /// the authenticate packet contains the authentication information of user that is generated by the credential
        /// of user.
        /// this function does not set the mic field of packet. it must be set manually if need.
        /// </summary>
        /// <param name="negotiateFlags">this flags indicates the capabilities that client supports.</param>
        /// <param name="version">
        /// This structure is used for debugging purposes only. In normal (non-debugging) protocol messages, it is 
        /// ignored and does not affect the NTLM message processing.
        /// </param>
        /// <param name="lmChallengeResponse">
        /// An LM_RESPONSE or LMv2_RESPONSE structure that contains the computed LM response to the challenge. If 
        /// NTLM v2 authentication is configured, LmChallengeResponse MUST be an LMv2_RESPONSE structure. Otherwise, 
        /// it MUST be an LM_RESPONSE structure.
        /// </param>
        /// <param name="ntChallengeResponse">
        /// An NTLM_RESPONSE or NTLMv2_RESPONSE structure that contains the computed NT response to the challenge. 
        /// If NTLM v2 authentication is configured, NtChallengeResponse MUST be an NTLMv2_RESPONSE. Otherwise, it 
        /// MUST be an NTLM_RESPONSE structure.
        /// </param>
        /// <param name="domainName">
        /// The domain or computer name hosting the user account. DomainName MUST be encoded in the negotiated 
        /// character set. This param can not be null.
        /// </param>
        /// <param name="userName">
        /// The name of the user to be authenticated. UserName MUST be encoded in the negotiated character set. This 
        /// param can not be null.
        /// </param>
        /// <param name="workstation">
        /// The name of the computer to which the user is logged on. Workstation MUST be encoded in the negotiated 
        /// character set. This param can not be null.
        /// </param>
        /// <param name="encryptedRandomSessionKey">
        /// The client's encrypted random session key. This param can be null.
        /// </param>
        /// <returns>the authenticate packet</returns>
        /// <exception cref="System.ArgumentNullException">
        /// when LM is using, the ntChallengeResponse should be null!
        /// </exception>
        public NlmpAuthenticatePacket CreateAuthenticatePacket(
            NegotiateTypes negotiateFlags,
            VERSION version,
            byte[] lmChallengeResponse,
            byte[] ntChallengeResponse,
            string domainName,
            string userName,
            string workstation,
            byte[] encryptedRandomSessionKey
            )
        {
            #region Parameter check

            if (domainName == null)
            {
                throw new ArgumentNullException("domainName");
            }

            if (userName == null)
            {
                throw new ArgumentNullException("userName");
            }

            if (workstation == null)
            {
                throw new ArgumentNullException("workstation");
            }

            if (NlmpUtility.IsLm(negotiateFlags) && ntChallengeResponse != null)
            {
                throw new ArgumentException(
                    "when LM is using, the ntChallengeResponse should be null!", "ntChallengeResponse");
            }

            #endregion

            NlmpAuthenticatePacket packet = new NlmpAuthenticatePacket();

            // update flags
            packet.SetNegotiateFlags(negotiateFlags);

            // if version required, update version
            if (NlmpUtility.IsVersionRequired(negotiateFlags))
            {
                packet.SetVersion(version);
            }

            // update domain name
            packet.SetDomainName(domainName);

            // update user name
            packet.SetUserName(userName);

            // if version required, update workstation
            if (NlmpUtility.IsVersionRequired(negotiateFlags))
            {
                packet.SetWorkstation(workstation);
            }

            // update lmChallengeResponse
            packet.SetLmChallengeResponse(lmChallengeResponse);

            // update ntChallengeResponse
            packet.SetNtChallengeResponse(ntChallengeResponse);

            // update encryptedRandomSessionKey
            packet.SetEncryptedRandomSessionKey(encryptedRandomSessionKey);

            return packet;
        }


        #endregion
    }
}
