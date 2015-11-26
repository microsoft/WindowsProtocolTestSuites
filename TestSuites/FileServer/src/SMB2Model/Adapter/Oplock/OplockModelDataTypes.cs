// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Text;
using Microsoft.Modeling;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Oplock
{
    public struct OplockConfig
    {
        public Platform Platform;

        public ModelDialectRevision MaxSmbVersionSupported;

        public override string ToString()
        {
            StringBuilder outputInfo = new StringBuilder();
            outputInfo.AppendFormat("{0}: \r\n", "OplockConfig State");
            outputInfo.AppendFormat("{0}: {1} \r\n", "Platform", this.Platform.ToString());
            outputInfo.AppendFormat("{0}: {1} \r\n", "MaxSmbVersionSupported", this.MaxSmbVersionSupported.ToString());

            return outputInfo.ToString();
        }
    }

    public enum OplockFileOperation
    {
        CreateAnotherOpen,

        WriteFromAnotherOpen
    }

    public enum ModelShareType
    {
        NO_STYPE_CLUSTER_SOFS,

        STYPE_CLUSTER_SOFS
    }

    public enum ModelShareFlag
    {
        NO_SMB2_SHAREFLAG_FORCE_LEVELII_OPLOCK,

        SMB2_SHAREFLAG_FORCE_LEVELII_OPLOCK
    }

    public enum OplockVolatilePortion
    {
        VolatilePortionFound,

        VolatilePortionNotFound
    }

    public enum OplockPersistentPortion
    {
        PersistentMatchesDurableFileId,

        PersistentNotMatchesDurableFileId
    }
}
