// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// capabilities variables for smb  server side
    /// </summary>
    public class SmbServerCapability : SmbCapability
    {
        #region Fields

        /// <summary>
        /// A state that determines whether this node signs messages.
        /// </summary>
        private SignState serverSignState;

        #endregion

        #region Default Values

        /// <summary>
        /// When true, this SMB is being sent from the server in response to a client request. The Command     PROGRAM
        /// 1.0 field usually contains the same value in a protocol request from the client to the server as in the 
        /// matching response from the server to the client. This bit unambiguously distinguishes the command request 
        /// from the command response. 
        /// </summary>
        public bool IsFromServer
        {
            get
            {
                return SmbHeader_Flags_Values.FROM_SERVER
                    == (this.flag & SmbHeader_Flags_Values.FROM_SERVER);
            }
            set
            {
                if (value)
                {
                    this.flag |= SmbHeader_Flags_Values.FROM_SERVER;
                }
                else
                {
                    this.flag &= ~SmbHeader_Flags_Values.FROM_SERVER;
                }
            }
        }


        #endregion

        #region Capabilities Variables

        /// <summary>
        /// A state that determines whether this node signs messages. 
        /// </summary>
        public SignState ServerSignState
        {
            get
            {
                return this.serverSignState;
            }
            set
            {
                this.serverSignState = value;
            }
        }


        #endregion

        #region Constructor

        /// <summary>
        /// constructor 
        /// </summary>
        internal SmbServerCapability()
            : base()
        {
            this.serverSignState = SignState.DISABLED;
            this.IsFromServer = true;
        }


        #endregion
    }
}
