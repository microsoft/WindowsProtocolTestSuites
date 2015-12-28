// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.SessionMgmt
{
    public delegate void SessionSetupEventHandler(ModelSmb2Status status, ModelConnectionId connectionId, SessionMgmtConfig c);
    public delegate void LogOffEventHandler(ModelSmb2Status status, ModelConnectionId connectionId, SessionMgmtConfig c);
    public delegate void DisconnectionEventHandler(ModelConnectionId connectionId, SessionMgmtConfig c);

    public interface ISessionMgmtAdapter: IAdapter
    {
        event SessionSetupEventHandler SessionSetupResponse;
        event LogOffEventHandler LogOffResponse;
        event DisconnectionEventHandler ExpectDisconnect;

        void ReadConfig(out SessionMgmtConfig c);

        void SetupConnection(ModelConnectionId connectionId, ModelDialectRevision clientMaxDialect);

        void SessionSetupRequest(
            ModelConnectionId connectionId,
            ModelSessionId sessionId,
            ModelSessionId previousSessionId,
            ModelSigned signed,
            ModelFlags flags,
            ModelUser user,
            ModelAllowReauthentication ReAuthentication);

        void LogOffRequest(ModelConnectionId connectionId, ModelSessionId sessionId);

        void TerminateConnection(ModelConnectionId connectionId);
    }
}
