// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nlmp
{
    /// <summary>
    /// the context of nlmp client. the context holds the runtime information of sdk.
    /// </summary>
    public class NlmpClientContext : NlmpContext
    {
        #region Fields From TD

        /// <summary>
        /// The gss_channel_bindings_struct ([RFC2744] section 3.11). This value is optional.
        /// </summary>
        private object clientChannelBindingsUnhashed;

        #endregion

        #region Properties From Sdk

        /// <summary>
        /// A Boolean setting that disables the client from sending NTLM_AUTHENTICATE 
        /// messages.
        /// </summary>
        /// <remarks>
        /// The default value of this state variable is FALSE. This state variable is supported in Windows 7 and 
        /// Windows Server 2008 R2.
        /// </remarks>
        public bool IsClientBlocked
        {
            get
            {
                return this.ClientBlocked;
            }
            set
            {
                this.ClientBlocked = value;
            }
        }


        #endregion

        #region Properties From TD, Internal to Protocol

        /// <summary>
        /// The set of client configuration flags (section 2.2.2.5) that specify the full set of capabilities of the
        /// client.
        /// </summary>
        public NegotiateTypes ClientConfigFlags
        {
            get
            {
                return this.NegFlg;
            }
            set
            {
                this.NegFlg = value;
            }
        }


        #endregion

        #region Properties From TD, Exposed to Application

        /// <summary>
        /// Indicates that the caller wants to sign messages so that they cannot be tampered with while in transit.
        /// Setting this flag results in the NTLMSSP_NEGOTIATE_SIGN flag being set in the NegotiateFlags field of the
        /// NTLM NEGOTIATE_MESSAGE.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly")]
        public bool Integrity
        {
            set
            {
                if (value)
                {
                    this.ClientConfigFlags |= NegotiateTypes.NTLMSSP_NEGOTIATE_SIGN;
                }
            }
        }


        /// <summary>
        /// Indicates that the caller wants to sign messages so that they cannot be replayed. Setting this flag results
        /// in the NTLMSSP_NEGOTIATE_SIGN flag being set in the NegotiateFlags field of the NTLM NEGOTIATE_MESSAGE.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly")]
        public bool ReplayDetect
        {
            set
            {
                if (value)
                {
                    this.ClientConfigFlags |= NegotiateTypes.NTLMSSP_NEGOTIATE_SIGN;
                }
            }
        }


        /// <summary>
        /// Indicates that the caller wants to sign messages so that they cannot be sent out of order. Setting this
        /// flag results in the NTLMSSP_NEGOTIATE_SIGN flag being set in the NegotiateFlags field of the NTLM 
        /// NEGOTIATE_MESSAGE.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly")]
        public bool SequenceDetect
        {
            set
            {
                if (value)
                {
                    this.ClientConfigFlags |= NegotiateTypes.NTLMSSP_NEGOTIATE_SIGN;
                }
            }
        }


        /// <summary>
        /// Indicates that the caller wants to encrypt messages so that they cannot be read while in transit. If the
        /// Confidentiality option is selected by the client, NTLM performs a bitwise OR operation with the following
        /// NTLM Negotiate Flags into the ClientConfigFlags. (The ClientConfigFlags indicate which features the client
        /// host supports.)
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly")]
        public bool Confidentiality
        {
            set
            {
                if (value)
                {
                    this.ClientConfigFlags |= NegotiateTypes.NTLMSSP_NEGOTIATE_SEAL;
                    this.ClientConfigFlags |= NegotiateTypes.NTLMSSP_NEGOTIATE_KEY_EXCH;
                    this.ClientConfigFlags |= NegotiateTypes.NTLMSSP_NEGOTIATE_LM_KEY;
                    this.ClientConfigFlags |= NegotiateTypes.NTLMSSP_NEGOTIATE_EXTENDED_SESSIONSECURITY;
                }
            }
        }


        /// <summary>
        /// Indicates that the connectionless mode of NTLM is to be selected. If the Datagram option is selected by the
        /// client, then connectionless mode is used and NTLM performs a bitwise OR operation with the following NTLM
        /// Negotiate Flag into the ClientConfigFlags.
        /// </summary>
        public override bool Datagram
        {
            set
            {
                if (value)
                {
                    this.ClientConfigFlags |= NegotiateTypes.NTLMSSP_NEGOTIATE_DATAGRAM;
                }
            }
        }


        /// <summary>
        /// Indicates that the caller wants the server to know the identity of the caller, but that the server not be
        /// allowed to impersonate the caller to resources on that system. Setting this flag results in the 
        /// NTLMSSP_NEGOTIATE_IDENTIFY flag being set. Indicates that the GSS_C_IDENTIFY_FLAG flag was set in the 
        /// GSS_Init_sec_context call, as discussed in [RFC4757] section 7.1, and results in the GSS_C_IDENTIFY_FLAG
        /// flag set in the authenticator's checksum field ([RFC4757] section 7.1).
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly")]
        public bool Identify
        {
            set
            {
                if (value)
                {
                    this.ClientConfigFlags |= NegotiateTypes.NTLMSSP_NEGOTIATE_IDENTIFY;
                }
            }
        }


        /// <summary>
        /// The gss_channel_bindings_struct ([RFC2744] section 3.11). This value is optional.
        /// </summary>
        public object ClientChannelBindingsUnhashed
        {
            get
            {
                return this.clientChannelBindingsUnhashed;
            }
            set
            {
                this.clientChannelBindingsUnhashed = value;
            }
        }


        #endregion

        #region Constructor

        /// <summary>
        /// the constructor, set the default values
        /// </summary>
        internal NlmpClientContext()
            : base()
        {
        }


        #endregion
    }
}
