// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// RPCE packet types, both CO and CL.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum RpcePacketType : byte
    {
        /// <summary>
        /// CO/CL, request PDU
        /// </summary>
        Request = 0,

        /// <summary>
        /// CL, ping PDU
        /// </summary>
        Ping = 1,

        /// <summary>
        /// CO/CL, response PDU
        /// </summary>
        Response = 2,

        /// <summary>
        /// CO/CL, fault PDU
        /// </summary>
        Fault = 3,

        /// <summary>
        /// CL, working PDU
        /// </summary>
        Working = 4,

        /// <summary>
        /// CL, nocall PDU
        /// </summary>
        Nocall = 5,

        /// <summary>
        /// CL, reject PDU
        /// </summary>
        Reject = 6,

        /// <summary>
        /// CL, ack PDU
        /// </summary>
        Ack = 7,

        /// <summary>
        /// CL, cl_cancel PDU
        /// </summary>
        ClCancel = 8,

        /// <summary>
        /// CL, fack PDU
        /// </summary>
        Fack = 9,

        /// <summary>
        /// CL, cancel_ack PDU
        /// </summary>
        CancelAck = 10,

        /// <summary>
        /// CO, bind PDU
        /// </summary>
        Bind = 11,

        /// <summary>
        /// CO, bind_ack PDU
        /// </summary>
        BindAck = 12,

        /// <summary>
        /// CO, bind_nak PDU
        /// </summary>
        BindNak = 13,

        /// <summary>
        /// CO, alter_context PDU
        /// </summary>
        AlterContext = 14,

        /// <summary>
        /// CO, alter_context_resp PDU
        /// </summary>
        AlterContextResp = 15,

        /// <summary>
        /// CO, shutdown PDU
        /// </summary>
        Shutdown = 17,

        /// <summary>
        /// CO, co_cancel PDU
        /// </summary>
        CoCancel = 18,

        /// <summary>
        /// CO, orphaned PDU
        /// </summary>
        Orphaned = 19,

        /// <summary>
        /// CO, auth_3 PDU<para/>
        /// Supported by RPCE.
        /// </summary>
        Auth3 = 16
    }
}
