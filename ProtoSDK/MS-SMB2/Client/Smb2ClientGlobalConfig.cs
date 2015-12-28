// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// Contains config about global context
    /// </summary>
    public class Smb2ClientGlobalConfig
    {
        /// <summary>
        /// A Boolean that, if set, indicates that this node requires that messages MUST be signed 
        /// if the message is sent with a user security context that is neither anonymous nor guest.
        /// If not set, this node does not require that any messages be signed, but MAY still choose
        /// to do so if the other node requires it
        /// </summary>
        public bool RequireMessageSigning
        {
            get
            {
                return true;
            }
        }

        public TimeSpan Timeout
        {
            get
            {
                return TimeSpan.FromSeconds(10);
            }
        }

        public bool ImplementedV2002
        {
            get
            {
                return true;
            }
        }

        public bool ImplementedV21
        {
            get
            {
                return true;
            }
        }

        public bool MaySignRequest
        {
            get
            {
                return false;
            }
        }

        public bool MayReuseConnection
        {
            get
            {
                return false;
            }
        }
    }
}
