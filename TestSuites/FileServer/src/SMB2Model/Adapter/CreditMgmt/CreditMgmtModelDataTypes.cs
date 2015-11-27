// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.CreditMgmt
{
    /// <summary>
    /// Server configuration for credit management
    /// </summary>
    public struct CreditMgmtConfig
    {
        /// <summary>
        /// Max SMB2 dialect that server supports
        /// </summary>
        public ModelDialectRevision MaxSmbVersionSupported;

        public Platform Platform;

        /// <summary>
        /// Assistant server configuration in case server does not support
        /// multicredit even implements 2.1 or 3.0
        /// </summary>
        public bool IsMultiCreditSupportedOnServer;
    }

    /// <summary>
    /// Type of message id to be sent to server in request header
    /// </summary>
    public enum ModelMidType
    {
        /// <summary>
        /// Message id has been used
        /// </summary>
        UsedMid,

        /// <summary>
        /// Message id is neither smallest nor largest one in current sequence window
        /// </summary>
        ValidMid,

        /// <summary>
        /// Message id is not used and not in current sequence window
        /// Similar to UsedMid?
        /// </summary>
        UnavailableMid
    }

    /// <summary>
    /// Type of credit charge to be sent to server in request header
    /// </summary>
    public enum ModelCreditCharge
    {
        /// <summary>
        /// Credit charge is zero
        /// </summary>
        CreditChargeSetZero,

        /// <summary>
        /// Mid+CreditCharge don't reach the boundary of sequence window
        /// </summary>
        CreditChargeWithinBoundary,

        /// <summary>
        /// Mid+CreditCharge exceed the boundary of sequence window
        /// </summary>
        CreditChargeExceedBoundary
    }

    /// <summary>
    /// Type of credit request to be sent to server in request header
    /// </summary>
    public enum ModelCreditRequestNum
    {
        /// <summary>
        /// Request 0 credit from server
        /// </summary>
        CreditRequestSetZero,

        /// <summary>
        /// Request non 0 credit from server
        /// </summary>
        CreditRequestSetNonZero
    }

    /// <summary>
    /// Type of payload size of request/response send to/receive from server
    /// </summary>
    public enum ModelPayloadSize
    {
        /// <summary>
        /// <para>If server supports multi-credit and credit charge!=0, treat max(SendPayloadSize, Expected ResponsePayloadSize) – 1) / 65536 + 1 < CreditCharge</para>
        /// <para>Or if server supports multi-credit and credit charge==0, treat PayloadSize < 64*1024 </para>
        /// <para>Or if server does not support multi-credit request, treat PayloadSize < 68*1024 </para>
        /// </summary>
        PayloadSizeLessThanBoundary,

        /// <summary>
        /// <para>If server supports multi-credit and credit charge!=0, max(SendPayloadSize, Expected ResponsePayloadSize) – 1) / 65536 + 1 = CreditCharge</para>
        /// <para>Or if server supports multi-credit and credit charge==0, PayloadSize = 64*1024 </para>
        /// <para>Or if server does not support multi-credit, treat PayloadSize = 68*1024 if  request</para>
        /// </summary>
        PayloadSizeEqualToBoundary,

        /// <summary>
        /// <para>If server supports multi-credit and credit charge!=0, treat max(SendPayloadSize, Expected ResponsePayloadSize) – 1) / 65536 + 1 > CreditCharge  </para>
        /// <para>Or if server supports multi-credit and credit charge==0, treat PayloadSize > 64*1024</para>
        /// <para>Or if server does not support multi-credit request, treat PayloadSize > 68*1024 if</para>
        /// </summary>
        PayloadSizeLargerThanBoundary
    }

    /// <summary>
    /// Indicates whether the payload to be verified is in request/response
    /// </summary>
    public enum ModelPayloadType
    {
        /// <summary>
        /// Request payload to be verified
        /// e.g. WRITE
        /// </summary>
        RequestPayload,

        /// <summary>
        /// Response payload to be verified
        /// e.g. READ
        /// </summary>
        ResponsePayload
    }
}
