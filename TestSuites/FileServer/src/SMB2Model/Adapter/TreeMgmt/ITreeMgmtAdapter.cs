// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.TreeMgmt
{
    public delegate void TreeConnectEventHandler(ModelSmb2Status status, ShareType_Values shareType, TreeMgmtServerConfig config);
    public delegate void TreeDisconnectEventHandler(ModelSmb2Status status);

    public interface ITreeMgmtAdapter: IAdapter
    {
        event TreeConnectEventHandler TreeConnectResponse;
        event TreeDisconnectEventHandler TreeDisconnectResponse;

        void ReadConfig(out TreeMgmtServerConfig config);

        /// <summary>
        /// 1. Negotiate
        /// 2. Session Setup
        /// </summary>
        /// <param name="securityContext"></param>
        void SetupConnection(ModelSessionSecurityContext securityContext);

        void TreeConnectRequest(ModelSharePath sharePath);

        void TreeDisconnectRequest(ModelTreeId treeId);
    }
}
