// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// Contains global setting of client
    /// </summary>
    public class Smb2CommonGlobalContext : FileServiceClientContext
    {
        #region Properties

        /// <summary>
        /// A Boolean that, if set, indicates that this node requires that messages MUST be signed 
        /// if the message is sent with a user security context that is neither anonymous nor guest.
        /// If not set, this node does not require that any messages be signed, but MAY still choose
        /// to do so if the other node requires it
        /// </summary>
        public bool RequireMessageSigning
        {
            get;
            set;
        }

        #endregion
    }
}
