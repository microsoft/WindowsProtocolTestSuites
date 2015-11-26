// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Modeling;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Signing
{
    public delegate void NegotiateResponseEventHandler(
        ModelSmb2Status status, 
        SigningEnabledType signingEnabledType, 
        SigningRequiredType signingRequiredType, 
        SigningConfig c);
    public delegate void SessionSetupResponseEventHandler(
        ModelSmb2Status status, 
        SigningModelSessionId sessionId, 
        SigningFlagType signingFlagType, 
        SessionFlags_Values sessionFlag, 
        SigningConfig c);
    public delegate void TreeConnectResponseEventHandler(ModelSmb2Status status, SigningModelSessionId sessionId, SigningFlagType signingFlagType);

    public interface ISigningAdapter : IAdapter
    {
        event NegotiateResponseEventHandler NegotiateResponse;
        event SessionSetupResponseEventHandler SessionSetupResponse;
        event TreeConnectResponseEventHandler TreeConnectResponse;

        void ReadConfig(out SigningConfig c);

        void NegotiateRequest(
            ModelDialectRevision maxSmbVersionClientSupported, 
            SigningFlagType signingFlagType, 
            SigningEnabledType signingEnabledType, 
            SigningRequiredType signingRequiredType);

        void SessionSetupRequest(SigningFlagType signingFlagType, SigningEnabledType signingEnabledType, SigningRequiredType signingRequiredType, UserType userType);

        void TreeConnectRequest(SigningFlagType signingFlagType);
    }
}
