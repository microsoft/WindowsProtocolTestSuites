// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Replay
{
    public delegate void CreateResponseEventHandler(
            ModelSmb2Status status,
            ReplayModelDurableHandle durableHandleResponse,
            ReplayServerConfig c);

    public delegate void FileOperationResponseEventHandler(ModelSmb2Status status, ReplayServerConfig c);

    public interface IReplayAdapter : IAdapter
    {
        event CreateResponseEventHandler CreateResponse;

        event FileOperationResponseEventHandler FileOperationResponse;

        void ReadConfig(out ReplayServerConfig c);

        void PrepareCreate(
            ModelDialectRevision maxSmbVersionClientSupported,
            ReplayModelClientSupportPersistent isClientSupportPersistent,
            ReplayModelDurableHandle modelDurableHandle,
            ReplayModelShareType shareType,
            ReplayModelRequestedOplockLevel requestedOplockLevel,
            ReplayModelLeaseState leaseState);

        void CreateRequest(
            ModelDialectRevision maxSmbVersionClientSupported,
            ReplayModelShareType shareType,
            ReplayModelClientSupportPersistent isClientSupportPersistent,
            ReplayModelSwitchChannelType switchChannelType,
            ReplayModelChannelSequenceType channelSequence,
            ReplayModelSetReplayFlag isSetReplayFlag,
            ReplayModelDurableHandle modelDurableHandle,
            ReplayModelRequestedOplockLevel requestedOplockLevel,
            ReplayModelLeaseState leaseState,
            ReplayModelFileName fileName,
            ReplayModelCreateGuid createGuid,
            ReplayModelFileAttributes fileAttributes,
            ReplayModelCreateDisposition createDisposition,
            ReplayModelLeaseKey leaseKey);

        void PrepareFileOperation(
            ModelDialectRevision maxSmbVersionClientSupported,
            ReplayModelRequestCommand requestCommand);

        void FileOperationRequest(
            ReplayModelSwitchChannelType switchChannelType,
            ModelDialectRevision maxSmbVersionClientSupported,
            ReplayModelRequestCommand requestCommand,
            ReplayModelChannelSequenceType channelSequence,
            ReplayModelSetReplayFlag isSetReplayFlag,
            ReplayModelRequestCommandParameters requestCommandParameters);
    }
}
