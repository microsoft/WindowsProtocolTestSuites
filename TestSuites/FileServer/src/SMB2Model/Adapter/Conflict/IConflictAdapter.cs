// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using Microsoft.Modeling;
using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Conflict
{
    public delegate void ConflictResponseEventHandler(ModelSmb2Status status, LeaseBreakState leaseBreakState);

    public interface IConflictAdapter : IAdapter
    {
        void Preparation();
        void ConflictRequest(RequestType firstRequest, RequestType secondRequest);
        event ConflictResponseEventHandler ConflictResponse;
    }
}
