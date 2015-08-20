// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

using Microsoft.Modeling;

namespace Microsoft.Protocol.TestSuites.Smb
{
    /// <summary>
    /// This class is used to store information when one connection is established.
    /// </summary>
    public class SmbConnection
    {
        #region Abstract data maintained by client

        /// <summary>
        /// It indicates the client support extended security.
        /// </summary>
        public bool isClientSupportExtSecurity;

        /// <summary>
        /// It indicates the signing policy of the System Under Test (the SUT).
        /// </summary>
        public SignState sutSignState;

        /// <summary>
        /// The capabilities of the SUT.
        /// </summary>
        public Set<Capabilities> sutCapabilities;

        /// <summary>
        /// A boolean value that indicates whether a negotiation packet has been sent for this connection.
        /// </summary>
        public bool isNegotiateSent;

        /// <summary>
        /// A list of authenticated sessions that has been established on this SMB connection and the associated state.
        /// </summary>
        public MapContainer<int, SmbSession> sessionList;

        /// <summary>
        /// A list of the tree connected over this SMB connection are established to be shared on the target the SUT.
        /// </summary>
        public MapContainer<int, SmbTree> treeConnectList;

        #endregion

        #region Abstract data maintained by the SUT

        /// <summary>
        /// It is number of sequence the SUT will receive next.
        /// </summary>
        public int SutNextReceiveSequenceNumber;

        /// <summary>
        /// It is number of sequence the SUT will send .
        /// </summary>
        public SetContainer<int> SutSendSequenceNumber;

        /// <summary>
        /// The capabilities of the client.
        /// </summary>
        public Set<Capabilities> clientCapabilities;

        #endregion

        #region Abstract data maintained by model

        /// <summary>
        /// All opened files are stored in this container.
        /// </summary>
        public MapContainer<int, SmbFile> openedFiles;

        /// <summary>
        /// All opened pipes are stored in this container.
        /// </summary>
        public MapContainer<int, SmbPipe> openedPipes;

        /// <summary>
        /// All opened mailslots are stored in this container.
        /// </summary>
        public MapContainer<int, SmbMailslot> openedMailslots;

        /// <summary>
        /// To keep track the message being sent.
        /// </summary>
        public MapContainer<int, SmbRequest> sentRequest;

        /// <summary>
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </summary>
        public int sessionId;

        /// <summary>
        /// This is used to indicate the share that the client is accessing.
        /// </summary>
        public int treeId;

        /// <summary>
        /// The SMB file identifier of the target directory
        /// </summary>
        public int fId;

        /// <summary>
        /// To store search handler ID.
        /// </summary>
        public MapContainer<int, int> searchHandlerContainer;

        /// <summary>
        /// It indicates the client's sign state.
        /// </summary>
        public SignState clientSignState;

        /// <summary>
        /// A boolean value that indicates whether message signing is active for this SMB connection or not.
        /// </summary>
        public bool isSigningActive;

        /// <summary>
        /// Specify the different account type for this session.
        /// </summary>
        public AccountType accountType;

        #endregion

        /// <summary>
        /// SMB Connection constructor.
        /// </summary>
        /// Disable warning CA1805 because model need to initialize these variables for test case design.
        [SuppressMessage("Microsoft.Performance", "CA1805:DoNotInitializeUnnecessarily")]
        public SmbConnection()
        {
            isClientSupportExtSecurity = false;
            sutSignState = SignState.Disabled;
            sutCapabilities = new Set<Capabilities>();
            isNegotiateSent = false;
            sessionList = new MapContainer<int, SmbSession>();
            treeConnectList = new MapContainer<int, SmbTree>();

            SutNextReceiveSequenceNumber = 1;
            SutSendSequenceNumber = new SetContainer<int>();
            clientCapabilities = new Set<Capabilities>();

            openedFiles = new MapContainer<int, SmbFile>();
            openedPipes = new MapContainer<int, SmbPipe>();
            openedMailslots = new MapContainer<int, SmbMailslot>();
            sentRequest = new MapContainer<int, SmbRequest>();
            sessionId = 0;
            treeId = 0;
            fId = 0;
            searchHandlerContainer = new MapContainer<int, int>();
            clientSignState = SignState.Disabled;
            isSigningActive = false;
            accountType = AccountType.Admin;
        }


        /// <summary>
        /// It is used to indicate whether sign is enable in this connection.
        /// </summary>
        /// <param name="clientConnectionSignState">It indicates signstate for client.</param>
        /// <param name="sutConnectionSignState">It indicates the signstate for the SUT.</param>
        /// <returns>Client sign state.</returns>
        public bool isSignEnable(SignState clientConnectionSignState, SignState sutConnectionSignState)
        {
            if ((clientConnectionSignState == SignState.Required && sutConnectionSignState != SignState.Disabled)
                    || (clientConnectionSignState == SignState.Enabled
                        && sutConnectionSignState != SignState.Disabled
                        && sutConnectionSignState != SignState.DisabledUnlessRequired)
                    || (clientConnectionSignState == SignState.DisabledUnlessRequired
                        && sutConnectionSignState == SignState.Required))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
