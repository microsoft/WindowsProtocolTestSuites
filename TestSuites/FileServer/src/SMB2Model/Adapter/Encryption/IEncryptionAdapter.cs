// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Modeling;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Encryption
{
    public delegate void SessionSetupResponseEventHandler(ModelSmb2Status status, SessionEncryptDataType sessionEncryptDataType, EncryptionConfig c);
    public delegate void TreeConnectResponseEventHandler(ModelSmb2Status status, ShareEncryptDataType shareEncryptDataType, ModelResponseType modelResponseType, EncryptionConfig c);
    public delegate void FileOperationVerifyEncryptionResponseEventHandler(ModelSmb2Status status, ModelResponseType modelResponseType, EncryptionConfig c);
    public delegate void DisconnectionEventHandler();

    public interface IEncryptionAdapter : IAdapter
    {
        event SessionSetupResponseEventHandler SessionSetupResponse;
        event TreeConnectResponseEventHandler TreeConnectResponse;
        event FileOperationVerifyEncryptionResponseEventHandler FileOperationVerifyEncryptionResponse;
        event DisconnectionEventHandler ExpectDisconnect;

        void ReadConfig(out EncryptionConfig c);
        void SetupConnection(ModelDialectRevision maxSmbVersionClientSupported, ClientSupportsEncryptionType clientSupportsEncryptionType);
        void SessionSetupRequest();
        void TreeConnectRequest(ConnectToShareType connectToShareType, ModelRequestType modelRequestType);
        void FileOperationVerifyEncryptionRequest(ModelRequestType modelRequestType);
    }
}
