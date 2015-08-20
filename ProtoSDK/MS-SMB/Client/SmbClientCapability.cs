// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// capabilities variables for smb client side.
    /// </summary>
    public class SmbClientCapability : SmbCapability
    {
        #region Fields

        /// <summary>
        /// the sub command for all smb transactions. such as SMB_COM_TRANSACTION, SMB_COM_TRANSACTION2 and 
        /// SMB_COM_NT_TRANSACTION. 
        /// </summary>
        private TransSubCommandExtended transactionSubCommand;

        /// <summary>
        /// A state that determines whether this node signs messages. 
        /// </summary>
        private SignState clientSignState;

        #endregion

        #region Runtime Variables

        /// <summary>
        /// the sub command for all smb transactions. such as SMB_COM_TRANSACTION, SMB_COM_TRANSACTION2 and 
        /// SMB_COM_NT_TRANSACTION. 
        /// </summary>
        public TransSubCommandExtended TransactionSubCommand
        {
            get
            {
                return transactionSubCommand;
            }
            set
            {
                transactionSubCommand = value;
            }
        }


        #endregion

        #region Capabilities Variables

        /// <summary>
        /// A state that determines whether this node signs messages. 
        /// </summary>
        public SignState ClientSignState
        {
            get
            {
                return this.clientSignState;
            }
            set
            {
                this.clientSignState = value;
            }
        }


        #endregion

        #region Constructor

        /// <summary>
        /// constructor 
        /// </summary>
        internal SmbClientCapability()
            : base()
        {
            this.clientSignState = SignState.DISABLED;
        }


        #endregion
    }
}
