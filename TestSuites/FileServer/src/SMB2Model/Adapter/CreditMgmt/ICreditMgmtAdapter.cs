// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.CreditMgmt
{
    public delegate void CreditOperationEventHandler(ModelSmb2Status status, uint creditResponse, CreditMgmtConfig c);
    public delegate void DisconnectionEventHandler();

    public interface ICreditMgmtAdapter : IAdapter
    {
        event CreditOperationEventHandler CreditOperationResponse;
        event DisconnectionEventHandler ExpectDisconnect;

        void ReadConfig(out CreditMgmtConfig c);

        void SetupConnection(ModelDialectRevision clientMaxDialect);

        void CreditOperationRequest(
            ModelMidType midType,
            ModelCreditCharge creditCharge,
            ModelCreditRequestNum creditRequestNum,
            ModelPayloadSize payloadSize,
            ModelPayloadType payloadType);

    }
}
