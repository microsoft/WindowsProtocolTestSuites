// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Microsoft.Protocol.TestSuites.Smb
{
    /// <summary>
    /// SUT set up adapter.
    /// </summary>
    public class SutSetupAdapterCallPs : SmbAdapter, IServerSetupAdapter
    {
        #region model events

        /// <summary>
        /// Sut set up reponse handler.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event SutSetUpResponseHandler ServerSetupResponse;

        #endregion

        #region action request
        /// <summary>
        /// Config the SUT.
        /// </summary>
        /// <param name="fileSystemType">The file system type of the server.</param>
        /// <param name="serverSignState">Indicate what the message signing policy of server.</param>
        /// <param name="isSupportDfs">Indicate whether the server supports DFS.</param>
        /// <param name="isSupportPreviousVersion">Indicate whether the server will support previous version.</param>
        /// <param name="isMessageModePipe">Indicate the adapter to setup a message mode pipe or byte mode pipe</param>
        public void ServerSetup(
            FileSystemType fileSystemType,
            SignState serverSignState,
            bool isSupportDfs,
            bool isSupportPreviousVersion,
            bool isMessageModePipe)
        {
            bool isInteractiveAdapterUsed = bool.Parse(Site.Properties.Get("SutInteractiveAdapterIsUsed"));

            if (isInteractiveAdapterUsed)
            {
                IDeleteFilesInteractiveAdapter deleteFileAdapter = Site.GetAdapter<ISutInteractiveAdapter>();

                deleteFileAdapter.DeleteFiles();

                IConfigSignStateInteractiveAdapter configSignStateAdapter
                    = Site.GetAdapter<ISutInteractiveAdapter>();

                string sutName = Site.Properties["SutMachineName"];

                switch (serverSignState)
                {
                    case SignState.Disabled:
                        configSignStateAdapter.ConfigSignState(sutName, "Disabled");
                        break;
                    case SignState.Enabled:
                        configSignStateAdapter.ConfigSignState(sutName, "Enabled");
                        break;
                    case SignState.Required:
                        configSignStateAdapter.ConfigSignState(sutName, "Required");
                        break;
                    case SignState.DisabledUnlessRequired:
                        configSignStateAdapter.ConfigSignState(sutName, "DisabledUnlessRequired");
                        break;
                }
            }
            else
            {
                IDeleteFilesAdapter deleteFileAdapter = Site.GetAdapter<IDeleteFilesAdapter>();

                deleteFileAdapter.DeleteFiles();

                IConfigSignStateAdapter configSignStateAdapter = Site.GetAdapter<IConfigSignStateAdapter>();

                string sutName = Site.Properties["SutMachineName"];

                switch (serverSignState)
                {
                    case SignState.Disabled:
                        configSignStateAdapter.ConfigSignState(sutName, "Disabled");
                        break;
                    case SignState.Enabled:
                        configSignStateAdapter.ConfigSignState(sutName, "Enabled");
                        break;
                    case SignState.Required:
                        configSignStateAdapter.ConfigSignState(sutName, "Required");
                        break;
                    case SignState.DisabledUnlessRequired:
                        configSignStateAdapter.ConfigSignState(sutName, "DisabledUnlessRequired");
                        break;
                }
            }

            int totalByteWritten = 1;
            bool isSupportResumeKey = true;
            bool isSupportCopyChunk = true;
            if (fileSystemType == FileSystemType.Ntfs)
            {
                SmbAdapter.FsType = "Ntfs";
            }
            else
            {
                SmbAdapter.FsType = "Fat";
            }

            SmbAdapter.state = serverSignState;
            SmbAdapter.fileSystemType = fileSystemType;
            SmbAdapter.isSupportDfs = isSupportDfs;
            SmbAdapter.isMessageModePipe = isMessageModePipe;
            SmbAdapter.isPreviousVersion = isSupportPreviousVersion;

            SmbAdapter.isSupportStream = true;
            SmbAdapter.isSupportInfoLevelPassThru = true;
            SmbAdapter.isSupportNtSmb = true;
            SmbAdapter.isRapServerActive = true;
            this.ServerSetupResponse(
                totalByteWritten,
                SmbAdapter.isSupportInfoLevelPassThru,
                SmbAdapter.isSupportNtSmb,
                SmbAdapter.isRapServerActive,
                isSupportResumeKey,
                isSupportCopyChunk);
        }

        /// <summary>
        /// Create the named pipe and mailslot.
        /// Make sure the named pipe and mailslot are already available before using.
        /// </summary>
        /// <param name="pipes">All the pipes will be created on SMB server.</param>
        /// <param name="mailslot">All the mailslots will be created on SMB server.</param>
        /// <param name="isCreatePipeStatus">Indicate the status of Create named pipe and mailslot operation.</param>
        public void CreatePipeAndMailslot(
            Microsoft.Modeling.Set<string> pipes,
            Microsoft.Modeling.Set<string> mailslot,
            out bool isCreatePipeStatus)
        {
            isCreatePipeStatus = true;
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
