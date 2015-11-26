// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Modeling;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.AppInstanceId
{
    public delegate void OpenResponseEventHandler(OpenStatus status);

    public interface IAppInstanceIdAdapter: IAdapter
    {
        void ReadConfig(out ModelDialectRevision dialectRevision);

        void PrepareOpen(
            ModelDialectRevision dialect,
            AppInstanceIdType appInstanceIdType,
            CreateType createType);

        void OpenRequest(
            ClientGuidType clientGuidType,
            PathNameType pathNameType,
            CreateType createType,
            ShareType shareType,
            AppInstanceIdType appInstanceIdType);

        event OpenResponseEventHandler OpenResponse;
    }
}
