// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Modeling;

namespace Microsoft.Protocol.TestSuites.Smb
{
    /// <summary>
    /// This class is used to store information of a session.
    /// </summary>
    public class SmbSession
    {
        /// <summary>
        /// Session ID.
        /// </summary>
        public int sessionId;

        /// <summary>
        /// Session key for this session.
        /// </summary>
        public int sessionKey;

        /// <summary>
        /// Session state.
        /// </summary>
        public SessionState sessionState;

        /// <summary>
        /// SMB session constructor.
        /// </summary>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </param>
        /// <param name="sessionState">Session state.</param>
        public SmbSession(int sessionId, SessionState sessionState)
        {
            this.sessionId = sessionId;
            this.sessionState = sessionState;
            sessionKey = -1;
        }


        /// <summary>
        /// SMB session constructor.
        /// </summary>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.</param>
        /// <param name="sessionState">Session state.</param>
        /// <param name="sessionKey">Session key.</param>
        public SmbSession(int sessionId, int sessionKey, SessionState sessionState)
        {
            this.sessionId = sessionId;
            this.sessionState = sessionState;
            this.sessionKey = sessionKey;
        }
    }
}
