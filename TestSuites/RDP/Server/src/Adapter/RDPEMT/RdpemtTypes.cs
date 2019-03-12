// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestSuites.Rdpemt
{
    /// <summary>
    /// Specify various invalid types
    /// </summary>
    public enum NegativeType
    {
        /// <summary>
        /// Not negative testing.
        /// </summary>
        None,

        /// <summary>
        /// RequestID field is invalid in Tunnel Create Request PDU.
        /// </summary>
        TunnelCreateRequest_InvalidRequestID,

        /// <summary>
        /// SecurityCookie field is invalid in Tunnel Create Request PDU.
        /// </summary>
        TunnelCreateRequest_InvalidSecurityCookie,

        /// <summary>
        /// RequestID and SecurityCookie fields are invalid in Tunnel Create Request PDU.
        /// </summary>
        TunnelCreateRequest_InvalidRequestIDAndSecurityCookie

    }

    /// <summary>
    /// Possible values for RequestID of Tunnel Create Request PDU.
    /// </summary>
    public enum RequestID_Values : UInt32
    {
        /// <summary>
        /// Invalid value
        /// </summary>
        Invalid = UInt32.MaxValue
    }
}