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

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.ValidateNegotiateInfo
{
    public delegate void ValidateNegotiateInfoResponseEventHandler(ModelSmb2Status status, ValidateNegotiateInfoConfig config);
    public delegate void TerminateConnectionEventHandler();
    public interface IValidateNegotiateInfoAdapter: IAdapter
    {
        event ValidateNegotiateInfoResponseEventHandler ValidateNegotiateInfoResponse;

        event TerminateConnectionEventHandler TerminateConnection;

        void ReadConfig(out ValidateNegotiateInfoConfig c);

        void SetupConnection(ModelDialectRevision dialect, ModelCapabilities capabilities, SecurityMode_Values securityMode);

        void ValidateNegotiateInfoRequest(DialectType dialectType, CapabilitiesType capabilitiesType, SecurityModeType securityModeType, ClientGuidType clientGuidType);
    }
}
