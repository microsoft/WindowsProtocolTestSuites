// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocol.TestSuites.Smb
{
    /// <summary>
    /// Server under test(SUT) set up response.
    /// </summary>
    /// <param name="totalBytesWritten">The totall bytes can be written to a file.</param>
    /// <param name="isSupportInfoLevelPassthru">Indicate whether the server supports Info Passthrough.</param>
    /// <param name="isSupportNtSmb">Indicate whther the server supports NT SMBs.</param>
    /// <param name="isRapServerActive">Indicate whether the RAP server is available on server machine.</param>
    /// <param name="isSupportResumeKey">Indicate whether the server will support resume key.</param>
    /// <param name="isSupportCopyChunk">Indicate whether the server will support CopyChunk.</param>
    /// Disable warning CA1009 because according to Test Case design, 
    /// the two parameters, System.Object and System.EventArgs, are unnecessary.
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
    public delegate void SutSetUpResponseHandler(
        int totalBytesWritten,
        bool isSupportInfoLevelPassthru,
        bool isSupportNtSmb,
        bool isRapServerActive,
        bool isSupportResumeKey,
        bool isSupportCopyChunk);

    /// <summary>
    /// SUT set up adapter.
    /// </summary>
    public interface IServerSetupAdapter : IAdapter
    {
        /// <summary>
        /// SMB Server setup response handler.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event SutSetUpResponseHandler ServerSetupResponse;

        /// <summary>
        /// Server under test set up.
        /// </summary>
        /// <param name="fileSystemType">The file system type of the server.</param>
        /// <param name="serverSignState">Indicate what the message signing policy of server.</param>
        /// <param name="isSupportDfs">Indicate whether the server supports DFS.</param>
        /// <param name="isSupportPreviousVersion">Indicate whether the server will support previous version.</param>
        /// <param name="isMessageModePipe">Indicate the adapter to setup a message mode pipe or byte mode pipe</param>
        void ServerSetup(
            FileSystemType fileSystemType,
            SignState serverSignState,
            bool isSupportDfs,
            bool isSupportPreviousVersion,
            bool isMessageModePipe);

        /// <summary>
        /// Create the named pipe and mailslot.
        /// Make sure the named pipe and mailslot are already available before using.
        /// </summary>
        /// <param name="pipes">All the pipes will be created on SMB server.</param>
        /// <param name="mailslot">All the mailslots will be created on SMB server.</param>
        /// <param name="createPipeStatus"> Indicate the status of Create named pipe and mailslot operation.</param>
        void CreatePipeAndMailslot(
            Microsoft.Modeling.Set<string> pipes,
            Microsoft.Modeling.Set<string> mailslot,
            out bool createPipeStatus);
    }

    /// <summary>
    /// Delete files adapter.
    /// </summary>
    public interface IDeleteFilesAdapter : IAdapter
    {
        /// <summary>
        /// Delete specific files on the SUT.
        /// </summary>
        void DeleteFiles();
    }

    /// <summary>
    /// Config sign state adapter.
    /// </summary>
    public interface IConfigSignStateAdapter : IAdapter
    {
        /// <summary>
        /// Config the SUT signing policy.
        /// </summary>
        /// <param name="serverName">The SUT name.</param>
        /// <param name="signState">A state that determines whether this node signs messages.</param>
        void ConfigSignState(string serverName, string signState);
    }

    /// <summary>
    /// Delete files adapter.
    /// </summary>
    public interface IDeleteFilesInteractiveAdapter : IAdapter
    {
        /// <summary>
        /// Delete specific files on the SUT.
        /// </summary>
        [MethodHelp(@"To clean up the SUT environment. For clean up, 
        delete Test1.txt, Test2.txt, NewName.txt on each shared folder. ")]
        void DeleteFiles();
    }

    /// <summary>
    /// Config sign state adapter.
    /// </summary>
    public interface IConfigSignStateInteractiveAdapter : IAdapter
    {
        /// <summary>
        /// Config the SUT signing policy.
        /// </summary>
        /// <param name="serverName">The SUT name.</param>
        /// <param name="signState">A state that determines whether this node signs messages.</param>
        [MethodHelp(@"To set up the SUT environment. For set up, add reg on SUT.
        Summary : 
        when signState is DISABLED, set enablesecuritysignature to 0,and requiresecuritysignature to 0.
        when signState is ENABLED, set enablesecuritysignature to 1,and requiresecuritysignature to 0.
        when signState is REQUIRED, set enablesecuritysignature to 0,and requiresecuritysignature to 1.
        when signState is DISABLEDUNLESSREQUIRED, set enablesecuritysignature to 0,and requiresecuritysignature to 0.")]
        void ConfigSignState(string sutName, string signState);
    }
}