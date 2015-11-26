// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Modeling;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.CreateClose
{
    public delegate void CreateResponseEventHandler(ModelSmb2Status status, CreateCloseConfig config);
    public delegate void CloseResponseEventHandler(ModelSmb2Status status, QueryResponseStatus queryResponseStatus);

    public interface ICreateCloseAdapter : IAdapter
    {
        event CreateResponseEventHandler CreateResponse;
        event CloseResponseEventHandler CloseResponse;

        void ReadConfig(out CreateCloseConfig c);

        void SetupConnection(ModelDialectRevision maxSmbVersionClientSupported);

        void CreateRequest(
            CreateFileNameType fileNameType,
            CreateOptionsFileOpenReparsePointType fileOpenReparsePointType,
            CreateOptionsFileDeleteOnCloseType fileDeleteOnCloseType,
            CreateContextType contextType,
            ImpersonationLevelType impersonationType,
            CreateFileType fileType);
        
        void CloseRequest(
            CloseFlagType closeFlagType,                        
            FileIdVolatileType volatileType,                
            FileIdPersistentType persistentType);          
    }
}
